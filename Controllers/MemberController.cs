using CMS;
using CMS.Managers;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using vikik.Models;

namespace vikik.Controllers
{
    public class MemberController : KensoftController
    {


        // GET: Member
        public ActionResult Login(int error = 0, bool fail = false)
        {
            ViewBag.ReturnTo = Request["rt"];

            FillSEO(-3);
            StaticPageManager pMgr = new StaticPageManager(ConnectionString);
            StaticPage page = pMgr.GetStaticPage(65, CurrentLanguage);
            ViewBag.NewCustomers = pMgr.GetStaticPage(4, CurrentLanguage);
            if (CurrentUser != null)
            {
                //if (!string.IsNullOrEmpty(Request["rt"]))
                //    return Redirect(Request["rt"]);
                //else
                //    return RedirectToAction("Details", "Member");
            }

            ViewBag.CurrentUser = CurrentUser;
            return View("login", page);
        }

        public ActionResult Register()
        {

            StaticPageManager pMgr = new StaticPageManager(ConnectionString);
            ViewBag.NewCustomers = pMgr.GetStaticPage(4, CurrentLanguage);
            FillSEO(-6);
            ViewBag.RT = Request["rt"];
            //LoginUser();
            return View();
        }

        public ActionResult Details()
        {
            FillSEO(-4);
            if (CurrentUser == null || string.IsNullOrEmpty(CurrentUser.Name))
                return Redirect("~/member/login?rt" + Request["rt"]);
            if (!string.IsNullOrEmpty(Request["rt"]))
                return Redirect(Url.Content("~/") + Request["rt"].Replace("%3f", "?"));
            ViewBag.User = CurrentUser;
            ViewBag.Result = TempData["Result"];
            MemberManager mgr = new MemberManager(ConnectionString);
            CommerceManager cmgr = new CommerceManager(ConnectionString);
            ViewBag.Address = cmgr.GetUserAddresses(CurrentUser.ID);

            LookupManager lck = new LookupManager(ConnectionString);

            //ViewBag.Countries = lck.GetLookupCategories(CurrentLanguage).FindAll(x => x.ID != 1013).ToList();
            ViewBag.Countries = lck.GetLookupCategories(CurrentLanguage).FindAll(x => x.ID != 1010).ToList();
            ViewBag.DefaultID = cmgr.GetDefaultAddress(CurrentUser.ID);
            var cites = new List<Lookup>();
            foreach (LookupCategory item in ViewBag.Countries)
            {
                var countryCites = lck.GetLookUpCategoryItems(item.ID, CurrentLanguage);
                if (countryCites != null && countryCites.Count > 0)
                    cites.AddRange(countryCites);
            }
            ViewBag.Cites = cites;
            ViewBag.UserPoints = cmgr.GetUserPoints(CurrentUser.ID);
            return View("Profile");
        }

        [IsLoggedIn]
        [HttpPost]
        public ActionResult AddReview()
        {

            string id = Request["prodID"];
            string review = Request["review"];
            int rating = Convert.ToInt32(Request["rate"]);
            bool iBought = Request["iBought"] == "on";
            ProductManager pmgr = new ProductManager(ConnectionString);
            pmgr.ProductReviewAdd(id, CurrentUser.ID, review, rating, iBought);
            return RedirectToAction("ProductDetails", "Catalog", new { id = id });
        }



        [HttpPost]
        //   [ValidateGoogleCaptcha]
        public ActionResult LoginUser()
        {
            string email = Request["txtEmail"];
            string password = Request["txtPassword"];
            string phone = Request["phone"];
            string name = Request["firstName"] + " " + Request["lastName"];
            string dob = Request["DOB"];
            string pattern = @"^\d{10}$";
           
            MemberManager mMgr = new MemberManager(ConnectionString);
            DirectoryManager dmgr = new DirectoryManager(ConnectionString);
            switch (Request["radAccount"])
            {
                case "0": //new user
                    SQLResult result;
                    Guid? userID;

                    DateTime DOB = Convert.ToDateTime(dob);
                    int pResult = 0;
                    Guid? newUser = mMgr.SaveUserBasicInfo(null, 0, email, password, phone, false, UserGender.NotSpecified, null, null, null, null, null, (AcademicDegree)1, null, null, DateTime.Now, null, 1, null, false, null, null, out pResult);

                    result = (SQLResult)pResult;
                    if (result == SQLResult.SUCCESS)
                    {
                        LanguageManager lmg = new LanguageManager(ConnectionString);
                        List<Language> lang = lmg.GetAllLanguages();
                        foreach (var item in lang)
                        {
                            mMgr.SaveUserInfo((Guid)newUser, name, item);
                        }
                        LoginUser(true);
                        string welcomeBody = string.Format(Resources.Strings.EmailWelcomeBody, name, email);
                        Helper.SendEmail("site@vikikfashion.com", email, Resources.Strings.EmailWelcomeSubject, welcomeBody);
                        return RedirectToAction("index", "Home");

                    }
                    else
                    {
                        ViewBag.ErrorMessage = Resources.Strings.UserAlreadyExists;
                        return Login(1, true);
                    }
                    break;
                case "1": //old user
                    LoginUser(false);
                    break;

            }
            return Login(0, true);
        }
        [HttpPost]
        public ActionResult DoAddUser()
        {
            string email = Request["txtEmail"];
            string password = Request["txtPassword"];
            string phone = Request["phone"];
            string name = Request["firstName"] + " " + Request["lastName"];
            string dob = Request["DOB"];
            string phonepattern = @"^\d{10}$";
            string emailpattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            SQLResult result;
            Guid? userID;
            if (!Regex.IsMatch(phone, phonepattern))
            {
                TempData["msg"] = "Please enter a valid 10-digit phone number";
                TempData["toastrType"] = "1";
                return RedirectToAction("Register", "Member");

            }
            if (!Regex.IsMatch(email, emailpattern))
            {
                TempData["msg"] = "Please enter a valid Email";
                TempData["toastrType"] = "1";
                return RedirectToAction("Register", "Member");

            }
            MemberManager mMgr = new MemberManager(ConnectionString);
            DirectoryManager dmgr = new DirectoryManager(ConnectionString);

            DateTime DOB = Convert.ToDateTime(dob);
            int pResult = 0;
            Guid? newUser = mMgr.SaveUserBasicInfo(null, 0, email, password, phone, false, UserGender.NotSpecified, null, null, null, null, null, (AcademicDegree)1, null, null, DateTime.Now, null, 1, null, false, null, null, out pResult);

            result = (SQLResult)pResult;
            if (result == SQLResult.SUCCESS)
            {
   

                LanguageManager lmg = new LanguageManager(ConnectionString);
                List<Language> lang = lmg.GetAllLanguages();
                foreach (var item in lang)
                {
                    mMgr.SaveUserInfo((Guid)newUser, name, item);
                }
                mMgr.SaveUserGroup((Guid)newUser,  new Guid("22222222-2222-2222-2222-222222222222"));

                User user = mMgr.GetUser((Guid)newUser);

 
                Session["User"] = user;
                FormsAuthentication.SetAuthCookie(user.Email, true);
                Response.Cookies["User"]["id"] = user.ID.ToString();
                Response.Cookies["User"].Expires = DateTime.Now.AddDays(70);

                string welcomeBody = string.Format(Resources.Strings.EmailWelcomeBody, name, email);
                Helper.SendEmail("site@vikikfashion.com", email, Resources.Strings.EmailWelcomeSubject, welcomeBody);
                return RedirectToAction("index", "Home");
            }
            else if(result == SQLResult.LOGIN_FAIL)
            {
                CMS.User user = mMgr.GetUserByEmail(email, CurrentLanguage);
                if (user.Isdeleted == true)
                {
                    ViewBag.ErrorMessage = Resources.Strings.UserIsDeleted;
                    TempData["msg"] = Resources.Strings.UserIsDeleted;
                    TempData["toastrType"] = "1";
                }
                else
                {
                ViewBag.ErrorMessage = Resources.Strings.UserAlreadyExists;
                TempData["msg"] = Resources.Strings.UserAlreadyExists;
                TempData["toastrType"] = "1";
                }
 
            }
            else
            {
                TempData["msg"] = Resources.Strings.UserAlreadyExists;
                TempData["toastrType"] = "1";
 
            }
            return RedirectToAction("Register", "Member");

        }

        public ActionResult ActiveAccount()
        {
            ViewBag.Title = Resources.Strings.Client + " | Active Account";

            return View();
        }

        [HttpPost]
        public ActionResult DoActiveUser()
        {
            MemberManager mgr = new MemberManager(ConnectionString);
            string email = Request["email"];
            string code = Request["code"];
            var user = mgr.GetUser(email, CurrentLanguage, false);
            if (mgr.CheckResetCode(email, new Guid(code), out Guid pUserID))
            {
                mgr.DoActivateUser(pUserID, true);
                ViewBag.Message = "User has been activated successfully";
            }
            else
            {
                ViewBag.Message = "Code Invalde";
            }
            ViewBag.Title = Resources.Strings.Client + " | Active Account";

            return View("ActiveAccount");
        }

        public ActionResult RegisterSuccess(bool isMerchent = false)
        {
            ViewBag.IsMerchent = isMerchent;
            ViewBag.Title = Resources.Strings.Client + " " + Resources.Strings.RegisterSuccess;
            return View();
        }

        public ActionResult RemoveAddress(string addressID)
        {
            CommerceManager cmgr = new CommerceManager(ConnectionString);
            cmgr.RemoveUserAddress(CurrentUser.ID, addressID);
            return RedirectToAction("Profile");
        }

        private void LoginUser(bool pUpdate, bool isMerchent = false)
        {
            MemberManager mMgr = new MemberManager(ConnectionString);
            SQLResult result;
            string rt = Request["rt"];
            CMS.User user = mMgr.UserLogin(Request["txtEmail"], Request["txtPassword"], CurrentLanguage, out result);
            if (result == SQLResult.LOGIN_SUCCESS && user.Isdeleted == false)
            {

                
                Session["User"] = user;
                FormsAuthentication.SetAuthCookie(user.Email, true);
                Response.Cookies["User"]["id"] = user.ID.ToString();
                Response.Cookies["User"].Expires = DateTime.Now.AddDays(70);
               
                if (string.IsNullOrEmpty(rt))
                {
                    Response.Redirect("~/");
                }
                else
                {
                    Response.Redirect("~/" + rt);
                }

            }

            else if (user != null && user.Isdeleted == true)
            {
                ViewBag.ErrorMessage = "This User Was Deleted Pls Contact With AdminiStrator";
            }
            else
            {
                ViewBag.ErrorMessage = Resources.Strings.LoginFailed;
            }
        }

        private void LoginUser2()
        {
            MemberManager mMgr = new MemberManager(ConnectionString);
            SQLResult result;
            CMS.User user = mMgr.UserLogin(Request["txtEmail"], Request["txtPassword"], CurrentLanguage, out result);
            if (result == SQLResult.LOGIN_SUCCESS)
            {
                Session["User"] = user;
                FormsAuthentication.SetAuthCookie(user.Email, true);
                Response.Cookies["User"]["id"] = user.ID.ToString();
                Response.Cookies["User"].Expires = DateTime.Now.AddDays(70);
            }
            else
            {

                ViewBag.ErrorMessage = Resources.Strings.LoginFailed;
            }
        }

        void FillSEO(int pageID)
        {
            StaticPageManager pmgr = new StaticPageManager(ConnectionString);
            StaticPage page = pmgr.GetStaticPage(pageID, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
        }

        //ForgotPassword ..1
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            FillSEO(-2);
            return View();
        }

        //ForgotPassword ..2
        [HttpPost]
        public ActionResult RequestReset()
        {
            CMS.Config.CMSConfigurations config = new CMS.Config.CMSConfigurations();
            string email = Request["email"];
            MemberManager memMgr = new MemberManager(ConnectionString);
            string name;
            Guid resetID;
            if (memMgr.SendResetPassword(email, CurrentLanguage, out name, out resetID))
            {
                try
                {
                    string body = string.Format(Resources.Strings.EmailPasswordReset, name, email, resetID.ToString());
                    ViewBag.Result = string.Format("{{\"pass\":{1}, \"message\":\"{0}\"}}", Resources.Strings.ResetPasswordEmailSent, 1);
                    Helper.SendEmail("site@vikikfashion.com", email, Resources.Strings.EmailPasswordResetSubject, body);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ViewBag.Result = string.Format("{{\"pass\":{1}, \"message\":\"{0}\"}}", Resources.Strings.EmailNotFound, 0);
            }

            return View("_result");
        }


        public ActionResult DoResetPassword(string email, string rid)
        {
            MemberManager memberManager = new MemberManager(base.ConnectionString);
            Guid pUserID;
            if (memberManager.CheckResetCode(email, new Guid(rid), out pUserID))
            {
                base.Session["User"] = memberManager.GetUser(pUserID);
                FormsAuthentication.SetAuthCookie(((User)base.Session["User"]).UserName, true);
                return RedirectToAction("ChangePassword");
                //base.Response.Redirect("~/home/ChangePassword");
            }
            else
            {
                return RedirectToAction("login");
            }
            return base.View();
        }

        public ActionResult ChangePassword()
        {
            FillSEO(6);
            return View();
        }


        [HttpPost]
        public ActionResult DoChange()
        {
            string newpassword = Request["newpassword"];
            string passconf = Request["confpass"];
            if (newpassword != passconf)
            {
                ViewBag.Result = Resources.Strings.PasswordMismatch;
            }
            else
            {
                MemberManager mgr = new MemberManager(ConnectionString);
                if (mgr.ResetPassword(CurrentUser.ID, newpassword))
                {
                    ViewBag.Result = Resources.Strings.PasswordChangeSuccess;
                }
                else
                {
                    ViewBag.Result = Resources.Strings.PasswordChangeFail;
                }
            }
            //TempData["Result"] = ViewBag.Result;
            return RedirectToAction("index", "Home");
        }

        [HttpPost]
        public ActionResult UpdateUserInfo()
        {

            MemberManager mMgr = new MemberManager(ConnectionString);
            string UserID = Request["UserID"];
            string FirstName = Request["FirstName"];
            string LastName = Request["LastName"];
            string DOB = Request["DOB"];
            //string Email = Request["Email"];
            string Phone = Request["phone"];

            if (!string.IsNullOrEmpty(UserID) && CurrentUser != null)
            {
                User user = CurrentUser;
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                    user.Name = FirstName + " " + LastName;
                if (!string.IsNullOrEmpty(DOB))
                {
                    DateTime dob = user.DOB;
                    DateTime.TryParse(DOB, out dob);
                    user.DOB = dob;
                }
                //if (!string.IsNullOrEmpty(Email))
                //{
                //    user.Email = Email;
                //}
                if (!string.IsNullOrEmpty(Phone))
                {
                    user.Phone = Phone;
                }
                int result;
                mMgr.SaveUserBasicInfo(new Guid(UserID), user.Email, user.Phone, user.City, user.Country, user.DOB, null, user.IsActive, user.FacebookID, out result);
                if ((SQLResult)result == SQLResult.SUCCESS)
                {
                    //TempData["Result"] = Resources.Strings.SaveSuccess;
                }
            }
            return RedirectToAction("Details");
        }

        public void Logout()
        {
            ViewBag.LoggingOut = true;
            Session["User"] = null;
            ViewBag.CurrentUser = null; ;
            FormsAuthentication.SignOut();
            HttpCookie cookie = new HttpCookie("User");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            Response.Redirect("~/");
        }
    }
}