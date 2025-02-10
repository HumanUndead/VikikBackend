using CMS;
using CMS.Config;
using CMS.Helpers;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace vikik.Controllers
{
    public class ClassifiedController : KensoftController
    {
        public ActionResult Delete(string id)
        {
            if (CurrentUser == null)
            {
                Response.Redirect("~/user/ads");
            }
            ProductManager pMgr = new ProductManager(ConnectionString);
            Product prod = pMgr.GetProduct(id, CurrentLanguage);
            if (prod.OwnerID == CurrentUser.ID || prod.OwnerID== (Guid)HttpContext.Application["AdminID"])
            {
                pMgr.DeleteProduct(id);
                Response.Redirect("~/user/ads/delsuccess");
            }
            Response.Redirect("~/user/ads/delfail");
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Edit(string id)
        {
            if (CurrentUser == null)
            {
                Response.Redirect("~/user/ads");
            }
            ProductManager mgr = new ProductManager(ConnectionString);
           
            LookupManager lmgr = new LookupManager(ConnectionString);
            Product prod = mgr.GetProduct(id, CurrentLanguage);
            if (prod.OwnerID != CurrentUser.ID && CurrentUser.ID != (Guid)HttpContext.Application["AdminID"])
            {
                Response.Redirect("~/user/ads");
            }
            ViewBag.SelectedRegion = string.Format("{0},{1}", "1", "1");
            Category cat = mgr.GetCategory(prod.CategoryID, CurrentLanguage);
            cat = mgr.GetCategory(cat.ParentID, CurrentLanguage);
            ViewBag.Categories = mgr.GetCategoryCategories(cat.ID, CurrentLanguage);
            ViewBag.SelectedCategory = cat.Name;
            
            ViewBag.Phone = prod.Specs.Find(x => x.ID == "18")!=null?prod.Specs.Find(x => x.ID == "18").Value:"";
            ViewBag.CategorySpecs = mgr.GetCategorySpecs(cat.ID, CurrentLanguage, true).FindAll(x => x.ID != "13");
            return View("editlisting", prod);
        }
        public ActionResult Upgrade(string id)
        {
            if (CurrentUser == null)
            {
                Response.Redirect("~/user/ads");
            }
            ProductManager pMgr = new ProductManager(ConnectionString);
            Product prod = pMgr.GetProduct(id, CurrentLanguage);
            if (prod.OwnerID != CurrentUser.ID)
            {
                Response.Redirect("~/user/ads");
            }
            else
            {
                ViewBag.PageName = Resources.Strings.UpgradeYourAd;
                ViewBag.PropertyID = prod.ID;
            }
            return View();
        }
        public ActionResult AddListing()
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Category> mainCats = pMgr.GetCategoryCategories("20063", CurrentLanguage);
            ViewBag.MainCats = mainCats;
            List<Category> subCats = new List<Category>();
            foreach(Category cat in mainCats)
            {
                subCats.AddRange(pMgr.GetCategoryCategories(cat.ID,CurrentLanguage));
            }
            ViewBag.SubCats = subCats;
            return View();
        }
        public ActionResult AddShopListing()
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Category> mainCats = pMgr.GetCategoryCategories("7", CurrentLanguage);
            ViewBag.MainCats = mainCats;
            List<Category> subCats = new List<Category>();
            foreach (Category cat in mainCats)
            {
                subCats.AddRange(pMgr.GetCategoryCategories(cat.ID, CurrentLanguage));
            }
            ViewBag.SubCats = subCats;
            return View();
        }
        public ActionResult AddListing2(string catid)
        {
            ViewBag.CatID = catid;
            ProductManager pMgr = new ProductManager(ConnectionString);
            Category cat = pMgr.GetCategory(catid, CurrentLanguage);
            ViewBag.SelectedCategory = cat.Name;
            ViewBag.CategorySpecs = pMgr.GetCategorySpecs(cat.ID, CurrentLanguage, true).FindAll(x => x.ID != "13");
            ViewBag.Categories = pMgr.GetCategoryCategories(cat.ID,CurrentLanguage);
            ViewBag.UserLogged = CurrentUser != null;
            if (CurrentUser != null)
            {
                ViewBag.Phone = CurrentUser.Phone;
            }
            LookupManager lookMgr = new LookupManager(ConnectionString);
            /*List<KeyValuePair<string, string>> Regions = new List<KeyValuePair<string, string>>();
            List<Lookup> Cities = lookMgr.GetLookUpCategoryItems(2, CurrentLanguage);
            ViewBag.Locations = lookMgr.GetLookUpCategoryItems(3, CurrentLanguage);
            ViewBag.Cities = Cities;
            foreach (Lookup city in Cities)
            {
                List<Lookup> Areas = lookMgr.GetLookUpCategoryItemsFilterParent(3, city.ID.Value, CurrentLanguage);
                foreach (Lookup area in Areas)
                {
                    Regions.Add(new KeyValuePair<string, string>(string.Format("{0}>{1}", city.ID, area.ID), string.Format("{0} > {1}", city.Name, area.Name)));
                }
            }
            ViewBag.Regions = Regions;*/
            return View("addlisting2");
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddListing3(string id)
        {
            string catID = Request["catid"];
            CMS.Config.CMSConfigurations config = CMSConfigurations.GetSection();
            if (CurrentUser == null)
            {
                MemberManager mMgr = new MemberManager(ConnectionString);
                SQLResult result;
                string email = Request["email"];
                string password = Request["password"];
                string phone = Request["phone"];
                switch (Request["radMember"])
                {
                    case "0": //new user
                        Guid? userID;
                        result = mMgr.RegisterUser(email, password, "New Member", out userID);
                        if (result == SQLResult.SUCCESS)
                        {
                            Session["User"] = mMgr.GetUser(userID.Value, CurrentLanguage, true);
                            CurrentUser.Phone = phone;
                            int res;
                            mMgr.SaveUserBasicInfo(CurrentUser, null, out res);
                            mMgr.DoActivateUser(userID.Value, false);
                            Guid resetID;
                            string name;
                            mMgr.SendResetPassword(email, CurrentLanguage, out name, out resetID);
                            string body = string.Format(Resources.Strings.EmailWelcomeBody, System.Configuration.ConfigurationManager.AppSettings["SiteURL"], HttpUtility.UrlEncode(resetID.ToString()), HttpUtility.UrlEncode(email));
                            Helper.SendEmail(System.Configuration.ConfigurationManager.AppSettings["FromEmail"], email, Resources.Strings.EmailWelcomeSubject, body);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = Resources.Strings.UserAlreadyExists;
                        }
                        break;
                    case "1": //old user
                        LoginUser(email, password);
                        break;

                }
            }
            if (CurrentUser != null)
            {
                ProductManager mgr = new ProductManager(ConnectionString);

                string filename = string.Format("{0}\\{1}\\{2}\\{3}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, GeneralHelper.GenerateNewID());

                string title = Request["title"];
                string description = Request["description"];
                string price = Request["price"];
                string hdnfiles = Request["hdnImages"];
                string ignorenumbers = Request["hdnoldimages"];
                string files = ignorenumbers + (string.IsNullOrEmpty(ignorenumbers) || string.IsNullOrEmpty(hdnfiles) ? "" : ",") + hdnfiles;
                if (!string.IsNullOrEmpty(id))
                {
                    Product prod = mgr.GetProduct(id, CurrentLanguage);
                    if (prod.OwnerID != CurrentUser.ID && prod.OwnerID!=(Guid)HttpContext.Application["AdminID"])
                    {
                        Response.Redirect("error");
                    }
                    filename = !string.IsNullOrEmpty(prod.ThumbURL) ? prod.ThumbURL : filename;
                }
                string prodID = mgr.SaveProduct(CurrentUser.ID, id, Request["subcat"], title, description.Replace("\n", "<br/>"), Convert.ToDouble(price), null, filename, GetImages(files.Split((",").ToCharArray()), string.Empty), false);//muimages.Images
                //now save the specs based on category
                List<Spec> specs = new List<Spec>();
                Category cat = mgr.GetCategory(catID, CurrentLanguage);
                foreach (Spec spec in mgr.GetCategorySpecs(cat.ID, CurrentLanguage, true))
                {
                    switch (spec.Type)
                    {
                        case SpecType.Text:
                        case SpecType.Numeric:
                            spec.Value = Request["spec" + spec.ID];
                            specs.Add(spec);
                            break;
                        case SpecType.AdminSelection:
                            int specsource = Convert.ToInt32(spec.Source);
                            if (specsource > 3)
                            {
                                spec.Value = Request["spec" + spec.ID];
                            }
                            else
                            {
                                if (specsource == 1) //Country
                                {
                                    spec.Value = "1"; //Jordan
                                }
                                if (specsource == 2) //city
                                {
                                    spec.Value = Request["spec" + spec.ID].Split((">").ToCharArray())[0];
                                }
                                if (specsource == 3) //area
                                {
                                    spec.Value = Request["spec" + spec.ID].Split((">").ToCharArray())[1];
                                }
                            }
                            specs.Add(spec);
                            break;
                        case SpecType.Available:
                            spec.Value = Request["spec" + spec.ID];
                            specs.Add(spec);
                            break;
                    }
                }
                Spec phonespec = new Spec();
                phonespec.ID = "18";
                phonespec.Value = Request["phone"];
                specs.Add(phonespec);
                mgr.SaveProductSpecs(prodID, specs, CurrentLanguage);
                CommerceManager comMgr = new CommerceManager(ConnectionString);
                comMgr.SaveSellableProduct(prodID, Convert.ToDouble(price), false, 1, Period.Years);
                string imagesServerPath = Server.MapPath("~/content/products/");
                if (!string.IsNullOrEmpty(hdnfiles))
                {
                    foreach (CMSImageInfo imginfo in config.Images)
                    {
                        Helper.SaveImage(hdnfiles.Split((",").ToCharArray()), ignorenumbers, filename, imagesServerPath, (string.IsNullOrEmpty(imginfo.OverrideName) ? imginfo.Name : imginfo.OverrideName), imginfo.Width, imginfo.Height, imginfo.Anchor, imginfo.ResizeMode, (imginfo.ImageFormat == "png" ? ImageFormat.Png : ImageFormat.Jpeg), imginfo.BaseImage, imginfo.Padding);
                    }
                }
                ViewBag.NewAdLink = string.Format("{1}shop/{0}/pending", prodID, Url.Content("~/"));
                //muimages.SaveUsingConfig(@"\content\products\", filename);
            }
            else
            {
                return AddListing2(catID);
            }
            return View();
        }
        public string GetImages(string[] filenames, string IgnoreNumbers)
        {

            string images = "";
            int i = 1;
            foreach (string filename in filenames)
            {
                if (string.IsNullOrEmpty(filename)) //to ignore the empty first one
                    continue;
                if (images.Length > 0)
                    images += ",";
                if (!string.IsNullOrEmpty(IgnoreNumbers))
                    while (IgnoreNumbers.Contains(i.ToString()))
                        i++;
                images += i;
                i++;
            }
            return images;
        }
        private void LoginUser(string email, string password)
        {
            MemberManager mMgr = new MemberManager(ConnectionString);
            SQLResult result;
            CMS.User user = mMgr.UserLogin(email, password, CurrentLanguage, out result);
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
    }
}