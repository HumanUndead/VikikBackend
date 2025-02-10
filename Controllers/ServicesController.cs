
using CMS;
using CMS.Config;
using CMS.Dapper;
using CMS.Helpers;
using CMS.Managers;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;


namespace vikik.Controllers
{
    public class ServicesController : KensoftController
    {

     // GET: 
         ActionResult Login(string name, string pass, string langID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            Language lang = new Language();
            lang.ID = langID;
            MemberManager mMgr = new MemberManager(ConnectionString);
            SQLResult sqlresult;
            CMS.User user = mMgr.UserLogin(name, pass, lang, out sqlresult);

            if (sqlresult == SQLResult.LOGIN_SUCCESS)
            {
                if (user != null && user.Isdeleted == true)
                {
                    result.Add("result", -1);
                    result.Add("message", "This User Was Deleted Pls Contact With AdminiStrator");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                result.Add("result", 1);
                result.Add("uid", user.ID);
                result.Add("name", user.Name);
                result.Add("email", user.Email);
                result.Add("gender", user.Gender);
                result.Add("phone", user.Phone);
                result.Add("isActive", user.OTPConfirmed);
                result.Add("Identity", user.Useridentity);
            }
            else
            {
                result.Add("result", "0");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult IsProductsAvailable(string langID, string prodsjson)
        {
            Language lang = new Language();
            lang.ID = langID;
            var prods = JsonConvert.DeserializeObject<List<JsonItem>>(prodsjson);
            ProductManager mgr = new ProductManager(ConnectionString);
            Dictionary<string, object> response = new Dictionary<string, object>();
            Cart CurrentCart = new Cart();
            List<Product> AlmosFinishedProd = new List<Product>();
            try
            {
                if (prods != null)
                {
                    foreach (JsonItem item in prods)
                    {
                        var Product = new Product();
                        Product prod = mgr.GetProduct(item.id, lang);
                        Product.ID = prod.ID;
                        Product.Quantity = prod.Quantity;
                        if (prod.Quantity < item.qty)
                        {
                            AlmosFinishedProd.Add(Product);
                        }
                    }
                    response.Add("Success", 1);
                    response.Add("data", AlmosFinishedProd);
                }
                else
                {
                    response.Add("Success", 0);
                }
            }
            catch (Exception)
            {


                response.Add("Success", 0);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RegisterUser(string email, string password, string name, string gender, string phone, string refID)
        {
            CMS.Config.CMSConfigurations config = new CMS.Config.CMSConfigurations();
            MemberManager mMgr = new MemberManager(ConnectionString);
            PersonManager pMgr = new PersonManager(ConnectionString);
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            int gen = Convert.ToInt32(gender);
            SQLResult sqlresult;
            string enPhone = "";
            foreach (char item in phone.ToCharArray())
            {
                enPhone += GeneralHelper.CleanArabicNumbers(item.ToString());
            }
            Guid? userID;
            int userResult;
            StringBuilder result = new StringBuilder();


            //sqlresult = mMgr.RegisterUser(email, password, name,gen, phone, string.IsNullOrEmpty(refID) ? null : (Guid?)new Guid(refID), out userID);
            sqlresult = mMgr.RegisterUser(email, password, name, enPhone, string.IsNullOrEmpty(refID) ? null : (Guid?)new Guid(refID), out userID, null, null);
            if (sqlresult == SQLResult.SUCCESS)
            {
                User newUser = mMgr.GetUser((Guid)userID);
                if (newUser != null)
                {
                    newUser.IsActive = false;
                    newUser.Gender = (UserGender)gen;
                    mMgr.SaveUserBasicInfo(newUser, password, false, out userResult);
                }
                else
                {
                    result.AppendFormat("{\"result\":0, \"message\":\"{0}\"}}", Resources.Strings.UserAlreadyExists);
                }
            }


            if (sqlresult == SQLResult.SUCCESS)
            {
                User newUser = mMgr.GetUser((Guid)userID);
                result.AppendFormat("{{\"result\":1,\"uid\":\"{0}\",\"name\":\"{1}\",\"email\":\"{2}\",\"phone\":\"{3}\",\"gender\":\"{4}\",\"isActive\":\"{5}\",\"Identity\":\"{6}\"}}", userID, name, email, enPhone, gender, false, newUser.Useridentity);
                //Session["User"] = mMgr.GetUser(userID.Value, CurrentLanguage, true);
                //cMgr.SaveUserAddress(userID.Value, null, "Main Address", Request["txtAddress1"], Request["txtAddress2"], Request["country"], Request["City"], Request["phone"], null, Request["Postal"]);

                //string body = string.Format(Resources.CubicM.EmailWelcomeBody, System.Configuration.ConfigurationManager.AppSettings["SiteURL"], HttpUtility.UrlEncode(resetID.ToString()), HttpUtility.UrlEncode(email));
                //Helper.SendEmail(System.Configuration.ConfigurationManager.AppSettings["FromEmail"], email, Resources.CubicM.EmailWelcomeSubject, body);
            }
            else
            {
                result.AppendFormat("{{\"result\":0, \"message\":\"{0}\"}}", Resources.Strings.UserAlreadyExists);

            }
            ViewBag.Result = result.ToString();
            return View("_result");
        }
        public ActionResult FacebookLogin(string facebookid, string email, string name, string langID)
        {
            int dummy;
            Language lang = new Language();
            lang.ID = langID;
            MemberManager mMgr = new MemberManager(ConnectionString);
            SQLResult sqlresult;
            CMS.User user = mMgr.GetFacebookUser(facebookid, true, lang);
            StringBuilder result = new StringBuilder();
            Language en = new Language();
            Language ar = new Language();
            en.ID = "1";
            ar.ID = "2";
            if (user == null) //user doesnt exist before
            {
                if (!string.IsNullOrEmpty(email))
                {
                    user = mMgr.GetUser(email);
                    if (user != null)
                    {

                        user.FacebookID = facebookid;
                        user.Email = email;
                        user.Name = name;
                        user.FacebookID = facebookid;
                        mMgr.SaveNewFacebookUser(user, en, out dummy);
                        mMgr.SaveNewFacebookUser(user, ar, out dummy);
                    }
                    else
                    {
                        user = new User();
                        user.Email = email;
                        user.Name = name;
                        user.FacebookID = facebookid;
                        user.IsActive = false;

                        //mohammad edits from here
                        Random ran = new Random((int)DateTime.Now.Ticks);
                        user.Nationality = name + ran.Next(100, 999);
                        //to here basicly we want a random 3 numbers
                        mMgr.SaveNewFacebookUser(user, en, out dummy);
                        mMgr.SaveNewFacebookUser(user, ar, out dummy);
                    }
                }
                //result.AppendFormat("{{\"result\":0}}");
            }
            if (user != null)
            {

                //result.AppendFormat("{{\"result\":1,\"uid\":\"{0}\",\"name\":\"{1}\",\"groupid\":\"{2}\", \"phone\":\"{3}\", \"OTPConfirmed\":{4}}}", user.ID, user.Name, user.MemberOf.Count > 0 ? user.MemberOf[0].ID.ToString() : "", user.Phone, user.OTPConfirmed.ToString().ToLower());// user.MemberOf[0]);
                result.AppendFormat("{{\"result\":1,\"uid\":\"{0}\",\"name\":\"{1}\",\"email\":\"{2}\",\"gender\":\"{3}\",\"phone\":\"{4}\",\"isActive\":\"{5}\",\"Identity\":\"{6}\"}}", user.ID, user.Name, user.Email, (int)user.Gender, user.Phone, user.IsActive, user.Useridentity);

            }
            else
            {
                result.AppendFormat("{{\"result\":0}}");
            }
            ViewBag.Result = result.ToString();
            return View("_result");
        }
        public ActionResult GetFacebookUser(string facebookid, string langID)
        {
            int dummy;
            Language lang = new Language();
            lang.ID = langID;
            MemberManager mMgr = new MemberManager(ConnectionString);
            SQLResult sqlresult;
            CMS.User user = mMgr.GetFacebookUser(facebookid, true, lang);


            StringBuilder result = new StringBuilder();
            if (user == null) //user doesnt exist before
            {
                result.AppendFormat("{{\"result\":0}}");
            }
            else
            {

                //result.AppendFormat("{{\"result\":1,\"uid\":\"{0}\",\"name\":\"{1}\",\"groupid\":\"{2}\", \"phone\":\"{3}\", \"OTPConfirmed\":{4}}}", user.ID, user.Name, user.MemberOf.Count > 0 ? user.MemberOf[0].ID.ToString() : "", user.Phone, user.OTPConfirmed.ToString().ToLower());// user.MemberOf[0]);
                result.AppendFormat("{{\"result\":1,\"uid\":\"{0}\",\"name\":\"{1}\",\"email\":\"{2}\",\"gender\":\"{3}\",\"phone\":\"{4}\",\"isActive\":\"{5}\",\"Identity\":\"{6}\"}}", user.ID, user.Name, user.Email, (int)user.Gender, user.Phone, user.IsActive, user.Useridentity);

            }

            ViewBag.Result = result.ToString();
            return View("_result");
        }
        public ActionResult PushRegister(bool android, string userid, string token, string langID)
        {
            AppManager aMgr = new AppManager(ConnectionString);
            Language lang = new Language();
            lang.ID = langID;
            aMgr.SaveUserDevice(new Guid(userid), token, langID, android);
            ViewBag.Result = "{\"success\":1}";
            return View("_result");
        }
        public JsonResult SetUserToken(string userID, string token)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            MemberManager mgr = new MemberManager(ConnectionString);
            try
            {
                User user = mgr.GetUser(new Guid(userID));

                if (user != null)
                {
                    mgr.SaveUserToken(user.ID, token);
                    result.Add("Success", 1);
                }
                else
                {
                    result.Add("Success", 0);
                    result.Add("Message", "User does not exists");
                }

            }
            catch (Exception ex)
            {
                result.Add("Success", 0);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoginByOTP(string phone, string langID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            Language lang = new Language();
            lang.ID = langID;
            MemberManager mMgr = new MemberManager(ConnectionString);
            SQLResult sqlresult;
            CMS.User user = mMgr.GetUserByPhone(phone, lang);

            if (user.Phone == phone)
            {
                if (user.Isdeleted == true)
                {
                    result.Add("result", -1);
                    result.Add("message", "This User Was Deleted Pls Contact With AdminiStrator");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else 
                {
                   string  otp=  OTPGenerate(user.ID.ToString(), phone);
                    result.Add("result", 1);
                    result.Add("uid", user.ID);
                    result.Add("isActive", user.OTPConfirmed);
                    result.Add("otp", otp);

                }
                //result.Add("result", 1);
                //result.Add("uid", user.ID);
                //result.Add("name", user.Name);
                //result.Add("email", user.Email);
                //result.Add("gender", user.Gender);
                //result.Add("phone", user.Phone);
                //result.Add("isActive", user.OTPConfirmed);
                //result.Add("Identity", user.Useridentity);
            }
            else
            {
                result.Add("result", "0");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private string OTPGenerate(string ID, string Phone)
        {
            string OTP = UserOTPGenerate(ID, false);
            SendSMS("Your Tajje verification No. is " + OTP, Phone);
            ViewBag.Result = string.Format("{{\"success\": {0}, \"id\": \"{1}\", \"otp\": \"{2}\"}}", 1, ID, OTP);
            return OTP;
        }
        private string UserOTPGenerate(string ID, bool pOTPConfrimed)
        {
            MemberManager mMgr = new MemberManager(ConnectionString);
            string OTP = GenerateOTP(5);
            mMgr.UserOTPSave(ID, OTP, pOTPConfrimed);
            return OTP;
        }
        public ActionResult UserVerifyOTP(string ID, string OTP)
        {
            MemberManager mMgr = new MemberManager(ConnectionString);
            User user = mMgr.GetUser(new Guid(ID));
            if (OTP == user.OTP)
            {
                ViewBag.Result = string.Format("{{\"success\": {0}, \"id\": \"{1}\"}}", 1, ID);
                mMgr.UserOTPVerify(new Guid(ID));
            }
            else
                ViewBag.Result = string.Format("{{\"success\": {0}, \"error\": \"{1}\"}}", 0, "Wrong OTP");
            return View("_result");
        }
        public ActionResult UserVerifyTempOTP(string ID, string OTP, string Phone)
        {
            MemberManager mMgr = new MemberManager(ConnectionString);
            User user = mMgr.GetUser(new Guid(ID));
            if (OTP == user.OTP)
            {
                ViewBag.Result = string.Format("{{\"success\": {0}, \"id\": \"{1}\"}}", 1, ID);
                mMgr.UserOTPVerify(new Guid(ID));
                mMgr.SaveUserInfo(ID, Phone, "null", "null", "null");
            }
            else
                ViewBag.Result = string.Format("{{\"success\": {0}, \"error\": \"{1}\"}}", 0, "Wrong OTP");
            return View("_result");
        }
        private void SendSMS(string msg, string phone)
        {
            WebClient client = new WebClient();
            client.OpenRead("http://josmsservice.com/smsonline/msgservicejo.cfm?numbers=" + phone + ",&senderid=Tajje&AccName=Tajje&AccPass=Tajje123&msg=" + msg + "&requesttimeout=5000000");
        }
        protected string GenerateOTP(int length, string Type = "0")
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            if (Type == "1")
            {
                characters += alphabets + small_alphabets + numbers;
            }
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        public ActionResult CategoriesGet(string parentID, string langID, string brandid)
        {
            Language lang = new Language();
            lang.ID = langID;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Category> cats = null;
            if (string.IsNullOrWhiteSpace(parentID))
                cats = pMgr.GetCategories(lang);
            else
                cats = pMgr.GetCategoryCategories(parentID, lang);
            StringBuilder result = new StringBuilder("{\"categories\":[");
            int i = 0;
            foreach (Category item in cats)
            {
                if (i > 0)
                {
                    result.Append(",");
                }
                result.AppendFormat("{{\"id\":\"{0}\",\"name\":\"{1}\",\"image\":\"{2}\",\"haschildren\":\"{3}\", \"parentid\":\"{4}\"}}", item.ID, item.Name, item.ThumbURL, item.HasChildren ? "1" : "0", item.ParentID);
                i++;
            }
            result.Append("]}");
            ViewBag.Result = result.ToString();
            return View("_result");
        }
        public ActionResult BrandCategoriesGet(string langID, string brandid)
        {
            Language lang = new Language();
            lang.ID = langID;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Category> cats = null;


            cats = pMgr.GetBrandCategories(brandid, lang);
            StringBuilder result = new StringBuilder("{\"categories\":[");
            int i = 0;
            foreach (Category item in cats)
            {
                if (i > 0)
                {
                    result.Append(",");
                }
                result.AppendFormat("{{\"id\":\"{0}\",\"name\":\"{1}\",\"image\":\"{2}\",\"haschildren\":\"{3}\", \"parentid\":\"{4}\"}}", item.ID, item.Name, item.ThumbURL, item.HasChildren ? "1" : "0", item.ParentID);
                i++;
            }
            result.Append("]}");
            ViewBag.Result = result.ToString();
            return View("_result");
        }
        public ActionResult BrandsGet(string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Brand> cats = pMgr.GetBrands(lang);
            ViewBag.Result = JsonConvert.SerializeObject(cats);
            return View("_result");
        }
        public JsonResult EmailLogin(string email, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            MemberManager mMgr = new MemberManager(ConnectionString);
            CMS.User user = mMgr.GetUser(email);
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (user == null) //user doesnt exist before
            {
                result.Add("result", "0");
            }
            else
            {
                result.Add("result", 1);
                result.Add("uid", user.ID);
                result.Add("name", user.Name);
                result.Add("phone", user.Phone);
                result.Add("email", user.Email);
                result.Add("gender", user.Gender);
                result.Add("Identity", user.Useridentity);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmailRegister(string email, string phone, string name)
        {
            int dummy;
            Dictionary<string, object> result = new Dictionary<string, object>();
            MemberManager mMgr = new MemberManager(ConnectionString);
            SQLResult sqlresult;
            CMS.User user = mMgr.GetUser(email);
            Language en = new Language();
            string enPhone = "";
            foreach (char item in phone.ToCharArray())
            {
                enPhone += GeneralHelper.CleanArabicNumbers(item.ToString());
            }
            en.ID = "1";
            try
            {

                if (user == null) //user doesnt exist before
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        Guid? uid = null;
                        //mohammad
                        string password = new Guid().ToString().Replace("-", "");
                        //basicly we want a dummy strong password
                        sqlresult = mMgr.RegisterUser(email, password, name, enPhone, null, out uid, null, null);
                        if (uid != null && sqlresult == SQLResult.SUCCESS)
                        {

                            LanguageManager lMgr = new LanguageManager(ConnectionString);
                            foreach (Language thelang in lMgr.GetAllLanguages())
                            {
                                mMgr.SaveUserInfo((Guid)uid, name, thelang);
                            }
                            User user1 = mMgr.GetUser((Guid)uid);
                            //      mMgr.SaveUserInfo((Guid)uid, name, en);
                            ///   mMgr.SaveUserInfo((Guid)uid, name, ar);
                            result.Add("result", 1);
                            result.Add("uid", uid);
                            result.Add("name", name);
                            result.Add("phone", phone);
                            result.Add("email", email);
                            result.Add("Identity", user1.Useridentity);

                            result.Add("gender", "0");
                        }
                    }
                }
                else
                {
                    result.Add("result", "0");
                }
            }
            catch (Exception e)
            {
                result.Add("result", "0");
                result.Add("message", e.ToString());

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CategoryProductsGet(string catid, string brandid, int page, string sch, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            int pagecount;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Product> prods = pMgr.GetProducts(catid, brandid, lang, page, 20, out pagecount, sch, null, null, null, null, null, null, null, false, null, null, null, true);

            string result = JsonConvert.SerializeObject(prods);
            ViewBag.Result = string.Format("{{\"products\":{0},\"pages\":{1}}}", result, pagecount);
            return View("_result");
        }
        public JsonResult ProductGet(string productID, string langID)
        {
            Language lang = new Language();
            Dictionary<string, object> result = new Dictionary<string, object>();
            lang.ID = langID;
            ProductManager pMgr = new ProductManager(ConnectionString);
            Product prod = pMgr.GetProduct(productID, lang);
            result.Add("product", prod);
            if (prod.GroupID.HasValue)
            {
                List<Product> products = pMgr.ProductsGetGrouped((int)prod.GroupID, lang, null);
                result.Add("sizes", products);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SpecsGet(string source, string langID)
        {
            Language lang = new Language();

            lang.ID = langID;
            ProductManager pMgr = new ProductManager(ConnectionString);
            StringBuilder result = new StringBuilder("{\"specs\":[");
            //List<string> specs = pMgr.GetSpecValues(Convert.ToInt32(source), lang);
            LookupManager lMgr = new LookupManager(ConnectionString);
            List<Lookup> specs = lMgr.GetLookUpCategoryItems(Convert.ToInt32(source), lang);
            int i = 0;
            foreach (Lookup spec in specs)
            {
                if (i > 0)
                {
                    result.Append(",");
                }
                result.AppendFormat("{{\"ID\":\"{0}\",\"Name\":\"{1}\",\"Value\":\"{2}\",\"CategoryID\":\"{3}\", \"Rank\":\"{4}\"}}", spec.ID, spec.Name, spec.Value, spec.CategoryID, spec.Rank);
                i++;
            }
            result.Append("]}");
            ViewBag.Result = JsonConvert.SerializeObject(specs);
            return View("_result");
        }
        public ActionResult UserAddressesGet(Guid uid, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            List<Address> adds = cMgr.GetUserAddresses(uid);
            StringBuilder result = new StringBuilder("[");
            int i = 0;
            foreach (Address item in adds)
            {
                if (i > 0)
                {
                    result.Append(",");
                }
                result.AppendFormat("{{\"id\":\"{0}\",\"name\":\"{1}\",\"address1\":\"{2}\",\"address2\":\"{3}\",\"city\":\"{4}\",\"country\":\"{5}\",\"phone1\":\"{6}\",\"phone2\":\"{7}\",\"postal\":\"{8}\"}}", item.ID, GeneralHelper.MakeValidJSON(item.Label), GeneralHelper.MakeValidJSON(item.Address1), GeneralHelper.MakeValidJSON(item.Address2), GeneralHelper.MakeValidJSON(item.City), GeneralHelper.MakeValidJSON(item.Country), !string.IsNullOrEmpty(item.Phone1) ? GeneralHelper.MakeValidJSON(item.Phone1) : null, GeneralHelper.MakeValidJSON(item.Phone2), GeneralHelper.MakeValidJSON(item.Postal));
                i++;
            }
            result.Append("]");
            ViewBag.Result = result.ToString();
            return View("_result");
        }
        public ActionResult RemoveUserAddress(Guid uid, string addressID)
        {
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            //StringBuilder result = new StringBuilder("{");
            if (cMgr.RemoveUserAddress(uid, addressID) == SQLResult.SUCCESS)
            {
                ViewBag.Result = "{\"success\":1}";
            }
            else
            {
                ViewBag.Result = "{\"failed\":-10}";
            }
            return View("_result");
        }
        public ActionResult UpdateUserInfo(Guid uid, string name, string email, string gender, string phone, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            MemberManager mMgr = new MemberManager(ConnectionString);
            User user = mMgr.GetUser(uid);
            StringBuilder result = new StringBuilder();
            int gen;
            //if (mMgr.ValidatePassword(email, password))
            //{
            string enPhone = "";
            foreach (char item in phone.ToCharArray())
            {
                enPhone += GeneralHelper.CleanArabicNumbers(item.ToString());
            }
            gen = Convert.ToInt32(gender);
            user.Name = name;
            user.Email = email;
            user.Phone = enPhone;
            user.Gender = (UserGender)gen;
            int pResult;
            mMgr.SaveUserBasicInfo(user, null, out pResult);
            mMgr.SaveUserInfo(uid, name, lang);
            user = mMgr.GetUser(uid);

            result.AppendFormat("{{\"result\":{0},\"uid\":\"{1}\",\"name\":\"{2}\",\"email\":\"{3}\",\"gender\":\"{4}\",\"phone\":\"{5}\",\"isActive\":\"{6}\",\"Identity\":\"{7}\"}}", pResult, uid, name, email, gender, phone, user.OTPConfirmed, user.Useridentity);
            //}
            //else
            //{
            //result.AppendFormat("{{\"result\":0}}");
            //}
            ViewBag.Result = result.ToString();
            return View("_result");
        }
        public ActionResult UserOrdersGet(Guid uid, string langID, int page)
        {
            Language lang = new Language();
            lang.ID = langID;
            int pagecount, dummy;
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            List<Cart> orders = cMgr.GetUserOrders(uid, page, 20, out pagecount, out dummy, CartStatus.Cancelled, CartStatus.Complete, lang);
            foreach (Cart c in orders)
            {
                List<TransactionPointDiscount> transactions = cMgr.GetTransactionsPointDiscount(c.OwnerID.ToString(), Convert.ToInt32(CMS.TransactionType.Credit), c.ID);
                if (transactions.Count > 0)
                    c.pointDiscount = transactions[0].Amount;
                else
                    c.pointDiscount = 0;
            }
            string result = JsonConvert.SerializeObject(orders);
            ViewBag.Result = string.Format("{{\"orders\":{0},\"pages\":{1}}}", result, pagecount);
            return View("_result");
        }
        public ActionResult UserOrderGet(string uid, string orderID, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            Cart order = cMgr.GetCartOrder<SellableProduct>(orderID);
            List<TransactionPointDiscount> transactions = cMgr.GetTransactionsPointDiscount(uid, Convert.ToInt32(CMS.TransactionType.Credit), order.ID);
            if (transactions.Count > 0 && order.ID == transactions[0].CartID)
                order.pointDiscount = transactions[0].Amount;
            if (order.OwnerID == new Guid(uid))
            {
                string result = JsonConvert.SerializeObject(order);
                ViewBag.Result = string.Format("{{\"order\":{0}}}", result);
                return View("_result");
            }
            return null;
        }
        public ActionResult BarcodeToID(string barcode, string langID)
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            Language lang = new Language();
            lang.ID = langID;
            Product prod = pMgr.ProductGetByBarcode(barcode, lang);
            if (!String.IsNullOrEmpty(prod.ID))
                ViewBag.Result = JsonConvert.SerializeObject(prod);
            else
                ViewBag.Result = string.Format("{{\"success\":0}}");
            return View("_result");
        }
        public ActionResult ProductsOnSaleGet(string catid, string brandid, int page, string sch, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            int pagecount;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Product> prods = pMgr.GetProductsOnSale(catid, brandid, lang, page, 20, out pagecount, sch, true);
            ViewBag.Result = JsonConvert.SerializeObject(prods);
            return View("_result");
        }
        public ActionResult ProductsBestSellerGet(string catid, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Product> prods = pMgr.GetBestSellerCategoryProducts(catid, lang, 20, true);
            ViewBag.Result = JsonConvert.SerializeObject(prods);
            return View("_result");
        }
        public ActionResult FeaturedProductsGet(string catid, string brandid, int page, string sch, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            int pagecount;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Product> prods = pMgr.GetProducts(catid, brandid, lang, page, 20, out pagecount, sch, null, null, null, null, null, null, true, true, true, null, null, null);
            ViewBag.Result = JsonConvert.SerializeObject(prods);
            return View("_result");
        }
        public ActionResult SearchSuggestions(string sch, int count, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            int pagecount;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<KeyValuePair<string, string>> prods = pMgr.GetProductsSuggestions(sch, count, langID);
            ViewBag.Result = JsonConvert.SerializeObject(prods);
            return View("_result");
        }
        public ActionResult ProductAddToWishlist(string uid, string prodid, string langID, string type)
        {
            Language lang = new Language();
            lang.ID = langID;
            int type1 = Convert.ToInt32(type);
            ProductManager pMgr = new ProductManager(ConnectionString);
            pMgr.ProductAddToWishlist(prodid, new Guid(uid), type1);
            ViewBag.Result = "{\"success\":1}";
            return View("_result");
        }
        public ActionResult ProductRemoveFromWishlist(string uid, string prodid, string langID, string type)
        {
            Language lang = new Language();
            lang.ID = langID;
            int type1 = Convert.ToInt32(type);
            ProductManager pMgr = new ProductManager(ConnectionString);
            pMgr.ProductRemoveFromWishlist(prodid, new Guid(uid), type1);
            ViewBag.Result = "{\"success\":1}";
            return View("_result");
        }
        public ActionResult UserWishlistGet(string uid, string langID, string type)
        {
            Language lang = new Language();
            lang.ID = langID;
            int type1 = Convert.ToInt32(type);
            int pagecount;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Product> prods = pMgr.ProductsGetUserWishlist(new Guid(uid), lang, 1, int.MaxValue, out pagecount, type1);
            ViewBag.Result = JsonConvert.SerializeObject(prods);
            return View("_result");
        }

        public JsonResult CitiesGet(string langID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            GeneralManager gMgr = new GeneralManager(ConnectionString);
            LookupManager lMgr = new LookupManager(ConnectionString);
            Language lang = new Language() { ID = langID };
            List<City> cities = gMgr.CitiesGet("7", lang, "");
            result.Add("Cities", cities);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserAddressSave(string uid, string id, string name, string address1, string address2, string country, string city, string phone1, string phone2, string postal, string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            string enPhone1 = "";
            foreach (char item in phone1.ToCharArray())
            {
                enPhone1 += GeneralHelper.CleanArabicNumbers(item.ToString());
            }
            string enPhone2 = "";
            foreach (char item in phone2.ToCharArray())
            {
                enPhone2 += GeneralHelper.CleanArabicNumbers(item.ToString());
            }
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            cMgr.SaveUserAddress(new Guid(uid), id, name, address1, address2, country, city, enPhone1, enPhone2, postal);
            ViewBag.Result = "{\"success\":1}";
            return View("_result");
        }
        public ActionResult ProductReviewAdd(string uid, string prodid, string review, int rating)
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            pMgr.ProductReviewAdd(prodid, new Guid(uid), review, rating);
            ViewBag.Result = "{\"success\":1}";
            return View("_result");
        }
        private class JsonItem
        {
            public string id;
            public int qty;
            public string notes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="langID"></param>
        /// <param name="prodsjson">[{id:1,qty:2,notes:3},{id:1,qty:2,notes:3}]</param>
        /// <returns></returns>
        public ActionResult CartCheckout(string uid, string langID, string prodsjson, string addressID, double total, string coupon, bool usePoint)
        {
            bool StockEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["StockEnabled"]); ;
            int ii = 5;
            Language lang = new Language();
            lang.ID = langID;
            int pagecount;
            double discount = 0;
            Guid CartOwnerID = new Guid(uid);
            var prods = JsonConvert.DeserializeObject<List<JsonItem>>(prodsjson);
            if (!string.IsNullOrEmpty(coupon))
            {
                GiftManager gMgr = new GiftManager(ConnectionString);
                Gift TheCoupon = gMgr.UserGiftGet(CartOwnerID, coupon, true, CurrentLanguage);
                discount = TheCoupon.Amount;
            }
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            ProductManager pMgr = new ProductManager(ConnectionString);
            GeneralManager geMgr = new GeneralManager(ConnectionString);
            Cart CurrentCart = new Cart();

            CurrentCart.ID = Guid.NewGuid();
            foreach (JsonItem item in prods)
            {
                SellableProduct prod = cMgr.GetSellableProduct(item.id, lang);
                CartItem cartitem = new CartItem();
                cartitem.Item = prod;
                cartitem.Quantity = item.qty;
                cartitem.Notes = item.notes;
                CurrentCart.Items.Add(cartitem);
            }
            Gateway gate = cMgr.GetGateway(GatewayType.None);

            CurrentCart.OwnerID = CartOwnerID;
            CurrentCart.Delivery = DeliveryMethod.PayOnDelivery;
            CurrentCart.OrderAddress = cMgr.GetUserAddress(CartOwnerID, addressID);
            CurrentCart.OrderAddress.ID = addressID;
            City userCity = geMgr.CityGet(Convert.ToInt32(CurrentCart.OrderAddress.City), lang);
            CurrentCart.ShippingPrice = Convert.ToDouble(userCity.Value);
            CurrentCart.OrderID = cMgr.SaveCart(CurrentCart.ID, CartOwnerID, (CurrentCart.TotalWOShipping * ((100 - discount) / 100)) + CurrentCart.ShippingPrice, CurrentCart.Delivery, CurrentCart.OrderAddress.ID, GatewayType.None, CurrentCart.Notes + (discount > 0 ? "Coupon: " + coupon + " discount: " + discount : ""), null);
            cMgr.ClearCart(CurrentCart.ID);
            foreach (CartItem item in CurrentCart.Items)
            {
                StringBuilder extrainfo = new StringBuilder();
                foreach (KeyValuePair<string, string> spec in item.Item.CustomSpecs)
                {
                    extrainfo.AppendFormat("{0}:{1},", spec.Key, spec.Value);
                }
                if (StockEnabled && item.Item.Quantity < item.Quantity) //check quantity if more than available, set to max then
                {
                    item.Item.Quantity = Convert.ToInt32(item.Quantity);
                }
                cMgr.SaveCartItem(CurrentCart.ID, item.Item.ProductID, item.Quantity, item.Item.UnitPrice, item.Notes, extrainfo.ToString());
            }
            // CurrentCart.TransactionID = cMgr.AddPendingTransaction(CurrentCart.TransactionID, CartOwnerID, TransactionType.Purchase, CurrentCart.Total, (int)GatewayType.None, "Order #" + CurrentCart.OrderID.ToString() + " Purchase", null, true, CurrentCart.ID);


            cMgr.AddTransaction(null, new Guid(uid), TransactionType.Purchase, CurrentCart.Total * ((100 - discount) / 100), null, (int)GatewayType.None, string.Format("cart #: {0}, Mobile App", CurrentCart.ID), null, false, CurrentCart.ID, StockEnabled, 1);
            double cartTotal = CurrentCart.Total * ((100 - discount) / 100);
            double PointValue = Convert.ToDouble(ConfigurationManager.AppSettings["PointValue"]);

            if (usePoint)
            {
                int UserPoints = cMgr.GetUserPoints(new Guid(uid));
                double PointsValue = cMgr.GetUserPoints(CurrentUser.ID) * PointValue;

                double singlePointsValue = 0;
                if (cartTotal < PointsValue)
                {
                    singlePointsValue = (int)Math.Ceiling(cartTotal);
                }
                else
                {
                    singlePointsValue = PointsValue;
                }
                cMgr.AddTransaction(null, new Guid(uid), TransactionType.Credit, singlePointsValue, null, (int)GatewayType.None, string.Format("cart #: {0}, Mobile App", CurrentCart.ID), null, false, CurrentCart.ID, StockEnabled, 1);

                cMgr.RemovePoints(new Guid(uid), Convert.ToInt32(singlePointsValue / PointValue), string.Format("Used by user from app on cart {0}", CurrentCart.ID), null);
            }
            else
            {
                //We do not add points before completed order
                //cMgr.AddPoints(new Guid(uid), Convert.ToInt32(cartTotal * LoyaltyPoint));
            }
            //CartHelper.NewCart();
            StringBuilder result = new StringBuilder();
            //SendOrderEmail(CurrentCart);
            try
            {
                Helper.SendEmail("site@vikikfashion.com", "order@vikikfashion.com", "order to vikik", "new order");
            }
            catch (Exception e)
            {

            }
            result.AppendFormat("{{\"success\":1,\"OrderID\":\"{0}\"}}", CurrentCart.OrderID);
            //ViewBag.Result = "{\"success\":1,\"OrderID\":\"{0}\"}";
            ViewBag.Result = result.ToString();
            return View("_result");
        }
        [HttpGet]
        public ContentResult MainPage(string langID)
        {
            Language lang = new Language();
            lang.ID = langID;
            int dummy = 0;
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<List<Product>> home = pMgr.TajjeHomePageGet(lang);
            List<Product> FeaturedItems = home[0];
            string category2 = "82404";
            string cat2subcat1 = "82412";
            string cat2subcat2 = "82405";
            string cat2subcat3 = "82426";
            string cat2subcat4 = "82433";
            List<Product> products2 = home[1];
            string cat2result = string.Format("{{\"category\":{0},\"subcategories\":[{1},{2},{3},{4}],\"products\":{5}}}", category2, cat2subcat1, cat2subcat2, cat2subcat3, cat2subcat4, JsonConvert.SerializeObject(products2));
            string category1 = "82142";
            string cat1subcat1 = "82192";
            string cat1subcat2 = "82150";
            string cat1subcat3 = "82162";
            string cat1subcat4 = "82208";
            List<Product> products1 = home[2];
            string cat1result = string.Format("{{\"category\":{0},\"subcategories\":[{1},{2},{3},{4}],\"products\":{5}}}", category1, cat1subcat1, cat1subcat2, cat1subcat3, cat1subcat4, JsonConvert.SerializeObject(products1));
            string category3 = "82517";
            string cat3subcat1 = "82521";
            string cat3subcat2 = "82520";
            string cat3subcat3 = "82519";
            string cat3subcat4 = "82518";
            List<Product> products3 = home[3];
            string cat3result = string.Format("{{\"category\":{0},\"subcategories\":[{1},{2},{3},{4}],\"products\":{5}}}", category3, cat3subcat1, cat3subcat2, cat3subcat3, cat3subcat4, JsonConvert.SerializeObject(products3));
            string category4 = "82480";
            string cat4subcat1 = "82481";
            string cat4subcat2 = "82503";
            string cat4subcat3 = "82494";
            string cat4subcat4 = "82491";
            List<Product> products4 = home[4];
            string cat4result = string.Format("{{\"category\":{0},\"subcategories\":[{1},{2},{3},{4}],\"products\":{5}}}", category4, cat4subcat1, cat4subcat2, cat4subcat3, cat4subcat4, JsonConvert.SerializeObject(products4));
            string category5 = "82383";
            string cat5subcat1 = "82390";
            string cat5subcat2 = "82387";
            string cat5subcat3 = "82384";
            string cat5subcat4 = "82385";
            List<Product> products5 = home[5];
            string cat5result = string.Format("{{\"category\":{0},\"subcategories\":[{1},{2},{3},{4}],\"products\":{5}}}", category5, cat5subcat1, cat5subcat2, cat5subcat3, cat5subcat4, JsonConvert.SerializeObject(products5));
            string category6 = "82395";
            string cat6subcat1 = "82397";
            string cat6subcat2 = "82398";
            string cat6subcat3 = "82396";
            string cat6subcat4 = "82298";
            List<Product> products6 = home[6];
            string cat6result = string.Format("{{\"category\":{0},\"subcategories\":[{1},{2},{3},{4}],\"products\":{5}}}", category6, cat6subcat1, cat6subcat2, cat6subcat3, cat6subcat4, JsonConvert.SerializeObject(products6));
            List<Product> product7 = home[7];


            string result = string.Format("\"featureditems\":{0}", JsonConvert.SerializeObject(FeaturedItems));
            result = string.Format("{0},\"flashSale\":{1}", result, JsonConvert.SerializeObject(product7));
            string fullresult = string.Format("\"hpnew\":[{0},{1},{2},{3},{4},{5}]", cat2result, cat1result, cat3result, cat4result, cat5result, cat6result);
            ArticleManager aMgr = new ArticleManager(ConnectionString);
            List<Article> banners = aMgr.GetArticleTypeArticles("8", lang);
            List<Article> saleBanners = aMgr.GetArticleTypeArticles("20019", lang);
            result = string.Format("{0},\"banners\":{1}", result, JsonConvert.SerializeObject(banners));
            result = string.Format("{0},\"saleBanners\":{1}", result, JsonConvert.SerializeObject(saleBanners));
            result = string.Format("{{{0},{1}}}", result, fullresult);
            return Content(result, "application/json", UTF8Encoding.UTF8);
        }




        public JsonResult NewHomePage(string langID)
        {

            Language lang = new Language() { ID = langID };
            Dictionary<string, object> result = new Dictionary<string, object>();
            ArticleManager aMgr = new ArticleManager(ConnectionString);
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Article> banners = aMgr.GetArticleTypeArticles("8", lang);

            try
            {
                List<Article> categories = aMgr.GetArticleTypeArticles("20021", CurrentLanguage, 1, 5, out int dummy, null, out dummy, 0, null, true, null, true, true, true);

                result.Add("Banner", banners);
                result.Add("categories", categories);
                result.Add("Success", 1);
            }
            catch
            {
                result.Add("Success", 0);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private Language GetLanguageOrDefault(string langID)
        {
            Language lang = new Language() { ID = "1" };
            LanguageManager lmg = new LanguageManager(ConnectionString);

            lang = lmg.GetAllLanguages().Find(x => x.ID == langID);
            return lang;
        }

        public JsonResult AddEntitySession(string userID = null, string targetID = null, string relatedEntity = null)
        {

            MessagingManager mgr = new MessagingManager(ConnectionString);
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {

                mgr.AddEntitySession(new Guid(userID), new Guid(targetID), relatedEntity);
                response.Add("Success", 1);
            }
            catch (Exception ex)
            {
                response.Add("Success", 0);
                response.Add("Message", ex.Message.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCommunication(string userID = null, string targetID = null, string langID = "1")
        {
            MessagingManager mgr = new MessagingManager(ConnectionString);
            MemberManager mmgr = new MemberManager(ConnectionString);
            NewListingManager lgr = new NewListingManager(ConnectionString);
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Language CurrentLanguage = new Language() { ID = langID };
                List<Messaging> communication = mgr.GetCommunication(new Guid(userID), new Guid(targetID), CurrentLanguage).OrderByDescending(x => x.DateAdded).ToList();

                MessagingSession session = mgr.GetUserSession(new Guid(userID), Convert.ToInt32(CurrentLanguage.ID)).Find(x => x.SecondParty.ToString() == targetID);

                if (session != null)
                {
                    response.Add("RelatedEntity", session.RelatedEntity);
                }
                else
                {
                    response.Add("RelatedEntity", null);
                }

                if (communication == null || communication.Count == 0)
                {
                    mgr.AddUserSession(null, new Guid(userID), new Guid(targetID), CurrentLanguage, null);
                }
                var target = mmgr.GetUser(new Guid(targetID), CurrentLanguage);
                List<dynamic> items = new List<dynamic>();
                foreach (var item in communication)
                {
                    dynamic message = new
                    {
                        _id = item.ID,
                        createdAt = item.DateAdded,
                        text = item.Text,
                        extraInfo = item.ExtraInfo,
                        user = new { _id = item.SenderID, avatar = target.FullImagePath },
                    };
                    items.Add(message);
                }

                response.Add("Success", 1);
                response.Add("Messages", items);

            }
            catch (Exception ex)
            {
                response.Add("Success", 0);
                response.Add("Message", ex.Message.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MessagingGetCommunication(string userID, string targetID, int langID = 1)
        {
            Language lang = GetLanguageOrDefault(langID.ToString());
            Dictionary<string, object> result = new Dictionary<string, object>();
            MessagingManager mgr = new MessagingManager(ConnectionString);
            try
            {
                List<Messaging> communication = mgr.GetCommunication(new Guid(userID), new Guid(targetID), lang).OrderByDescending(x => x.DateAdded).ToList();
                List<dynamic> items = new List<dynamic>();
                foreach (var item in communication)
                {
                    dynamic message = new
                    {
                        _id = item.ID,
                        createdAt = item.DateAdded,
                        text = item.Text,
                        user = new { _id = item.SenderID, avatar = "http://lyv.me/wsimages/profile.png" },
                    };
                    items.Add(message);
                }
                result.Add("Success", 1);
                result.Add("Messages", items);
            }
            catch (Exception ex)
            {
                result.Add("Success", 0);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        string firebaseKey = "AAAAZb7om-M:APA91bHLfEVt1mNjUBV6WqoV1kzFpUoH6E_0PPwemsXQCHXsIdB8u9pBXJOABKeAjThmOs5vbm_bFtLSqZR8ayIDnCb4f4e_MJ_4XX7YMQ8LjZTSm8siQxcPGGHw2eErtPB8qH7-slHp";

        public JsonResult MessagingSend(string senderID, string receiverID, string message, string extraInfo, string relatedEntityID, int langID = 1)
        {
            Language lang = GetLanguageOrDefault(langID.ToString());
            Dictionary<string, object> result = new Dictionary<string, object>();
            MessagingManager mgr = new MessagingManager(ConnectionString);
            try
            {
                int messageID = mgr.SendMessage(null, new Guid(senderID), new Guid(receiverID), message, extraInfo, relatedEntityID);
                SendFirebaseNotification(new Guid(receiverID), message.Length > 50 ? message.Substring(0, 49) + "..." : message, new Guid(senderID));
                result.Add("Success", 1);
                result.Add("MessageID", messageID);
            }
            catch (Exception ex)
            {
                result.Add("Success", 0);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotificationList(string UserID, int langID = 1)
        {
            Language lang = GetLanguageOrDefault(langID.ToString());
            Dictionary<string, object> result = new Dictionary<string, object>();
            GiftManager gMgr = new GiftManager(ConnectionString);

            try
            {
                List<Gift> gifts = gMgr.UserGiftsGetNew(langID.ToString(), UserID, true);
                result.Add("Success", 1);
                result.Add("Gifts", gifts);
            }
            catch (Exception ex)
            {
                result.Add("Success", 0);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUser(string userID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            MemberManager mgr = new MemberManager(ConnectionString);

            try
            {
                User user = mgr.GetUser(new Guid(userID));

                result.Add("User", user);

                result.Add("result", 1);
            }
            catch (Exception e)
            {
                result.Add("message", e.ToString());
                result.Add("result", 0);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private void SendFirebaseNotification(Guid receiverID, string body, Guid senderID)
        {
            MemberManager mgr = new MemberManager(ConnectionString);
            string userToken = mgr.GetUser(receiverID).FirebaseToken;
            User sender = mgr.GetUser(senderID);
            var request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["Authorization"] = "key=" + firebaseKey;
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    to = userToken,
                    notification = new
                    {
                        body = body,
                        title = "Vikik | New Message",
                        priority = 10,
                    },
                    data = new
                    {
                        userID = senderID.ToString(),
                        UserName = sender.UserName
                    }
                });
                streamWriter.Write(json);
            }
            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }


        public JsonResult HideCommunication(string firstParty = null, string secondParty = null)
        {
            MessagingManager mgr = new MessagingManager(ConnectionString);
            NewListingManager lgr = new NewListingManager(ConnectionString);
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {

                mgr.HideCommunication(firstParty, secondParty);
                response.Add("Success", 1);
            }
            catch (Exception ex)
            {
                response.Add("Success", 0);
                response.Add("Message", ex.Message.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMessageSessions(string userID = null, string langID = "1")
        {
            MessagingManager mgr = new MessagingManager(ConnectionString);
            NewListingManager lgr = new NewListingManager(ConnectionString);
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Language CurrentLanguage = new Language() { ID = langID };
                var sessions = mgr.GetUserSession(new Guid(userID), Convert.ToInt32(CurrentLanguage.ID)).OrderByDescending(x => x.DateAdded).ToList();
                for (int i = 0; i < sessions.Count; i++)
                {
                    if (sessions[i].SecondParty == new Guid(userID))
                    {
                        string holder = sessions[i].SecondPartyName;
                        sessions[i].SecondParty = sessions[i].FirstParty;
                        sessions[i].SecondPartyName = sessions[i].FirstPartyName;
                        sessions[i].FirstParty = new Guid(userID);
                        sessions[i].FirstPartyName = holder;
                    }
                }
                response.Add("Success", 1);
                response.Add("Sessions", sessions);
            }
            catch (Exception ex)
            {
                response.Add("Success", 0);
                response.Add("Message", ex.Message.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public void SendOrderEmail(Cart cart)
        {
            ProductManager mgr = new ProductManager(ConnectionString);
            StringBuilder items = new StringBuilder("<table border=1 cellpadding=5><tr style=\"background:#f05829;color:#ffffff;\"><th>Brand</th><th>Category</th><th>Item</th><th>Quantity</th><th>Unit Price</th><th>Total Price</th><th>Notes</th></tr>");
            foreach (CartItem product in cart.Items)
            {
                Product prod = mgr.GetProduct(product.Item.ProductID.ToString(), CurrentLanguage);

                items.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>", null, product.Name, product.Quantity, product.UnitPrice.ToString("$ #,##0"), product.TotalPrice.ToString("$ #,##0"), product.Notes));
            }
            items.Append("</table>");
            items.Append(string.Format("<br/><br/><b>Delivery Cost:</b> {0}", "$ " + CMS.Config.CMSConfigurations.GetSection().ShippingCost));
            items.Append(string.Format("<br/><b>Total Price:</b> {0}", cart.Total.ToString("$ #,##0")));

            MemberManager mMgr = new MemberManager(ConnectionString);
            User user = mMgr.GetUser(cart.OwnerID, CurrentLanguage);
            StringBuilder customerInformation = new StringBuilder("");
            customerInformation.AppendFormat("<table><tr><td><b>Order #</b></td><td>{0}</td><td><b>Date</b></td><td>{1}</td><td><b>Delivery Date</b></td><td>{2}</td></tr></table>", cart.OrderID, DateTime.Now.ToString("dd/MM/yyyy HH:mm"), cart.Notes);
            customerInformation.AppendFormat("<table><tr><th colspan=\"2\">Customer Details</th></tr><tr><td><b>Name</b></td><td>{0}</td></tr><tr><td><b>Email</b></td><td>{1}</td></tr><tr><td><b>Mobile</b></td><td>{2}</td></tr><tr><td colspan=2><b>Address</b></td></tr><tr><td colspan=2>{3}</td></tr><tr><td colspan=2><b>City</b></td></tr><tr><td colspan=2>{4}</td></tr></table>", user.Name, cart.Email, cart.OrderAddress.Phone1 + "<br />" + cart.OrderAddress.Phone2, cart.OrderAddress.Address1 + "<br/> Receiver: " + cart.OrderAddress.Address2, cart.OrderAddress.City);

            try
            {
                StringBuilder adminEmail = new StringBuilder("");
                adminEmail.Append(string.Format("You have a new order, to view details please <a href=\"http://www.cakeshopco.com/order/view/{1}\">click here</a><br/><br/>", cart.ID.ToString(), cart.Delivery == DeliveryMethod.PayOnline ? (int)GatewayType.TwoCO : (int)GatewayType.None));
                adminEmail.Append(customerInformation);
                adminEmail.Append(items);
                if (!string.IsNullOrEmpty(cart.Notes))
                {
                    adminEmail.AppendFormat("<br/><br/><b>Notes</b><br/>", cart.Notes);
                }
                Helper.SendEmail("site@cakeshopco.com", "order@cakeshopco.com", (mMgr.SubscriptionCheck(CurrentUser.ID, "1") ? "VIP: " : "") + "Order Confirmation from cakeshopco.com", adminEmail.ToString());

                StringBuilder deliveryitems = new StringBuilder("<table border=1 cellpadding=5><tr style=\"background:#f05829;color:#ffffff;\"><th>Brand</th><th>Category</th><th>Item</th><th>Quantity</th><th>Notes</th></tr>");
                foreach (CartItem product in cart.Items)
                {
                    Product prod = mgr.GetProduct(product.Item.ProductID.ToString(), CurrentLanguage);

                    deliveryitems.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", product.Name, product.Quantity, product.Notes));
                }
                deliveryitems.Append("</table>");

                StringBuilder DeliveryEmail = new StringBuilder("");
                DeliveryEmail.Append(customerInformation);
                DeliveryEmail.Append(deliveryitems);
                if (!string.IsNullOrEmpty(cart.Notes))
                {
                    DeliveryEmail.AppendFormat("<br/><br/><b>Notes</b><br/>", cart.Notes);
                }
                Helper.SendEmail("site@cakeshopco.com", "delivery@cakeshopco.com", "WAYBILL from cakeshopco.com", DeliveryEmail.ToString());



                StringBuilder CustomerEmail = new StringBuilder("");
                CustomerEmail.Append(customerInformation);
                CustomerEmail.Append(items);

                if (!string.IsNullOrEmpty(cart.Notes))
                {
                    CustomerEmail.AppendFormat("<br/><br/><b>Notes</b><br/>", cart.Notes);
                }

                Helper.SendEmail("site@cakeshopco.com", cart.Email, "Invoice from cakeshopco.com", CustomerEmail.ToString());
            }
            finally
            {

            }
        }


        // GET: Services
        public ActionResult CheckCoupon(Guid userID, string coupon)
        {
            Language lang = new Language();
            lang.ID = "1";
            StringBuilder result = new StringBuilder();
            GiftManager gMgr = new GiftManager(ConnectionString);
            Gift TheCoupon = gMgr.UserGiftGet(userID, coupon, false, lang);
            if ((TheCoupon.Type == 3 || TheCoupon.Type == 5) && TheCoupon != null)
            {
                result.AppendFormat("{{\"result\":1,\"discount\":\"{0}\"}}", TheCoupon.Amount);
            }
            else
            {
                result.AppendFormat("{{\"result\":0}}");
            }
            ViewBag.Result = result.ToString();
            return View("_result");
        }
        public ActionResult CouponClaim(Guid userID, string coupon)
        {
            Language lang = new Language();
            lang.ID = "1";
            StringBuilder result = new StringBuilder();
            GiftManager gMgr = new GiftManager(ConnectionString);
            if (gMgr.UserClaimGift(userID, coupon))
            {
                Gift TheCoupon = gMgr.UserGiftGet(userID, coupon, false, lang);
                if ((TheCoupon.Type == 3 || TheCoupon.Type == 5))
                {
                    result.AppendFormat("{{\"result\":1,\"discount\":\"{0}\",\"id\":\"{1}\"}}", TheCoupon.Amount, TheCoupon.ID);

                }
                else
                    result.AppendFormat("{{\"result\":0}}");

            }
            else
            {
                Gift TheCoupon = gMgr.UserGiftGet(userID, coupon, false, lang);
                if (TheCoupon != null && TheCoupon.RemainingUses > 0 && (TheCoupon.Type == 3 || TheCoupon.Type == 5))
                {
                    result.AppendFormat("{{\"result\":1,\"discount\":\"{0}\",\"id\":\"{1}\"}}", TheCoupon.Amount, TheCoupon.ID);
                }
                else
                {
                    result.AppendFormat("{{\"result\":0}}");
                }
            }
            ViewBag.Result = result.ToString();
            return View("_result");
        }




        public JsonResult PromoRedeem(string userID, string couponID)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            CommerceManager mgr = new CommerceManager(ConnectionString);
            GiftManager gMgr = new GiftManager(ConnectionString);

            try
            {

                Gift promo = gMgr.GiftGet(couponID, "1");
                if (promo.Type == 4 || promo.Type == 5)
                {
                    int done = gMgr.UserClaimPromo(userID, couponID);
                    if (done != 10)
                    {
                        result.Add("Success", false);

                    }
                    else
                        // result.Add("UserPoints", UserPoints);
                        result.Add("Success", true);
                }
                else
                    result.Add("Success", false);

            }
            catch (Exception ex)
            {
                result.Add("Success", false);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GiftGet(string ID)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            CommerceManager mgr = new CommerceManager(ConnectionString);
            GiftManager gMgr = new GiftManager(ConnectionString);

            try
            {
                var gift = gMgr.GiftGet(ID, "1");
                gift.RemainingUses = 1;//this one for mobile merchant app validation 
                result.Add("gift", gift);
                result.Add("Success", true);

            }
            catch (Exception ex)
            {
                result.Add("Success", false);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetUserPoints(string userID)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            CommerceManager mgr = new CommerceManager(ConnectionString);
            try
            {
                var UserPoints = mgr.GetUserPoints(new Guid(userID));
                result.Add("UserPoints", UserPoints);
                result.Add("Success", true);

            }
            catch (Exception ex)
            {
                result.Add("Success", false);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveItemFlashSale(long pProdid)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            CommerceManager mgr = new CommerceManager(ConnectionString);
            try
            {
                mgr.RemoveFlashSale(pProdid);
                result.Add("Success", true);

            }
            catch (Exception ex)
            {
                result.Add("Success", false);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetChart()
        {
            //WebClient client = new WebClient();
            XmlDocument doc = new XmlDocument();
            // string xml=client.DownloadString("http://www.zagtrader.com//external/economytoday/chartdataapi4.php?resolution=D1&symbol=" + Request["sym"] + "&marketSymbol=ASE&days=365");
            doc.Load("http://www.zagtrader.com//external/economytoday/chartdataapi4.php?resolution=D1&symbol=" + Request["sym"] + "&marketSymbol=ASE&days=365");
            string closing = doc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["close"].Value;
            string[] closingvals = closing.Split((",").ToCharArray());
            string times = doc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["time"].Value;
            string[] timevals = times.Split((",").ToCharArray());
            //TimeSpan t = DateTime.ParseExact(timevals[0], "yyyy.MM.dd", CultureInfo.CurrentCulture) - new DateTime(1970, 1, 1);
            //string timestamp = t.TotalSeconds.ToString();
            for (int i = 0; i < timevals.Length; i++)
            {
                timevals[i] = DateTimeOffset.ParseExact(timevals[i], "yyyy.MM.dd", CultureInfo.CurrentCulture).ToUnixTimeMilliseconds().ToString();
            }
            ViewBag.Closings = closingvals;
            ViewBag.Times = timevals;

            XmlDocument sumdoc = new XmlDocument();
            sumdoc.Load("http://www.zagtrader.com/external/economytoday/PriceQuoteAPI4.php?marketIndexOnly=0&tstamp=0&outputType=xml&tickerSymbol=" + Request["sym"] + "@ASE");
            ViewBag.bidvol = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["bidVol"].Value;
            ViewBag.offvol = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["askVol"].Value;
            ViewBag.prevclose = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["prevClose"].Value;
            ViewBag.todayopen = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["open"].Value;
            ViewBag.dayrange = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["low"].Value + " - " + sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["high"].Value;
            ViewBag.weeksrange = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["low52"].Value + " - " + sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["high52"].Value;
            ViewBag.bidask = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["bid"].Value + "/" + sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["ask"].Value;
            ViewBag.trades = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["trades"].Value;
            ViewBag.lasttime = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["timeStampFormatted"].Value;
            ViewBag.lastsize = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["contractSize"].Value;
            ViewBag.volume = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["volume"].Value;
            ViewBag.value = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["value"].Value;
            return View("_rumchart");
        }
        public ActionResult GetChart2()
        {
            //WebClient client = new WebClient();
            XmlDocument doc = new XmlDocument();
            // string xml=client.DownloadString("http://www.zagtrader.com//external/economytoday/chartdataapi4.php?resolution=D1&symbol=" + Request["sym"] + "&marketSymbol=ASE&days=365");
            doc.Load("http://www.zagtrader.com//external/economytoday/chartdataapi4.php?resolution=D1&symbol=" + Request["sym"] + "&marketSymbol=ASE&days=365");
            string closing = doc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["close"].Value;
            string[] closingvals = closing.Split((",").ToCharArray());
            string times = doc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["time"].Value;
            string[] timevals = times.Split((",").ToCharArray());
            //TimeSpan t = DateTime.ParseExact(timevals[0], "yyyy.MM.dd", CultureInfo.CurrentCulture) - new DateTime(1970, 1, 1);
            //string timestamp = t.TotalSeconds.ToString();
            for (int i = 0; i < timevals.Length; i++)
            {
                timevals[i] = DateTimeOffset.ParseExact(timevals[i], "yyyy.MM.dd", CultureInfo.CurrentCulture).AddHours(7).ToUnixTimeMilliseconds().ToString(); //added 7 hours to make it jordan time 
            }
            ViewBag.Closings = closingvals;
            ViewBag.Times = timevals;

            XmlDocument sumdoc = new XmlDocument();
            sumdoc.Load("http://www.zagtrader.com/external/economytoday/PriceQuoteAPI4.php?marketIndexOnly=0&tstamp=0&outputType=xml&tickerSymbol=" + Request["sym"] + "@ASE");
            ViewBag.bidvol = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["bidVol"].Value;
            ViewBag.offvol = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["askVol"].Value;
            ViewBag.prevclose = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["prevClose"].Value;
            ViewBag.todayopen = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["open"].Value;
            ViewBag.dayrange = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["low"].Value + " - " + sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["high"].Value;
            ViewBag.weeksrange = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["low52"].Value + " - " + sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["high52"].Value;
            ViewBag.bidask = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["bid"].Value + "/" + sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["ask"].Value;
            ViewBag.trades = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["trades"].Value;
            ViewBag.lasttime = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["timeStampFormatted"].Value;
            ViewBag.lastsize = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["contractSize"].Value;
            ViewBag.volume = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["volume"].Value;
            ViewBag.value = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["value"].Value;
            ViewBag.close = sumdoc.DocumentElement.ChildNodes[0].ChildNodes[0].Attributes["close"].Value;
            return View("rumpage");
        }



        #region Points merchant
        public readonly double pointvalue = Convert.ToDouble(ConfigurationManager.AppSettings["PointValue"]);
        public int ConvertAmountToPoints(double amount)
        {
            return Convert.ToInt32(Math.Ceiling(amount / pointvalue)); //any fraction of point is converted to full point
        }

        public JsonResult RedeemPoints(string userID, string adminID, double amount)
        {
            Dictionary<String, Object> result = new Dictionary<string, object>();
            try
            {
                CommerceManager cMgr = new CommerceManager(ConnectionString);
                MemberManager mMgr = new MemberManager(ConnectionString);
                StringBuilder hashBuilder = new StringBuilder();


                User admin = mMgr.GetUser(new Guid(adminID));
                if (cMgr.RemovePoints(new Guid(userID), ConvertAmountToPoints(amount), string.Format("Through Merchant APP by {0}", admin.Name), new Guid(adminID)))
                    result.Add("Success", 1);
                else
                {
                    result.Add("Success", 0);
                    result.Add("Error", "insufficient points");
                }

            }
            catch (Exception e)
            {
                result.Add("Success", 0);
                result.Add("Error", e.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AddPoints(string userID, string adminID, double amount)
        {
            Dictionary<String, Object> result = new Dictionary<string, object>();
            try
            {
                CommerceManager cMgr = new CommerceManager(ConnectionString);
                MemberManager mMgr = new MemberManager(ConnectionString);
                StringBuilder hashBuilder = new StringBuilder();

                User admin = mMgr.GetUser(new Guid(adminID));
                if (cMgr.AddPoints(new Guid(userID), (int)Math.Floor(amount), string.Format("Through Merchant APP by {0}", admin.Name), new Guid(adminID)))
                    result.Add("Success", 1);
                else
                {
                    result.Add("Success", 0);
                    result.Add("Error", "Invalid insert command");
                }

            }
            catch (Exception e)
            {
                result.Add("Success", 0);
                result.Add("Error", e.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetBalance(string userID, int pLang)
        {
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            MemberManager mMgr = new MemberManager(ConnectionString);
            Language lang = new Language();
            lang.ID = pLang.ToString();
            int points = cMgr.GetUserPoints(new Guid(userID));
            User user = mMgr.GetUser(new Guid(userID), lang);
            user.Limit = points;
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("Success", true);
            map.Add("UserDetails", user);
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoginMerchant(string email, string password, long lang)
        {
            Dictionary<String, Object> result = new Dictionary<string, object>();
            try
            {
                Language language = new Language();
                language.ID = lang.ToString();
                SQLResult dummy;
                MemberManager mMgr = new MemberManager(ConnectionString);
                User user = mMgr.UserLogin(email, password, language, out dummy);
                if (Roles.IsUserInRole(user.UserName, "Merchant"))
                {
                    result.Add("User", user);
                    result.Add("Success", true);
                }
                else
                    result.Add("Success", false);

            }
            catch (Exception e)
            {
                result.Add("Success", false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MessageImageUpload(string messageID, string imagebase64, string filename, string fileextension)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                string ImagesPath = "~/content/messages/" + messageID + "/";

                if (!Directory.Exists(Server.MapPath(ImagesPath).ToLower().Replace("admin", "")))
                    Directory.CreateDirectory(Server.MapPath(ImagesPath).ToLower().Replace("admin", ""));
                string imagesServerPath = Server.MapPath(ImagesPath).ToLower().Replace("admin", "");
                string filePath = imagesServerPath + filename + "." + fileextension;
                Byte[] bytes = Convert.FromBase64String(imagebase64);
                System.IO.File.WriteAllBytes(filePath, bytes);
                if (fileextension.ToLower() != ".jpg")
                {
                    string jpgFilePath = imagesServerPath + filename + ".jpg";
                    System.IO.File.WriteAllBytes(jpgFilePath, bytes);
                }
                SaveImageWithPostNumber(imagesServerPath, filename, filePath, "");
                result.Add("Success", 1);
            }
            catch (Exception ex)
            {
                result.Add("Success", 0);
                result.Add("Message", ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        string SaveImage(string imagesServerPath, string fileName, FileInfo fileThumb, string pPostNumber, Product pProd)
        {

            string fileExtension = fileThumb.Extension;
            string filePath = imagesServerPath + fileName + pPostNumber + fileExtension;

            if (System.IO.File.Exists(imagesServerPath + fileName))
            {
                System.IO.File.Delete(imagesServerPath + fileName);
            }

            fileThumb.CopyTo(filePath);
            SaveImageWithPostNumber(imagesServerPath, fileName, filePath, pPostNumber);
            return fileName;
        }

        void SaveImageWithPostNumber(string imagesServerPath, string fileName, string filePath, string postNumber)
        {
            if (postNumber == null)
                postNumber = string.Empty;
            CMSConfigurations config = CMSConfigurations.GetSection();
            foreach (CMSImageInfo imginfo in config.Images)
            {
                string namerest = (string.IsNullOrEmpty(imginfo.OverrideName) ? imginfo.Name : imginfo.OverrideName);
                ImageHelper.SaveImage(imagesServerPath + fileName + postNumber + "_" + namerest, filePath, "", imginfo.Width, imginfo.Height, imginfo.Anchor, imginfo.ResizeMode, (imginfo.ImageFormat == "png" ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg), imginfo.BaseImage, imginfo.Padding);

            }
            ImageHelper.SaveImage(imagesServerPath + fileName + postNumber + "_thumb" + ".png", filePath, "", 192, 141, ImageHelper.Anchor.Center, ImageHelper.PicResizeMode.Transparent, System.Drawing.Imaging.ImageFormat.Png, 0);
        }
        #endregion

        [HttpGet]
        public JsonResult GetMinAllowed()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("minAllowed", ConfigurationManager.AppSettings["minAllowedVersion"]);
            result.Add("current", ConfigurationManager.AppSettings["currentVersion"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteAccountPage()
        {
            return View();
        }
        public JsonResult DeleteUser(string email, string phone)
            {
            Dictionary<string, object> result = new Dictionary<string, object>();
    
            try
            {
                MemberManager mMgr = new MemberManager(ConnectionString);
                CMS.User user = mMgr.GetUserByEmail(email, CurrentLanguage);
                if (user != null  && user.ID != Guid.Empty) 
                {
                    mMgr.DeleteUser(user.ID);
                    result.Add("Success", 1);
                }
                else
                {
                    result.Add("Success", 0);
                }
            }
            catch (Exception ex)
            {
                result.Add("Success", 0);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RestoreUser(string email)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            MemberManager mgr = new MemberManager(ConnectionString);

            try
            {
                MemberManager mMgr = new MemberManager(ConnectionString);
                User OLduser = mMgr.GetUserByEmail(email, CurrentLanguage);

                DynamicParameters param = new DynamicParameters();
                param.Add("@pUserID", OLduser.ID);
                CMS.Dapper.Database.ExcuteNonQuery("Usr_RestoreAccunt", param);


                result.Add("Success", true);

            }
            catch (Exception ex)
            {
                result.Add("Success", false);
                result.Add("Message", ex.Message.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}