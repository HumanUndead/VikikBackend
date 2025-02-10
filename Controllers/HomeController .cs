using System;
using CMS;
using CMS.Config;
using CMS.Managers;
using vikik.Models;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using CMS.Helpers;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Security;
using CMS.Controls;
using System.Data.OleDb;
using HtmlAgilityPack;
using System.Data;
using Fizzler.Systems.HtmlAgilityPack;
using Newtonsoft.Json;
using vikik.Models;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CMS.Dapper;
using System.Xml;
 
namespace vikik.Controllers
{
    public class HomeController : KensoftController
    {
        public int pageSize = 40;

        public ActionResult Index()
        {
            //def
            StaticPageManager sMgr = new StaticPageManager(ConnectionString);
            ArticleManager mgr = new ArticleManager(ConnectionString);
            ProductManager pMgr = new ProductManager(ConnectionString);
            GalleryManager gMgr = new GalleryManager(ConnectionString);

            // Title & Description
            StaticPage page = sMgr.GetStaticPage(-1, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;

            //text under slider
            ViewBag.text = sMgr.GetStaticPage(1, CurrentLanguage);

            //home slider
            ViewBag.SliderHome = mgr.GetArticleTypeArticles("10013", CurrentLanguage);
            ViewBag.SliderHomeMobileView = mgr.GetArticleTypeArticles("20015", CurrentLanguage);

            //SHOPPING IN VIKIK
            ViewBag.servicesTitle = mgr.GetArticleType("10014", CurrentLanguage);
            ViewBag.services = mgr.GetArticleTypeArticles("10014", CurrentLanguage);

            //categorys products
            ViewBag.AllShopCategories = pMgr.GetCategoryCategories(null, CurrentLanguage);


            List<Product> ListProducts = new List<Product>();
            var Categories = pMgr.GetCategoryCategories(null, CurrentLanguage);
            foreach (var item in Categories) //optimization point make as tajjie single service to get them all
            {
                 List<Product> prod = pMgr.GetProducts(item.ID, null, CurrentLanguage, 1, 10, out int dummy, null, null, null, null, null, null, null, null, true, null, null, null, true);

                if (prod != null)
                {
                    ListProducts.AddRange(prod);

                }
                    
            }
            ViewBag.ListAllProducts = ListProducts;

            ViewBag.SingleProduct = pMgr.GetProduct("270709", CurrentLanguage);



            // Featured products
            List<Product> featprods = pMgr.GetProducts("", null, CurrentLanguage, 1, int.MaxValue, out int dummy2, null, null, null, null, null, null, null, null, true, true ,null, null ,null );
            ViewBag.FeaturedProducts = featprods;


            //sketch
            ViewBag.sketch = sMgr.GetStaticPage(2, CurrentLanguage);

            //Banner1
            // ViewBag.Banner1 = sMgr.GetStaticPage(10009, CurrentLanguage);
            //ViewBag.Banner2 = sMgr.GetStaticPage(10010, CurrentLanguage);
            ViewBag.Banner1 = mgr.GetArticle("40080", CurrentLanguage);
            ViewBag.Banner2 = mgr.GetArticle("40081", CurrentLanguage);
            //brands
            //ViewBag.Brands = gMgr.GetAlbumPhotos("10", CurrentLanguage);




            //if (!isShow)
            //    return Redirect("~/ComingSoon");
            //ArticleManager aMgr = new ArticleManager(ConnectionString);
            //ViewBag.HomeSlide = aMgr.GetArticleTypeArticles("3", CurrentLanguage);
            //ViewBag.Testimonials = aMgr.GetArticleTypeArticles("6", CurrentLanguage);

            //int pageCount;
            //ProductManager pMgr = new ProductManager(ConnectionString);
            //ViewBag.Categories = pMgr.GetCategoryCategories("null", CurrentLanguage);

            //StaticPageManager mgr = new StaticPageManager(ConnectionString);
            //StaticPage page = mgr.GetStaticPage(-1, CurrentLanguage);
            //ViewBag.Title = page.Title;
            //ViewBag.Description = page.Description;

            //page = mgr.GetStaticPage(-2, CurrentLanguage);
            //ViewBag.PostSlideTitle = page.Name;
            //ViewBag.PostSlideText = page.Text;
            //ViewBag.PostSlideLink = page.SubTitle;


            //page = mgr.GetStaticPage(-3, CurrentLanguage);
            //ViewBag.PostSlide2Title = page.Name;
            //ViewBag.PostSlide2Text = page.Text;
            //ViewBag.PostSlide2Link = page.SubTitle;

            //page = mgr.GetStaticPage(-4, CurrentLanguage);
            //ViewBag.BlogTitle = page.Name;
            //ViewBag.CurrentUser = CurrentUser;

            //ViewBag.Path = "shop/categories";
            //int dummy;
            //DirectoryManager dMgr = new DirectoryManager(ConnectionString);
            //ViewBag.GiftOutlets = dMgr.GetOutlets(CurrentLanguage, 1, 12, out dummy);

            ViewBag.showmessage = 0;
            if (Request.Cookies["message"] == null || (Request.Cookies["message"].Value == "0"))
            {
                //message =  Request["message"];
                ViewBag.showmessage = 1;
                cookies();
            }

            return View();
        }


        public void cookies()
        {
            //Cookie
            HttpCookie message = new HttpCookie("message");
            message.Value = "1";
            message.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(message);
        }
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            //def
            StaticPageManager sMgr = new StaticPageManager(ConnectionString);
            ArticleManager aMgr = new ArticleManager(ConnectionString);
            // Title & Description
            //StaticPage page = sMgr.GetStaticPage(3, CurrentLanguage);
            //ViewBag.Title = page.Title;
            //ViewBag.Description = page.Description;
            ViewBag.About = aMgr.GetArticle("30066", CurrentLanguage);
            FillSEO(3);
            return View();
        }

        public ActionResult TurnErrorPage()
        {
            ViewBag.Error = Resources.Strings.ItemOutOfStock;
            return View("_404");
        }



        void FillSEO(int pageID)
        {
            StaticPageManager pmgr = new StaticPageManager(ConnectionString);
            StaticPage page = pmgr.GetStaticPage(pageID, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
        }

        public ActionResult ContactUs()
        {
            StaticPageManager mgr = new StaticPageManager(ConnectionString);
            StaticPage page = mgr.GetStaticPage(-10, CurrentLanguage);
            FillSEO(-10);
            ViewBag.Page = page;
            ViewBag.Contact = page;
            ViewBag.email = mgr.GetStaticPage(8, CurrentLanguage).Text;
            ViewBag.phone = mgr.GetStaticPage(9, CurrentLanguage).Text;
            ViewBag.fax = mgr.GetStaticPage(10, CurrentLanguage).Text;
            return View();
        }

        public ActionResult Page(string id)
        {
            ArticleManager amgr = new ArticleManager(ConnectionString);
            var page = amgr.GetArticleType(id, CurrentLanguage);
            var pages = amgr.GetArticleTypeArticles(id, CurrentLanguage);
            ViewBag.Title = page.Name;
            ViewBag.Description = page.Description;
            ViewBag.Pages = pages;
            ViewBag.PageID = id;
            return View();
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
        public ActionResult Faq()
        {
            ArticleManager amgr = new ArticleManager(ConnectionString);
            ViewBag.CurrentUser = CurrentUser;
            FillSEO(-11);

            ViewBag.MainFAQ = amgr.GetArticleTypes("30025", CurrentLanguage);
            ViewBag.FAQs = amgr.GetArticleTypeArticles("30025", CurrentLanguage);

            return View();

        }
         
        public ActionResult SearchProducts()
        {
            ProductManager pmgr = new ProductManager(ConnectionString);

            //ViewBag.Products = pmgr.getpro

            return View();
        }

        public ActionResult TermsAndConditions()
        {
            StaticPageManager smgr = new StaticPageManager(ConnectionString);
            StaticPage page = smgr.GetStaticPage(10012, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
            ViewBag.Terms = page;
            return View();
        } 

        public ActionResult PrivacyPolicy()
        {
            StaticPageManager smgr = new StaticPageManager(ConnectionString);
            StaticPage page = smgr.GetStaticPage(10013, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
            ViewBag.Page = page;
            return View();
        }

        public ActionResult Delivery()
        {
            StaticPageManager smgr = new StaticPageManager(ConnectionString);
            StaticPage page = smgr.GetStaticPage(-14, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
            ViewBag.Page = page;
            return View();
        }


        [HttpPost]
        public JsonResult SendContact(string subjectNew, string recipient, string name, string email, string message)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                StringBuilder Email = new StringBuilder();
                Email.Append(@Resources.Strings.Name + ":" + name + "<br/>");
                Email.Append(@Resources.Strings.Email + ":" + email + "<br/>");
                Email.Append(@Resources.Strings.Message + ":" + message + "<br/>");

                MailMessage mail = new MailMessage();
                SmtpClient server = new SmtpClient();
                mail.To.Add(recipient);
                mail.Subject = subjectNew;
                mail.IsBodyHtml = true;
                mail.Body = Email.ToString();
                server.Send(mail);

                response.Add("Success", true);
                response.Add("Message", "<h3>" + "ThankYou" + "</h3><p>" + "SuccessTrueMsg" + "</p>");


                return Json(response);
            }
            catch (Exception ex)
            {
                response.Add("Success", false);
                response.Add("Message", "<h3>" + "Error" + "</h3><br/>" + "<p>" + ex.Message.ToString() + "</p>");
                return Json(response);
            }

        }

        [HttpGet]
        public ActionResult Search(string sch, string sort, int page = 1)
        {
            FillSEO(11);
            StaticPageManager mgr = new StaticPageManager(ConnectionString);
            ProductManager pmgr = new ProductManager(ConnectionString);
            int pageSize = 20;
            int totalPages = 0;


            string sortby = string.Empty;
            bool asc = false;
            ViewBag.Sort = sort;
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sort;
                ViewBag.Sort = Request["s"];
            }
            string cat = Request["cat"];
            //ViewBag.Products = mgr.GetProducts(string.IsNullOrEmpty(cat) ? null: cat, null, CurrentLanguage, page, 20, out pCount, sch, true, sortby, asc);
            ViewBag.Sch = sch;
            ViewBag.CurrentPage = page;




            if (!String.IsNullOrEmpty(Request["inpsearch"]))
            {
                sch = Request["inpsearch"];
            }
            //GetProducts(id, null, CurrentLanguage, page, pageSize, out pCount, null, false, sortby, asc, true, null, min, max);
            //List<Product> Products = pmgr.GetProducts(null, null, CurrentLanguage, page, pageSize, out totalPages, sch, null, sortby, asc, null, null, null, null, false, false);
            List<Product> Products = pmgr.GetProducts(null, null, CurrentLanguage, page, pageSize, out totalPages, sch, false, sortby, asc, true, null, null, null);
            ViewBag.Products = Products;
            return View();
        }
        //[HttpPost]
        //public ActionResult SendContact(string subjectNew, string recipient)
        //{
        //    try
        //    {
        //        StringBuilder Email = new StringBuilder();
        //        Email.Append("Name: " + Request.Form["name"] + "<br/>");
        //        Email.Append("Email: " + Request.Form["email"] + "<br/>");
        //        Email.Append("Phone: " + Request.Form["phone"] + "<br/>");
        //        Email.Append("Message:<br/> " + Request.Form["message"] + "<br/>");

        //        //MailMessage mail = new MailMessage();
        //        //SmtpClient smtpServer = new SmtpClient();
        //        //mail.To.Add(CMSConfigurations.GetSection().ContactEmail);
        //        //mail.Subject = "Website Inquiry";
        //        //mail.ReplyToList.Add(Request.Form["Email"]);
        //        //mail.IsBodyHtml = true;
        //        //mail.Body = message.ToString();

        //        MailMessage mail = new MailMessage();
        //        SmtpClient server = new SmtpClient();
        //        mail.To.Add(recipient);
        //        mail.Subject = subjectNew;
        //        mail.IsBodyHtml = true;
        //        mail.Body = Email.ToString();
        //        server.Send(mail);

        //        StringBuilder response = new StringBuilder();
        //        response.Append("{\"valid\":\"true\",\"msg\":\"message sent successfully\"}");
        //        ViewBag.msg = response.ToString();
        //        Response.ContentType = "application/json";
        //    }
        //    catch (Exception x)
        //    {
        //        StringBuilder response = new StringBuilder();
        //        response.Append("{valid:\"false\",msg:\"" + x.Message + "\"}");
        //        ViewBag.msg = response.ToString();
        //        Response.ContentType = "application/json";
        //    }
        //    return View();
        //}

        [HttpPost]
        //     [ValidateGoogleCaptcha]
        public JsonResult SendIssue()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                StringBuilder message = new StringBuilder();
                message.Append("FirstName: " + Request.Form["firstName"] + "<br/>");
                message.Append("LastName: " + Request.Form["lastName"] + "<br/>");

                message.Append("Email: " + Request.Form["email"] + "<br/>");
                message.Append("Issue Type:<br/> " + Request.Form["IssueType"] + "<br/>");
                message.Append("Message:<br/> " + Request.Form["message"] + "<br/>");

                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient();
                mail.To.Add(CMSConfigurations.GetSection().ContactEmail);
                mail.Subject = "Website Inquiry";
                mail.ReplyToList.Add(Request.Form["Email"]);
                mail.IsBodyHtml = true;
                mail.Body = message.ToString();
                smtpServer.Send(mail);

                result.Add("Success", 1);
                result.Add("Message", Resources.Strings.Successsms);

            }
            catch (Exception ex)
            {
                result.Add("Success", 0);
                result.Add("Message", ex.ToString());
                result.Add("Exception", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentComplete()
        {
            base.Response.Expires = 0;
            if (string.IsNullOrEmpty(base.Request.QueryString["vpc_SecureHash"]))
            {
                return base.View();
            }
            string[,] array = null;
            array = new string[base.Request.QueryString.Count, 2];
            int num = 1;
            foreach (string text in base.Request.QueryString.Keys)
            {
                if (!string.IsNullOrEmpty(base.Request.QueryString[text]) && text != "vpc_SecureHash")
                {
                    array[num, 0] = text;
                    array[num, 1] = base.Request.QueryString[text];
                    num++;
                }
            }
            if (!(base.Request.QueryString["vpc_SecureHash"].ToUpper() == this.doSecureHash(array, "6374BE15C4C7751DC435966390B50768")))
            {
                return base.View("_404");
            }
            string arg_11D_0 = base.Request.QueryString["title"];
            string arg_133_0 = base.Request.QueryString["AgainLink"];
            string arg_149_0 = base.Request.QueryString["vpc_Locale"];
            string arg_15F_0 = base.Request.QueryString["vpc_BatchNo"];
            string arg_175_0 = base.Request.QueryString["vpc_Command"];
            string arg_18B_0 = base.Request.QueryString["vpc_Version"];
            string arg_1A1_0 = base.Request.QueryString["vpc_OrderInfo"];
            string arg_1B7_0 = base.Request.QueryString["vpc_Merchant"];
            string arg_1CD_0 = base.Request.QueryString["vpc_AuthorizeId"];
            string arg_1E3_0 = base.Request.QueryString["vpc_AcqResponseCode"];
            string text2 = base.Request.QueryString["vpc_TxnResponseCode"];
            CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
            Guid pID = new Guid(base.Request["vpc_MerchTxnRef"]);
            double num2 = Convert.ToDouble(base.Request["vpc_Amount"]) / 1000.0;
            string text3 = base.Request["vpc_ReceiptNo"];
            string arg = base.Request["vpc_Card"];
            string arg_273_0 = base.Request["ip"];
            string arg2 = base.Request["vpc_TransactionNo"];

            ViewBag.PaymentReference = text3;
            if (!(text2 == "0"))
            {
                ViewBag.FailedCode = text2;
                ViewBag.PaymentFailedText = Resources.Strings.ResourceManager.GetString("PayErr" + text2);
                return base.View("PaymentFailed");
            }
            CMS.Transaction pendingTransaction = commerceManager.GetPendingTransaction(pID);
            if (pendingTransaction != null && Math.Round(pendingTransaction.Amount) == Math.Round(num2))
            {
                ViewBag.Amount = num2;
                ViewBag.TransactionReference = text3;
                commerceManager.CompleteTransaction(pendingTransaction.ID, false);
                commerceManager.AddTransaction(new Guid?(pendingTransaction.ID), pendingTransaction.UserID, TransactionType.Payment, num2, text3, new int?(7), string.Format("Card #: {0}, Acceptance: {1}", arg, arg2), null, true, null, false, 1);
                return base.View("Accept", pendingTransaction);
            }
            return base.View("_404");
        }



        public string doSecureHash(string[,] MyArray, string SECURE_SECRET)
        {
            this.SortDoubleDimension<string>(MyArray, false);
            string text = SECURE_SECRET;
            int num = 0;
            for (int i = 0; i <= MyArray.GetUpperBound(0); i++)
            {
                if (MyArray[i, 1] != null && MyArray[i, 1].Length > 0)
                {
                    text += MyArray[i, 1];
                    num++;
                }
            }
            num++;
            return HomeController.GetMD5Hash(text);
        }

        public static string GetMD5Hash(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        private void SortDoubleDimension<T>(T[,] array, bool bySecond = false)
        {
            int length = array.GetLength(0);
            T[] array2 = new T[length];
            T[] array3 = new T[length];
            for (int i = 0; i < length; i++)
            {
                array2[i] = array[i, 0];
                array3[i] = array[i, 1];
            }
            if (typeof(T) == typeof(string))
            {
                if (bySecond)
                {
                    Array.Sort(array3, array2, StringComparer.Ordinal);
                }
                else
                {
                    Array.Sort(array2, array3, StringComparer.Ordinal);
                }
            }
            else if (bySecond)
            {
                Array.Sort<T, T>(array3, array2);
            }
            else
            {
                Array.Sort<T, T>(array2, array3);
            }
            for (int j = 0; j < length; j++)
            {
                array[j, 0] = array2[j];
                array[j, 1] = array3[j];
            }
        }


        [HttpPost]
        public JsonResult SubscribeNewsletter(String Email)
        {
            NewsLetterManager nmgr = new NewsLetterManager(ConnectionString);
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (!string.IsNullOrEmpty(Email))

                {
                    nmgr.AddUser(Email, "077", "077", null, null, null, null);
                    result.Add("Status", "Success");
                    result.Add("Message", @Resources.Strings.Thankyouforsubscribingtoournewsletter);
                }
                else
                {
                    result.Add("Status", "Success");
                    result.Add("Message", @Resources.Strings.PleaseEnterAVaildEmail);
                }
            }
            catch (Exception ex)
            {
                result.Add("Status", "Fail");
                result.Add("Message", @Resources.Strings.AnerroroccuredpleaseCheckyourconsole);
                result.Add("Error", ex.ToString());
            }

            return Json(result);
        }

        public ActionResult ProdImporter()
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            pMgr.ClearProducts("KhaledConfirmed");
            string imagesServerPath = Server.MapPath("~/content/products/");
            DirectoryInfo allproductsdir = new DirectoryInfo(imagesServerPath);
            //allproductsdir.Delete(true);
            allproductsdir.Create();
            StringBuilder prod = new StringBuilder("");
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/content/vikik"));
            foreach (DirectoryInfo catdir in directory.EnumerateDirectories())
            {
                string catname = catdir.Name;
                string catid = pMgr.CategorySaveByName(catname, null);
                string catfilename = string.Format("{0}", catid);
                string filePath = catdir.FullName + "image.png";
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/content/categories/"));
                if (System.IO.File.Exists(filePath))
                {
                    FileInfo imgfile = new FileInfo(filePath);
                    if (!System.IO.File.Exists(Server.MapPath("~/content/categories/") + catfilename + ".png"))
                    {
                        imgfile.CopyTo(Server.MapPath("~/content/categories/") + catfilename + ".png");
                        SaveImageWithPostNumber(Server.MapPath("~/content/categories/"), catfilename, filePath, "");
                    }
                }
                pMgr.SaveCategoryBasicInfo(catid, catfilename, null, 10, true);
                DirectoryInfo productsdir = new DirectoryInfo(Server.MapPath(String.Format("~/content/vikik/{0}", catname)));
                foreach (DirectoryInfo productdir in productsdir.EnumerateDirectories())
                {
                    string productname = productdir.Name;
                    string prodid = pMgr.ProductSaveByName(productname, catid, new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"));
                    string proddesc = string.Empty;
                    List<string> vs = new List<string>();
                    string filename = string.Format("{0}/{0}", prodid);
                    string barcode = string.Empty;
                    double price = 0;
                    double oldprice = 0;
                    string tags = string.Empty;
                    int n = 0;
                    string Images = "";
                    int lang = 1;
                    string arDesc = "", enDesc = "";
                    string enName = "", arName = "";
                    string colorsstr = "", sizestr = "", qtystr = "";
                    foreach (FileInfo file in productdir.EnumerateFiles())
                    {
                        proddesc = "";
                        if (file.Extension.ToLower() == ".txt" && file.Name.Substring(0, 1) != ".")
                        {
                            lang = file.Name.ToLower().Contains("ar") ? 2 : 1;
                            string prodinfo = file.OpenText().ReadToEnd();
                            string[] prodinfoparts = prodinfo.Split(("\n").ToCharArray());
                            if (prodinfoparts.Length >= 4)
                            {
                                productname = prodinfoparts[0].Replace("\n", "");
                                if (lang == 1)
                                    enName = productname.Replace("\n", "").Replace("\r", "");
                                else
                                    arName = productname.Replace("\n", "").Replace("\r", "");
                                barcode = prodinfoparts[1].Replace("\n", "").Replace("\r", "");
                                try
                                {
                                    price = Convert.ToDouble(prodinfoparts[5]);
                                }
                                catch (Exception e)
                                {
                                    price = 0;
                                }
                                try
                                {
                                    oldprice = Convert.ToDouble(prodinfoparts[6]);
                                }
                                catch (Exception e)
                                {
                                    oldprice = 0;
                                }
                                colorsstr = prodinfoparts[2].Replace("\n", "").Replace("\r", "");
                                sizestr = prodinfoparts[3].Replace("\n", "").Replace("\r", "");
                                qtystr = prodinfoparts[4].Replace("\n", "").Replace("\r", "");

                                /* for (int i = 7; i < prodinfoparts.Length; i++)
                                 {
                                     proddesc += prodinfoparts[i];
                                 }
                                 proddesc = proddesc.Replace("\n", "<br/>");
                                 proddesc = proddesc.Replace(" \r", "");*/
                                enDesc = proddesc;
                            }
                            else if (prodinfoparts.Length >= 4 && prodinfoparts.Length < 8)
                            {
                                productname = prodinfoparts[0].Replace("\n", "");
                                if (lang == 1)
                                    enName = productname;
                                else
                                    arName = productname;
                                tags = prodinfoparts[2].Replace("\n", "");
                                for (int i = 4; i < prodinfoparts.Length; i++)
                                {
                                    proddesc += prodinfoparts[i];
                                }
                                proddesc = proddesc.Replace("\n", "<br/>");
                                if (lang == 1)
                                    enDesc = proddesc;
                                else
                                    arDesc = proddesc;
                            }
                            else
                            {
                                if (file.Name.ToLower().Contains("ar"))
                                {
                                    arName = prodinfoparts[0].Replace("\n", "").Replace("\r", "");
                                }
                            }
                            //else
                            // {
                            //proddesc = prodinfo;
                            // }
                        }
                        //else if (((file.Extension.ToLower() == ".jpg" && file.Name.Substring(0, 1) != ".") || (file.Extension.ToLower() == ".png" && file.Name.Substring(0, 1) != ".")) && !file.Name.Contains("_") && file.Name.Split('.')[0] == "0")
                        //{
                        //    if (!Directory.Exists(imagesServerPath + prodid))
                        //        Directory.CreateDirectory(imagesServerPath + prodid);
                        //    string fileExtension = file.Extension.ToLower();
                        //    string postNo = n > 0 ? n.ToString() : "";
                        //    string prodfilePath = imagesServerPath + filename + postNo + fileExtension;
                        //    file.CopyTo(prodfilePath);
                        //    SaveImageWithPostNumber(imagesServerPath, filename, prodfilePath, postNo);
                        //    if (!string.IsNullOrEmpty(Images))
                        //    {
                        //        vs.Add(postNo);
                        //        Images += "," + postNo;
                        //    }
                        //    else
                        //    {
                        //        vs.Add(postNo);
                        //        Images = postNo;
                        //    }
                        //    System.IO.File.Delete(filePath);
                        //    n++;
                        //}
                    }
                    string NewProdID = pMgr.SaveProduct(new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"), prodid, catid, productname, lang == 1 ? enDesc : arDesc, price, (oldprice == price ? null : (double?)oldprice), filename, Images, true, barcode, tags);
                    pMgr.SaveProductInfo(NewProdID, lang == 1 ? arName : enName, lang == 1 ? arDesc : enDesc, lang == 1 ? "2" : "1");
                    CommerceManager comMgr = new CommerceManager(ConnectionString);
                    GeneralManager gMgr = new GeneralManager(ConnectionString);
                    comMgr.SaveSellableProduct(NewProdID, price, false, 0, 0);
                    int groupid = gMgr.ItemGroupSave(null, enName);
                    string[] colors = colorsstr.Split(",".ToCharArray());
                    string[] sizes = sizestr.Split(",".ToCharArray());
                    string[] qtys = qtystr.Split(",".ToCharArray());
                    List<ItemColor> itemcolors = gMgr.ItemColorsGet();
                    List<ItemSize> itemsizes = gMgr.ItemSizesGet();
                    int index = 0;
                    Product theprod = pMgr.GetProduct(NewProdID, CurrentLanguage);
                    int colorNo = 0;
                    List<int> colorsArray = new List<int>();
                    foreach (string itemcolor in colors)
                    {
                        string thecolor = itemcolor.Trim();
                        ItemColor foundcolor = itemcolors.Find(x => x.Label.ToLower() == thecolor.ToLower() || x.Label.Replace(" ", "").ToLower() == thecolor.ToLower());
                        int colorid = (foundcolor != null ? foundcolor.ID.Value : 12);
                        foreach (string itemsize in sizes)
                        {
                            string thesize = itemsize.Trim();
                            ItemSize foundsize = itemsizes.Find(x => x.Label.ToLower() == thesize.ToLower() || x.Label.Replace(" ", "").ToLower() == thesize.ToLower());
                            int sizeid = (foundsize != null ? foundsize.ID.Value : 1);
                            //int qty = Convert.ToInt32(qtys[0]);
                            theprod.Size = new ItemSize();
                            theprod.Size.ID = sizeid;
                            theprod.Color = new ItemColor();
                            theprod.Color.ID = colorid;
                            theprod.GroupID = groupid;
                            theprod.Quantity = 5;
                            if (index > 0)
                            {
                                theprod.ID = null; //make new product
                            }
                            string theprodid = pMgr.SaveProductBasicInfo(theprod, true, true);

                            string theColorFilename = string.Format("{0}/{0}", theprodid);
                            if (index > 0)
                            {
                                theprod.ID = theprodid;
                                theprod.Searchable = false;
                                theprod.ThumbURL = theColorFilename;
                                if (colorNo > 0)
                                {
                                }
                            }
                            FileInfo newFile = productdir.EnumerateFiles().ToList().Find(x => (x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg") && x.Name.Split('.')[0] == colorNo.ToString());
                            if (newFile != null)
                            {
                                if (!Directory.Exists(imagesServerPath + theprodid))
                                    Directory.CreateDirectory(imagesServerPath + theprodid);
                                string fileExtension = newFile.Extension.ToLower();
                                string postNo = "";
                                string prodfilePath = imagesServerPath + theColorFilename + postNo + fileExtension;
                                newFile.CopyTo(prodfilePath, true);
                                SaveImageWithPostNumber(imagesServerPath, theColorFilename, prodfilePath, postNo);
                            }
                            theprodid = pMgr.SaveProductBasicInfo(theprod, true, true);
                            pMgr.ProductGroupSave(theprodid, groupid.ToString());
                            pMgr.SaveProductInfo(theprodid, enName, enDesc, "1");
                            pMgr.SaveProductInfo(theprodid, arName, arDesc, "2");
                            comMgr.SaveSellableProduct(theprodid, price, false, 0, 0);
                            index++;
                        }
                        colorNo++;
                        colorsArray.Add(colorid);
                    }
                    // pMgr.SaveProductBrand(prodid, brandid);
                }
            }
            return View();
        }

        public ActionResult ProdImporter2()
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            //pMgr.ClearProducts("KhaledConfirmed");
            string imagesServerPath = Server.MapPath("~/content/products/");
            DirectoryInfo allproductsdir = new DirectoryInfo(imagesServerPath);
            //allproductsdir.Delete(true);
            allproductsdir.Create();
            StringBuilder prod = new StringBuilder("");
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/content/vikik"));
            foreach (DirectoryInfo catdir in directory.EnumerateDirectories())
            {
                string catname = catdir.Name;
                string catid = pMgr.CategorySaveByName(catname, null);
                string catfilename = string.Format("{0}", catid);
                string filePath = catdir.FullName + "image.png";
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/content/categories/"));
                if (System.IO.File.Exists(filePath))
                {
                    FileInfo imgfile = new FileInfo(filePath);
                    if (!System.IO.File.Exists(Server.MapPath("~/content/categories/") + catfilename + ".png"))
                    {
                        imgfile.CopyTo(Server.MapPath("~/content/categories/") + catfilename + ".png");
                        SaveImageWithPostNumber(Server.MapPath("~/content/categories/"), catfilename, filePath, "");
                    }
                }
                Category oldcat = pMgr.GetCategory(catid, CurrentLanguage);
                pMgr.SaveCategoryBasicInfo(catid, oldcat.ThumbURL, null, oldcat.Rank, true);
                if (oldcat.HasChildren)
                {
                    DirectoryInfo subcatsdir = new DirectoryInfo(Server.MapPath(String.Format("~/content/vikik/{0}", catname)));
                    foreach (DirectoryInfo subcatdir in subcatsdir.EnumerateDirectories())
                    {
                        string subcatname = subcatdir.Name;
                        string subcatid = pMgr.CategorySaveByName(subcatname, catid);
                        string subcatfilename = string.Format("{0}", subcatid);
                        string subfilePath = subcatdir.FullName + "image.png";
                        if (System.IO.File.Exists(subfilePath))
                        {
                            FileInfo subimgfile = new FileInfo(subfilePath);
                            if (!System.IO.File.Exists(Server.MapPath("~/content/categories/") + subcatfilename + ".png"))
                            {
                                subimgfile.CopyTo(Server.MapPath("~/content/categories/") + subcatfilename + ".png");
                                SaveImageWithPostNumber(Server.MapPath("~/content/categories/"), subcatfilename, subfilePath, "");
                            }
                        }
                        Category oldsubcat = pMgr.GetCategory(subcatid, CurrentLanguage);
                        pMgr.SaveCategoryBasicInfo(subcatid, oldsubcat.ThumbURL, catid, oldsubcat.Rank, true);
                        DirectoryInfo productsdir = new DirectoryInfo(Server.MapPath(String.Format("~/content/vikik/{0}/{1}", catname, subcatname)));
                        foreach (DirectoryInfo productdir in productsdir.EnumerateDirectories())
                        {
                            string productname = productdir.Name;
                            string prodid = pMgr.ProductSaveByName(productname, subcatid, new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"));
                            string proddesc = string.Empty;
                            List<string> vs = new List<string>();
                            string filename = string.Format("{0}/{0}", prodid);
                            string barcode = string.Empty;
                            double price = 0;
                            double oldprice = 0;
                            string tags = string.Empty;
                            int n = 0;
                            string Images = "";
                            int lang = 1;
                            string arDesc = "", enDesc = "";
                            string enName = "", arName = "";
                            string colorsstr = "", sizestr = "", qtystr = "";
                            foreach (FileInfo file in productdir.EnumerateFiles())
                            {
                                proddesc = "";
                                if (file.Extension.ToLower() == ".txt" && file.Name.Substring(0, 1) != ".")
                                {
                                    lang = file.Name.ToLower().Contains("ar") ? 2 : 1;
                                    string prodinfo = file.OpenText().ReadToEnd();
                                    string[] prodinfoparts = prodinfo.Split(("\n").ToCharArray());
                                    if (prodinfoparts.Length >= 4)
                                    {
                                        productname = prodinfoparts[0].Replace("\n", "");
                                        if (lang == 1)
                                            enName = productname.Replace("\n", "").Replace("\r", "");
                                        else
                                            arName = productname.Replace("\n", "").Replace("\r", "");
                                        barcode = prodinfoparts[1].Replace("\n", "").Replace("\r", "");
                                        try
                                        {
                                            price = Convert.ToDouble(prodinfoparts[5]);
                                        }
                                        catch (Exception e)
                                        {
                                            price = 0;
                                        }
                                        try
                                        {
                                            oldprice = Convert.ToDouble(prodinfoparts[6]);
                                        }
                                        catch (Exception e)
                                        {
                                            oldprice = 0;
                                        }
                                        colorsstr = prodinfoparts[2].Replace("\n", "").Replace("\r", "");
                                        sizestr = prodinfoparts[3].Replace("\n", "").Replace("\r", "");
                                        qtystr = prodinfoparts[4].Replace("\n", "").Replace("\r", "");
                                        enDesc = proddesc;
                                    }
                                    else if (prodinfoparts.Length >= 4 && prodinfoparts.Length < 8)
                                    {
                                        productname = prodinfoparts[0].Replace("\n", "");
                                        if (lang == 1)
                                            enName = productname;
                                        else
                                            arName = productname;
                                        tags = prodinfoparts[2].Replace("\n", "");
                                        for (int i = 4; i < prodinfoparts.Length; i++)
                                        {
                                            proddesc += prodinfoparts[i];
                                        }
                                        proddesc = proddesc.Replace("\n", "<br/>");
                                        if (lang == 1)
                                            enDesc = proddesc;
                                        else
                                            arDesc = proddesc;
                                    }
                                    else
                                    {
                                        if (file.Name.ToLower().Contains("ar"))
                                        {
                                            arName = prodinfoparts[0].Replace("\n", "").Replace("\r", "");
                                        }
                                    }
                                }
                            }
                            string NewProdID = pMgr.SaveProduct(new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"), prodid, subcatid, productname, lang == 1 ? enDesc : arDesc, price, (oldprice == price ? null : (double?)oldprice), filename, Images, true, barcode, tags);
                            pMgr.SaveProductInfo(NewProdID, lang == 1 ? arName : enName, lang == 1 ? arDesc : enDesc, lang == 1 ? "2" : "1");
                            CommerceManager comMgr = new CommerceManager(ConnectionString);
                            GeneralManager gMgr = new GeneralManager(ConnectionString);
                            comMgr.SaveSellableProduct(NewProdID, price, false, 0, 0);
                            int groupid = gMgr.ItemGroupSave(null, enName);
                            string[] colors = colorsstr.Split(",".ToCharArray());
                            string[] sizes = sizestr.Split(",".ToCharArray());
                            string qtys = qtystr;
                            List<ItemColor> itemcolors = gMgr.ItemColorsGet();
                            List<ItemSize> itemsizes = gMgr.ItemSizesGet();
                            int index = 0;
                            Product theprod = pMgr.GetProduct(NewProdID, CurrentLanguage);
                            int colorNo = 0;
                            List<int> colorsArray = new List<int>();
                            foreach (string itemcolor in colors)
                            {
                                string thecolor = itemcolor.Trim();
                                ItemColor foundcolor = itemcolors.Find(x => x.Label.ToLower() == thecolor.ToLower() || x.Label.Replace(" ", "").ToLower() == thecolor.ToLower());
                                int colorid = (foundcolor != null ? foundcolor.ID.Value : 12);
                                foreach (string itemsize in sizes)
                                {
                                    string thesize = itemsize.Trim();
                                    ItemSize foundsize = itemsizes.Find(x => x.Label.ToLower() == thesize.ToLower() || x.Label.Replace(" ", "").ToLower() == thesize.ToLower());
                                    int sizeid = (foundsize != null ? foundsize.ID.Value : 1);
                                    int qty = Convert.ToInt32(qtys);
                                    theprod.Size = new ItemSize();
                                    theprod.Size.ID = sizeid;
                                    theprod.Color = new ItemColor();
                                    theprod.Color.ID = colorid;
                                    theprod.GroupID = groupid;
                                    theprod.Quantity = qty;
                                    if (index > 0)
                                    {
                                        theprod.ID = null; //make new product
                                    }
                                    string theprodid = pMgr.SaveProductBasicInfo(theprod, true, true);

                                    string theColorFilename = string.Format("{0}/{0}", theprodid);
                                    if (index > 0)
                                    {
                                        theprod.ID = theprodid;
                                        theprod.Searchable = false;
                                        theprod.ThumbURL = theColorFilename;
                                        if (colorNo > 0)
                                        {
                                        }
                                    }
                                    FileInfo newFile = productdir.EnumerateFiles().ToList().Find(x => (x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg") && x.Name.Split('.')[0] == colorNo.ToString());
                                    if (newFile != null)
                                    {
                                        if (!Directory.Exists(imagesServerPath + theprodid))
                                            Directory.CreateDirectory(imagesServerPath + theprodid);
                                        string fileExtension = newFile.Extension.ToLower();
                                        string postNo = "";
                                        string prodfilePath = imagesServerPath + theColorFilename + postNo + fileExtension;
                                        newFile.CopyTo(prodfilePath, true);
                                        SaveImageWithPostNumber(imagesServerPath, theColorFilename, prodfilePath, postNo);
                                    }
                                    theprodid = pMgr.SaveProductBasicInfo(theprod, true, true);
                                    pMgr.ProductGroupSave(theprodid, groupid.ToString());
                                    pMgr.SaveProductInfo(theprodid, enName, enDesc, "1");
                                    pMgr.SaveProductInfo(theprodid, arName, arDesc, "2");
                                    comMgr.SaveSellableProduct(theprodid, price, false, 0, 0);
                                    index++;
                                }
                                colorNo++;
                                colorsArray.Add(colorid);
                            }
                        }
                    }
                }
                else
                {
                    DirectoryInfo productsdir = new DirectoryInfo(Server.MapPath(String.Format("~/content/vikik/{0}", catname)));
                    foreach (DirectoryInfo productdir in productsdir.EnumerateDirectories())
                    {
                        string productname = productdir.Name;
                        string prodid = pMgr.ProductSaveByName(productname, catid, new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"));
                        string proddesc = string.Empty;
                        List<string> vs = new List<string>();
                        string filename = string.Format("{0}/{0}", prodid);
                        string barcode = string.Empty;
                        double price = 0;
                        double oldprice = 0;
                        string tags = string.Empty;
                        int n = 0;
                        string Images = "";
                        int lang = 1;
                        string arDesc = "", enDesc = "";
                        string enName = "", arName = "";
                        string colorsstr = "", sizestr = "", qtystr = "";
                        foreach (FileInfo file in productdir.EnumerateFiles())
                        {
                            proddesc = "";
                            if (file.Extension.ToLower() == ".txt" && file.Name.Substring(0, 1) != ".")
                            {
                                lang = file.Name.ToLower().Contains("ar") ? 2 : 1;
                                string prodinfo = file.OpenText().ReadToEnd();
                                string[] prodinfoparts = prodinfo.Split(("\n").ToCharArray());
                                if (prodinfoparts.Length >= 4)
                                {
                                    productname = prodinfoparts[0].Replace("\n", "");
                                    if (lang == 1)
                                        enName = productname.Replace("\n", "").Replace("\r", "");
                                    else
                                        arName = productname.Replace("\n", "").Replace("\r", "");
                                    barcode = prodinfoparts[1].Replace("\n", "").Replace("\r", "");
                                    try
                                    {
                                        price = Convert.ToDouble(prodinfoparts[5]);
                                    }
                                    catch (Exception e)
                                    {
                                        price = 0;
                                    }
                                    try
                                    {
                                        oldprice = Convert.ToDouble(prodinfoparts[6]);
                                    }
                                    catch (Exception e)
                                    {
                                        oldprice = 0;
                                    }
                                    colorsstr = prodinfoparts[2].Replace("\n", "").Replace("\r", "");
                                    sizestr = prodinfoparts[3].Replace("\n", "").Replace("\r", "");
                                    qtystr = prodinfoparts[4].Replace("\n", "").Replace("\r", "");

                                    /* for (int i = 7; i < prodinfoparts.Length; i++)
                                     {
                                         proddesc += prodinfoparts[i];
                                     }
                                     proddesc = proddesc.Replace("\n", "<br/>");
                                     proddesc = proddesc.Replace(" \r", "");*/
                                    enDesc = proddesc;
                                }
                                else if (prodinfoparts.Length >= 4 && prodinfoparts.Length < 8)
                                {
                                    productname = prodinfoparts[0].Replace("\n", "");
                                    if (lang == 1)
                                        enName = productname;
                                    else
                                        arName = productname;
                                    tags = prodinfoparts[2].Replace("\n", "");
                                    for (int i = 4; i < prodinfoparts.Length; i++)
                                    {
                                        proddesc += prodinfoparts[i];
                                    }
                                    proddesc = proddesc.Replace("\n", "<br/>");
                                    if (lang == 1)
                                        enDesc = proddesc;
                                    else
                                        arDesc = proddesc;
                                }
                                else
                                {
                                    if (file.Name.ToLower().Contains("ar"))
                                    {
                                        arName = prodinfoparts[0].Replace("\n", "").Replace("\r", "");
                                    }
                                }
                                //else
                                // {
                                //proddesc = prodinfo;
                                // }
                            }
                            //else if (((file.Extension.ToLower() == ".jpg" && file.Name.Substring(0, 1) != ".") || (file.Extension.ToLower() == ".png" && file.Name.Substring(0, 1) != ".")) && !file.Name.Contains("_") && file.Name.Split('.')[0] == "0")
                            //{
                            //    if (!Directory.Exists(imagesServerPath + prodid))
                            //        Directory.CreateDirectory(imagesServerPath + prodid);
                            //    string fileExtension = file.Extension.ToLower();
                            //    string postNo = n > 0 ? n.ToString() : "";
                            //    string prodfilePath = imagesServerPath + filename + postNo + fileExtension;
                            //    file.CopyTo(prodfilePath);
                            //    SaveImageWithPostNumber(imagesServerPath, filename, prodfilePath, postNo);
                            //    if (!string.IsNullOrEmpty(Images))
                            //    {
                            //        vs.Add(postNo);
                            //        Images += "," + postNo;
                            //    }
                            //    else
                            //    {
                            //        vs.Add(postNo);
                            //        Images = postNo;
                            //    }
                            //    System.IO.File.Delete(filePath);
                            //    n++;
                            //}
                        }
                        string NewProdID = pMgr.SaveProduct(new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"), prodid, catid, productname, lang == 1 ? enDesc : arDesc, price, (oldprice == price ? null : (double?)oldprice), filename, Images, true, barcode, tags);
                        pMgr.SaveProductInfo(NewProdID, lang == 1 ? arName : enName, lang == 1 ? arDesc : enDesc, lang == 1 ? "2" : "1");
                        CommerceManager comMgr = new CommerceManager(ConnectionString);
                        GeneralManager gMgr = new GeneralManager(ConnectionString);
                        comMgr.SaveSellableProduct(NewProdID, price, false, 0, 0);
                        int groupid = gMgr.ItemGroupSave(null, enName);
                        string[] colors = colorsstr.Split(",".ToCharArray());
                        string[] sizes = sizestr.Split(",".ToCharArray());
                        string[] qtys = qtystr.Split(",".ToCharArray());
                        List<ItemColor> itemcolors = gMgr.ItemColorsGet();
                        List<ItemSize> itemsizes = gMgr.ItemSizesGet();
                        int index = 0;
                        Product theprod = pMgr.GetProduct(NewProdID, CurrentLanguage);
                        int colorNo = 0;
                        List<int> colorsArray = new List<int>();
                        foreach (string itemcolor in colors)
                        {
                            string thecolor = itemcolor.Trim();
                            ItemColor foundcolor = itemcolors.Find(x => x.Label.ToLower() == thecolor.ToLower() || x.Label.Replace(" ", "").ToLower() == thecolor.ToLower());
                            int colorid = (foundcolor != null ? foundcolor.ID.Value : 12);
                            foreach (string itemsize in sizes)
                            {
                                string thesize = itemsize.Trim();
                                ItemSize foundsize = itemsizes.Find(x => x.Label.ToLower() == thesize.ToLower() || x.Label.Replace(" ", "").ToLower() == thesize.ToLower());
                                int sizeid = (foundsize != null ? foundsize.ID.Value : 1);
                                //int qty = Convert.ToInt32(qtys[0]);
                                theprod.Size = new ItemSize();
                                theprod.Size.ID = sizeid;
                                theprod.Color = new ItemColor();
                                theprod.Color.ID = colorid;
                                theprod.GroupID = groupid;
                                theprod.Quantity = 5;
                                if (index > 0)
                                {
                                    theprod.ID = null; //make new product
                                }
                                string theprodid = pMgr.SaveProductBasicInfo(theprod, true, true);

                                string theColorFilename = string.Format("{0}/{0}", theprodid);
                                if (index > 0)
                                {
                                    theprod.ID = theprodid;
                                    theprod.Searchable = false;
                                    theprod.ThumbURL = theColorFilename;
                                    if (colorNo > 0)
                                    {
                                    }
                                }
                                FileInfo newFile = productdir.EnumerateFiles().ToList().Find(x => (x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg") && x.Name.Split('.')[0] == colorNo.ToString());
                                if (newFile != null)
                                {
                                    if (!Directory.Exists(imagesServerPath + theprodid))
                                        Directory.CreateDirectory(imagesServerPath + theprodid);
                                    string fileExtension = newFile.Extension.ToLower();
                                    string postNo = "";
                                    string prodfilePath = imagesServerPath + theColorFilename + postNo + fileExtension;
                                    newFile.CopyTo(prodfilePath, true);
                                    SaveImageWithPostNumber(imagesServerPath, theColorFilename, prodfilePath, postNo);
                                }
                                theprodid = pMgr.SaveProductBasicInfo(theprod, true, true);
                                pMgr.ProductGroupSave(theprodid, groupid.ToString());
                                pMgr.SaveProductInfo(theprodid, enName, enDesc, "1");
                                pMgr.SaveProductInfo(theprodid, arName, arDesc, "2");
                                comMgr.SaveSellableProduct(theprodid, price, false, 0, 0);
                                index++;
                            }
                            colorNo++;
                            colorsArray.Add(colorid);
                        }
                    }

                }
            }
            return View();
        }

        public ActionResult ImportProducts()
        {
            return View();
        }

        public ActionResult ProdImporter3()
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            //pMgr.ClearProducts("KhaledConfirmed");
            string imagesServerPath = Server.MapPath("~/content/products/");
            DirectoryInfo allproductsdir = new DirectoryInfo(imagesServerPath);
            //allproductsdir.Delete(true);
            allproductsdir.Create();
            StringBuilder prod = new StringBuilder("");
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/content/vikik"));
            foreach (DirectoryInfo catdir in directory.EnumerateDirectories())
            {
                string catname = catdir.Name;
                string catid = pMgr.CategorySaveByName(catname, null);
                string catfilename = string.Format("{0}", catid);
                string filePath = catdir.FullName + "image.png";
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/content/categories/"));
                if (System.IO.File.Exists(filePath))
                {
                    FileInfo imgfile = new FileInfo(filePath);
                    if (!System.IO.File.Exists(Server.MapPath("~/content/categories/") + catfilename + ".png"))
                    {
                        imgfile.CopyTo(Server.MapPath("~/content/categories/") + catfilename + ".png");
                        SaveImageWithPostNumber(Server.MapPath("~/content/categories/"), catfilename, filePath, "");
                    }
                }
                Category oldcat = pMgr.GetCategory(catid, CurrentLanguage);
                pMgr.SaveCategoryBasicInfo(catid, oldcat.ThumbURL, null, oldcat.Rank, true);
                if (oldcat.HasChildren)
                {
                    DirectoryInfo subcatsdir = new DirectoryInfo(Server.MapPath(String.Format("~/content/vikik/{0}", catname)));
                    foreach (DirectoryInfo subcatdir in subcatsdir.EnumerateDirectories())
                    {
                        string subcatname = subcatdir.Name;
                        string subcatid = pMgr.CategorySaveByName(subcatname, catid);
                        string subcatfilename = string.Format("{0}", subcatid);
                        string subfilePath = subcatdir.FullName + "image.png";
                        if (System.IO.File.Exists(subfilePath))
                        {
                            FileInfo subimgfile = new FileInfo(subfilePath);
                            if (!System.IO.File.Exists(Server.MapPath("~/content/categories/") + subcatfilename + ".png"))
                            {
                                subimgfile.CopyTo(Server.MapPath("~/content/categories/") + subcatfilename + ".png");
                                SaveImageWithPostNumber(Server.MapPath("~/content/categories/"), subcatfilename, subfilePath, "");
                            }
                        }
                        Category oldsubcat = pMgr.GetCategory(subcatid, CurrentLanguage);
                        pMgr.SaveCategoryBasicInfo(subcatid, oldsubcat.ThumbURL, catid, oldsubcat.Rank, true);
                        DirectoryInfo productsdir = new DirectoryInfo(Server.MapPath(String.Format("~/content/vikik/{0}/{1}", catname, subcatname)));
                        foreach (DirectoryInfo productdir in productsdir.EnumerateDirectories())
                        {
                            string productname = productdir.Name;
                            string prodid = pMgr.ProductSaveByName(productname, subcatid, new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"));
                            string proddesc = string.Empty;
                            List<string> vs = new List<string>();
                            string filename = string.Format("{0}/{0}", prodid);
                            string barcode = string.Empty;
                            double price = 0;
                            double oldprice = 0;
                            string tags = string.Empty;
                            int n = 0;
                            string Images = "";
                            int lang = 1;
                            string arDesc = "", enDesc = "";
                            string enName = "", arName = "";
                            string colorsstr = "", sizestr = "", qtystr = "";
                            foreach (FileInfo file in productdir.EnumerateFiles())
                            {
                                proddesc = "";
                                if (file.Extension.ToLower() == ".txt" && file.Name.Substring(0, 1) != ".")
                                {
                                    lang = file.Name.ToLower().Contains("ar") ? 2 : 1;
                                    string prodinfo = file.OpenText().ReadToEnd();
                                    string[] prodinfoparts = prodinfo.Split(("\n").ToCharArray());
                                    if (prodinfoparts.Length >= 4)
                                    {
                                        productname = prodinfoparts[0].Replace("\n", "");
                                        if (lang == 1)
                                            enName = productname.Replace("\n", "").Replace("\r", "");
                                        else
                                            arName = productname.Replace("\n", "").Replace("\r", "");
                                        barcode = prodinfoparts[1].Replace("\n", "").Replace("\r", "");
                                        try
                                        {
                                            price = Convert.ToDouble(prodinfoparts[5]);
                                        }
                                        catch (Exception e)
                                        {
                                            price = 0;
                                        }
                                        try
                                        {
                                            oldprice = Convert.ToDouble(prodinfoparts[6]);
                                        }
                                        catch (Exception e)
                                        {
                                            oldprice = 0;
                                        }
                                        colorsstr = prodinfoparts[2].Replace("\n", "").Replace("\r", "");
                                        sizestr = prodinfoparts[3].Replace("\n", "").Replace("\r", "");
                                        qtystr = prodinfoparts[4].Replace("\n", "").Replace("\r", "");
                                        enDesc = proddesc;
                                    }
                                    else if (prodinfoparts.Length >= 4 && prodinfoparts.Length < 8)
                                    {
                                        productname = prodinfoparts[0].Replace("\n", "");
                                        if (lang == 1)
                                            enName = productname;
                                        else
                                            arName = productname;
                                        tags = prodinfoparts[2].Replace("\n", "");
                                        for (int i = 4; i < prodinfoparts.Length; i++)
                                        {
                                            proddesc += prodinfoparts[i];
                                        }
                                        proddesc = proddesc.Replace("\n", "<br/>");
                                        if (lang == 1)
                                            enDesc = proddesc;
                                        else
                                            arDesc = proddesc;
                                    }
                                    else
                                    {
                                        if (file.Name.ToLower().Contains("ar"))
                                        {
                                            arName = prodinfoparts[0].Replace("\n", "").Replace("\r", "");
                                        }
                                    }
                                }
                            }
                            string NewProdID = pMgr.SaveProduct(new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"), prodid, subcatid, productname, lang == 1 ? enDesc : arDesc, price, (oldprice == price ? null : (double?)oldprice), filename, Images, true, barcode, tags);
                            pMgr.SaveProductInfo(NewProdID, lang == 1 ? arName : enName, lang == 1 ? arDesc : enDesc, lang == 1 ? "2" : "1");
                            CommerceManager comMgr = new CommerceManager(ConnectionString);
                            GeneralManager gMgr = new GeneralManager(ConnectionString);
                            comMgr.SaveSellableProduct(NewProdID, price, false, 0, 0);
                            int groupid = gMgr.ItemGroupSave(null, enName);
                            string[] colors = colorsstr.Split(",".ToCharArray());
                            string[] sizes = sizestr.Split(",".ToCharArray());
                            string qtys = qtystr;
                            List<ItemColor> itemcolors = gMgr.ItemColorsGet();
                            List<ItemSize> itemsizes = gMgr.ItemSizesGet();
                            int index = 0;
                            Product theprod = pMgr.GetProduct(NewProdID, CurrentLanguage);
                            int colorNo = 0;
                            List<int> colorsArray = new List<int>();
                            ProcessColors(colors, sizes, qtys, itemcolors, itemsizes, groupid, theprod, productdir, imagesServerPath, arName, arDesc, enName, enDesc, price);
                        }
                    }
                }
                else
                {
                    DirectoryInfo productsdir = new DirectoryInfo(Server.MapPath(String.Format("~/content/vikik/{0}", catname)));
                    foreach (DirectoryInfo productdir in productsdir.EnumerateDirectories())
                    {
                        string productname = productdir.Name;
                        string prodid = pMgr.ProductSaveByName(productname, catid, new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"));
                        string proddesc = string.Empty;
                        List<string> vs = new List<string>();
                        string filename = string.Format("{0}/{0}", prodid);
                        string barcode = string.Empty;
                        double price = 0;
                        double oldprice = 0;
                        string tags = string.Empty;
                        int n = 0;
                        string Images = "";
                        int lang = 1;
                        string arDesc = "", enDesc = "";
                        string enName = "", arName = "";
                        string colorsstr = "", sizestr = "", qtystr = "";
                        foreach (FileInfo file in productdir.EnumerateFiles())
                        {
                            proddesc = "";
                            if (file.Extension.ToLower() == ".txt" && file.Name.Substring(0, 1) != ".")
                            {
                                lang = file.Name.ToLower().Contains("ar") ? 2 : 1;
                                string prodinfo = file.OpenText().ReadToEnd();
                                string[] prodinfoparts = prodinfo.Split(("\n").ToCharArray());
                                if (prodinfoparts.Length >= 4)
                                {
                                    productname = prodinfoparts[0].Replace("\n", "");
                                    if (lang == 1)
                                        enName = productname.Replace("\n", "").Replace("\r", "");
                                    else
                                        arName = productname.Replace("\n", "").Replace("\r", "");
                                    barcode = prodinfoparts[1].Replace("\n", "").Replace("\r", "");
                                    try
                                    {
                                        price = Convert.ToDouble(prodinfoparts[5]);
                                    }
                                    catch (Exception e)
                                    {
                                        price = 0;
                                    }
                                    try
                                    {
                                        oldprice = Convert.ToDouble(prodinfoparts[6]);
                                    }
                                    catch (Exception e)
                                    {
                                        oldprice = 0;
                                    }
                                    colorsstr = prodinfoparts[2].Replace("\n", "").Replace("\r", "");
                                    sizestr = prodinfoparts[3].Replace("\n", "").Replace("\r", "");
                                    qtystr = prodinfoparts[4].Replace("\n", "").Replace("\r", "");

                                    /* for (int i = 7; i < prodinfoparts.Length; i++)
                                     {
                                         proddesc += prodinfoparts[i];
                                     }
                                     proddesc = proddesc.Replace("\n", "<br/>");
                                     proddesc = proddesc.Replace(" \r", "");*/
                                    enDesc = proddesc;
                                }
                                else if (prodinfoparts.Length >= 4 && prodinfoparts.Length < 8)
                                {
                                    productname = prodinfoparts[0].Replace("\n", "");
                                    if (lang == 1)
                                        enName = productname;
                                    else
                                        arName = productname;
                                    tags = prodinfoparts[2].Replace("\n", "");
                                    for (int i = 4; i < prodinfoparts.Length; i++)
                                    {
                                        proddesc += prodinfoparts[i];
                                    }
                                    proddesc = proddesc.Replace("\n", "<br/>");
                                    if (lang == 1)
                                        enDesc = proddesc;
                                    else
                                        arDesc = proddesc;
                                }
                                else
                                {
                                    if (file.Name.ToLower().Contains("ar"))
                                    {
                                        arName = prodinfoparts[0].Replace("\n", "").Replace("\r", "");
                                    }
                                }
                                //else
                                // {
                                //proddesc = prodinfo;
                                // }
                            }
                            //else if (((file.Extension.ToLower() == ".jpg" && file.Name.Substring(0, 1) != ".") || (file.Extension.ToLower() == ".png" && file.Name.Substring(0, 1) != ".")) && !file.Name.Contains("_") && file.Name.Split('.')[0] == "0")
                            //{
                            //    if (!Directory.Exists(imagesServerPath + prodid))
                            //        Directory.CreateDirectory(imagesServerPath + prodid);
                            //    string fileExtension = file.Extension.ToLower();
                            //    string postNo = n > 0 ? n.ToString() : "";
                            //    string prodfilePath = imagesServerPath + filename + postNo + fileExtension;
                            //    file.CopyTo(prodfilePath);
                            //    SaveImageWithPostNumber(imagesServerPath, filename, prodfilePath, postNo);
                            //    if (!string.IsNullOrEmpty(Images))
                            //    {
                            //        vs.Add(postNo);
                            //        Images += "," + postNo;
                            //    }
                            //    else
                            //    {
                            //        vs.Add(postNo);
                            //        Images = postNo;
                            //    }
                            //    System.IO.File.Delete(filePath);
                            //    n++;
                            //}
                        }
                        string NewProdID = pMgr.SaveProduct(new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA"), prodid, catid, productname, lang == 1 ? enDesc : arDesc, price, (oldprice == price ? null : (double?)oldprice), filename, Images, true, barcode, tags);
                        pMgr.SaveProductInfo(NewProdID, lang == 1 ? arName : enName, lang == 1 ? arDesc : enDesc, lang == 1 ? "2" : "1");
                        CommerceManager comMgr = new CommerceManager(ConnectionString);
                        GeneralManager gMgr = new GeneralManager(ConnectionString);
                        comMgr.SaveSellableProduct(NewProdID, price, false, 0, 0);
                        int groupid = gMgr.ItemGroupSave(null, enName);
                        string[] colors = colorsstr.Split(",".ToCharArray());
                        string[] sizes = sizestr.Split(",".ToCharArray());
                        string qtys = qtystr;
                        List<ItemColor> itemcolors = gMgr.ItemColorsGet();
                        List<ItemSize> itemsizes = gMgr.ItemSizesGet();

                        Product theprod = pMgr.GetProduct(NewProdID, CurrentLanguage);

                        ProcessColors(colors, sizes, qtys, itemcolors, itemsizes, groupid, theprod, productdir, imagesServerPath, arName, arDesc, enName, enDesc, price);
                    }

                }
            }
            return View("ImportProducts");
        }
        private void ProcessColors(string[] colors, string[] sizes, string qtys, List<ItemColor> itemcolors, List<ItemSize> itemsizes, int groupid, Product theprod, DirectoryInfo productdir, string imagesServerPath, string arName, string arDesc, string enName, string enDesc, double price)
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            CommerceManager comMgr = new CommerceManager(ConnectionString);
            string Images;
            int index = 0;
            int colorNo = 0;
            foreach (string itemcolor in colors)
            {
                string thecolor = itemcolor.Trim();
                ItemColor foundcolor = itemcolors.Find(x => x.Label.ToLower() == thecolor.ToLower() || x.Label.Replace(" ", "").ToLower() == thecolor.ToLower());
                int colorid = (foundcolor != null ? foundcolor.ID.Value : 12);
                foreach (string itemsize in sizes)
                {
                    string thesize = itemsize.Trim();
                    ItemSize foundsize = itemsizes.Find(x => x.Label.ToLower() == thesize.ToLower() || x.Label.Replace(" ", "").ToLower() == thesize.ToLower());
                    int sizeid = (foundsize != null ? foundsize.ID.Value : 1);
                    int qty = Convert.ToInt32(qtys);
                    theprod.Size = new ItemSize();
                    theprod.Size.ID = sizeid;
                    theprod.Color = new ItemColor();
                    theprod.Color.ID = colorid;
                    theprod.GroupID = groupid;
                    theprod.Quantity = qty;
                    if (index > 0)
                    {
                        theprod.ID = null; //make new product
                    }
                    string theprodid = pMgr.SaveProductBasicInfo(theprod, true, true);

                    string theColorFilename = string.Format("{0}/{0}", theprodid);
                    if (index > 0)
                    {
                        theprod.ID = theprodid;
                        theprod.Searchable = false;
                        theprod.ThumbURL = theColorFilename;
                        if (colorNo > 0)
                        {
                        }
                    }
                    List<FileInfo> newFiles = productdir.EnumerateFiles().ToList().FindAll(x => (x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg") && x.Name.Split('.')[0].Split('-')[0] == colorNo.ToString());
                    if (newFiles.Count > 0)
                    {
                        if (!Directory.Exists(imagesServerPath + theprodid))
                            Directory.CreateDirectory(imagesServerPath + theprodid);
                        int i = 0;
                        Images = "";
                        theprod.OutDoorThumbURL = "";
                        foreach (FileInfo file in newFiles)
                        {
                            if (file.Name.ToLower().Contains("out"))
                            {
                                //save the out image here then continue
                                string outfileExtension = file.Extension.ToLower();
                                string outpostNo = "";
                                string outprodfilePath = imagesServerPath + theColorFilename + "-out" + outfileExtension;
                                file.CopyTo(outprodfilePath, true);
                                SaveImageWithPostNumber(imagesServerPath, theColorFilename + "-out", outprodfilePath, outpostNo);
                                theprod.OutDoorThumbURL = theColorFilename + "-out";
                                continue;
                            }

                            string fileExtension = file.Extension.ToLower();
                            string postNo = i == 0 ? "" : i.ToString();
                            string prodfilePath = imagesServerPath + theColorFilename + postNo + fileExtension;
                            file.CopyTo(prodfilePath, true);
                            SaveImageWithPostNumber(imagesServerPath, theColorFilename, prodfilePath, postNo);
                            if (!string.IsNullOrEmpty(Images))
                            {
                                Images += "," + postNo;
                            }
                            else
                            {
                                Images = postNo;
                            }
                            i++;
                        }
                        theprod.Images = Images;
                    }
                    theprodid = pMgr.SaveProductBasicInfo(theprod, true, true);
                    pMgr.ProductGroupSave(theprodid, groupid.ToString());
                    pMgr.SaveProductInfo(theprodid, enName, enDesc, "1");
                    pMgr.SaveProductInfo(theprodid, arName, arDesc, "2");
                    comMgr.SaveSellableProduct(theprodid, price, false, 0, 0);
                    index++;
                }
                colorNo++;
                //colorsArray.Add(colorid);
            }
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




        public ActionResult DeleteOrphanFolders()
        {

            ProductDapper dapper = new ProductDapper();

            // Step 1: Get the list of folder names from the server directory
            string serverFolderPath = Server.MapPath("~/content/products");
            string[] serverFolders = Directory.GetDirectories(serverFolderPath);
            List<String> ProductIDs = dapper.GetAllProductIDs();
            // Step 2: Get the list of existing folder names from the database


            // Step 3: Find the folders that don't exist in the database
            var orphanFolders = serverFolders.Where(folder => !ProductIDs.Contains(Path.GetFileName(folder)));

            // Step 4: Delete the orphan folders from the server

            int X = 1;
            foreach (var orphanFolder in orphanFolders)
            {

                Response.Write("Delete" + X + "  with ID   " + orphanFolder);

                Directory.Delete(orphanFolder, recursive: true);
            }

            return Content("Orphan folders deleted successfully.");
        }
        public void DeleteOrphanImg()
        {
            CMSConfigurations config = CMSConfigurations.GetSection();
           

            // Replace this with the root directory path containing the image folders
            string rootDirectory = Server.MapPath("~/content/products");

            // List of allowed image sizes (desired width and height pairs)
            List<(int width, int height)> allowedSizes = new List<(int, int)>
            {
               
            };
            foreach (CMSImageInfo imginfo in config.Images)
            {
                allowedSizes.Add((imginfo.Width, imginfo.Height));
             }
            // Get the list of image files from all subdirectories
            List<string> imageFiles = GetImageFiles(rootDirectory);

            // Delete images not matching the allowed sizes
            foreach (string imagePath in imageFiles)
            {
                if (!IsImageSizeAllowed(imagePath, allowedSizes))
                {
                    System.IO.File.Delete(imagePath);
                    Console.WriteLine($"Deleted: {imagePath}");
                }
            }
        }
        static List<string> GetImageFiles(string directoryPath)
        {
            List<string> imageFiles = new List<string>();

            try
            {
                // Search for image files in the current directory
                imageFiles.AddRange(Directory.GetFiles(directoryPath, "*.jpg"));
                imageFiles.AddRange(Directory.GetFiles(directoryPath, "*.jpeg"));
                imageFiles.AddRange(Directory.GetFiles(directoryPath, "*.png"));
                imageFiles.AddRange(Directory.GetFiles(directoryPath, "*.webp"));

                // Recursively search for image files in subdirectories
                foreach (string subdirectory in Directory.GetDirectories(directoryPath))
                {
                    imageFiles.AddRange(GetImageFiles(subdirectory));
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle exceptions related to directory access permissions
                // You can choose to log or ignore these errors depending on your requirements.
            }

            return imageFiles;
        }

        static bool IsImageSizeAllowed(string imagePath, List<(int width, int height)> allowedSizes)
        {
            try
            {
                // Load the image to get its width and height
                using (var image = System.Drawing.Image.FromFile(imagePath))
                {
                    int width = image.Width;
                    int height = image.Height;

                    return allowedSizes.Any(size => size.width == width && size.height == height);
                }
            }
            catch (Exception)
            {
                // Handle exceptions related to image loading or processing
                // You can choose to log or ignore these errors depending on your requirements.
                return false;
            }
        }


        public ActionResult Getappleappsiteassociation()
        {
            // Path to your JSON file
            string filePath =Server.MapPath("~/images/apple-app-site-association.json")  ;

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound(); // Return a 404 Not Found response if the file doesn't exist
            }

            // Read the contents of the JSON file
            string jsonData = System.IO.File.ReadAllText(filePath);

            // Set the appropriate response headers
            Response.ContentType = "application/json";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            // Return the JSON data
            return Content(jsonData);
        }
        
        public ActionResult Getassetlinks()
        {
            // Path to your JSON file
            string filePath =Server.MapPath("~/images/assetlinks.json")  ;

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound(); // Return a 404 Not Found response if the file doesn't exist
            }

            // Read the contents of the JSON file
            string jsonData = System.IO.File.ReadAllText(filePath);

            // Set the appropriate response headers
            Response.ContentType = "application/json";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            // Return the JSON data
            return Content(jsonData);
        }
    


    }
}