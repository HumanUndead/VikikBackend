using CMS;
using CMS.Config;
using CMS.Helpers;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vikik.Controllers
{
    public class ShopController : KensoftController
    {
        public int pageSize = 40;
        // GET: Shop

        public ActionResult SingleProduct(string id)
        {
            return View();
        }
        public ActionResult ShopCats(string id)
        {
            DirectoryManager mgr = new DirectoryManager(ConnectionString);
            DirectoryOutlet outlet = mgr.GetOutlet(id, true, CurrentLanguage);

            /* if (!string.IsNullOrEmpty(outlet.Keywords))
             {
                 lnkFB.HRef = outlet.Keywords;
             }
             else
                 lnkFB.Visible = false;
             if (!string.IsNullOrEmpty(outlet.SiteURL))
             {
                 lnkSt.HRef = outlet.SiteURL;
             }
             else
                 lnkSt.Visible = false;
                 */
            ProductManager pMgr = new ProductManager(ConnectionString);

            ViewBag.Categories = pMgr.GetUserCategoryCategories(outlet.OwnerID.Value, null, CurrentLanguage);
            //ViewBag.Title = string.Format(Resources.Vikik.GenericTitle, outlet.Name);
            return View(outlet);
        }
        public ActionResult AllShopCats(string catid, string name)
        {
            DirectoryManager mgr = new DirectoryManager(ConnectionString);
            // DirectoryOutlet outlet = mgr.GetOutlet(id, true, CurrentLanguage);

            ProductManager pMgr = new ProductManager(ConnectionString);
            ViewBag.Categories = mgr.GetOutletCategoryCatalogCategories(catid, "7", CurrentLanguage).FindAll(x => x.ProductCount > 0);
            ViewBag.Name = name;
            ViewBag.ID = catid;
            return View();
        }
        public ActionResult ShopCat(string cid)
        {
            //DirectoryManager mgr = new DirectoryManager(ConnectionString);
            //DirectoryOutlet outlet = mgr.GetOutlet(id, true, CurrentLanguage);

            //ProductManager pMgr = new ProductManager(ConnectionString);
            //int page = 1, pages = 1;
            //if (!string.IsNullOrEmpty(Request["p"]))
            //{
            //    page = Convert.ToInt32(Request["p"]);
            //}

            //string sortby = string.Empty;
            //bool asc = false;
            //ViewBag.Sort = Request["s"];
            //ViewBag.CID = cid;
            //if (!string.IsNullOrEmpty(Request["s"]))
            //{
            //    ViewBag.Sort = Request["s"];
            //    string[] sorting = Request["s"].Split((":").ToCharArray());
            //    sortby = sorting[0];
            //    asc = sorting[1] == "a";
            //    ViewBag.SortBy = sortby;
            //}
            //double? minprice = null;
            //double? maxprice = null;
            //if (!string.IsNullOrEmpty(Request["range"]))
            //{
            //    string range = Request["range"];
            //    string[] rangelimit = range.Replace(" - ", "-").Split(("-").ToCharArray());
            //    minprice = Convert.ToDouble(rangelimit[0]);
            //    maxprice = Convert.ToDouble(rangelimit[1]);
            //}
            //if (!string.IsNullOrEmpty(cid))
            //    ViewBag.Category = pMgr.GetCategory(cid, CurrentLanguage);
            //List<Product> products = pMgr.GetUserProducts(outlet.OwnerID.Value, cid, null, CurrentLanguage, page, 15, out pages, string.Empty, sortby, asc, null, null, null, minprice, maxprice);
            //ViewBag.CurrentPage = page;
            //ViewBag.Pages = pages;
            //ViewBag.Products = products;
            //ViewBag.OtherCategories = pMgr.GetUserCategoryCategories(outlet.OwnerID.Value, "7", CurrentLanguage);
            //ViewBag.SubCategories = pMgr.GetUserCategoryCategories(outlet.OwnerID.Value, cid, CurrentLanguage);
            //if (!string.IsNullOrEmpty(cid))
            //    ViewBag.Brands = pMgr.GetCategoryBrands(cid, CurrentLanguage).FindAll(x => x.ProductCount > 0);
            //return View(outlet);

            //string id = Request["catID"];
            double? min =null;
            double? max =null;
            string id = cid;
            string p = Request["p"];
            string s = Request["s"];
            //string sort = Request["sort"];
            int pageSize = 12;

            ProductManager mgr = new ProductManager(ConnectionString);
            ViewBag.ProductManager = mgr;
            int pCount = 0;


            //Page Pagination Start 
            int page = 1;
            if (!string.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }
            else
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Request["shownum"]))
            {
                pageSize = Convert.ToInt32(Request["shownum"]);
            }
      
            ViewBag.CID = id;

            string sortby = string.Empty;
            bool asc = false;
            ViewBag.Sort = Request["s"];
            ViewBag.CID = cid;
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                ViewBag.Sort = Request["s"];
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sortby;
            }

            ViewBag.pageSize = pageSize;
            ViewBag.CurrentPage = page;

            // ViewBag.Products = mgr.GetProducts(id, null, CurrentLanguage, page, pageSize, out pCount, null, null, sortby, null, null);

            // ViewBag.Products = mgr.GetProducts(id, null, CurrentLanguage, page, pageSize, out pCount, null, false, sortby, asc, null ,null,null,null);
            
            if(Request["minprice-filter"] != null) {
                try
                {
                    min = Convert.ToDouble(Request["minprice-filter"]);
                }
                catch (Exception e)
                {
                    min = 10;
                }
               
            }
           
            if (Request["maxprice-filter"] != null){
                try
                {
                    max = Convert.ToDouble(Request["maxprice-filter"]);
                }
                catch (Exception e)
                {
                    max = 100;
                }
              
            }
            
            ViewBag.Products = mgr.GetProducts(id, null, CurrentLanguage, page, pageSize, out pCount, null, false, sortby, asc, true, null, min, max);

            ViewBag.Pages = pCount;
            CMS.Category cat = mgr.GetCategory(id, CurrentLanguage);

            if (!string.IsNullOrEmpty(cat.ParentID))
            {
                ViewBag.ParentCat = mgr.GetCategory(cat.ParentID, CurrentLanguage);
            }
            //ltlCatDesc.Text = cat.Description;

            ViewBag.Description = cat.ShortDescription;
            ViewBag.Title = Resources.Strings.Client + " : " + cat.Name;
            ViewBag.SubCats = mgr.GetCategoryCategories(id, CurrentLanguage);
            


            return View(cat);
            
        }
        public ActionResult AllShopCat(string dircatid, string dircatname, string catid, string name)
        {
            DirectoryManager mgr = new DirectoryManager(ConnectionString);


            ProductManager pMgr = new ProductManager(ConnectionString);
            int page = 1, pages = 1;
            if (!string.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }

            string sortby = string.Empty;
            bool asc = false;
            ViewBag.Sort = Request["s"];
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                ViewBag.Sort = Request["s"];
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sortby;
            }
            double? minprice = null;
            double? maxprice = null;
            if (!string.IsNullOrEmpty(Request["range"]))
            {
                string range = Request["range"];
                string[] rangelimit = range.Replace(" - ", "-").Split(("-").ToCharArray());
                minprice = Convert.ToDouble(rangelimit[0]);
                maxprice = Convert.ToDouble(rangelimit[1]);
            }

            ViewBag.Category = pMgr.GetCategory(catid, CurrentLanguage);
            List<Product> products = pMgr.GetProducts(catid, null, CurrentLanguage, page, 15, out pages, string.Empty, true, sortby, asc, true, minprice, maxprice);
            ViewBag.CurrentPage = page;
            ViewBag.Pages = pages;
            ViewBag.Products = products;
            ViewBag.SubCategories = pMgr.GetCategoryCategories(catid, CurrentLanguage).FindAll(x => x.ProductCount > 0);
            ViewBag.Name = dircatname;
            ViewBag.DirCatID = dircatid;
            return View();
        }

        public ActionResult AllShopCategories(string cid)
       
        {
            //string id = Request["catID"];
            double? min = null;
            double? max = null;
            string id = cid;
            string p = Request["p"];
            string s = Request["s"];
            //string sort = Request["sort"];
            int pageSize = 12;

            ProductManager mgr = new ProductManager(ConnectionString);
            ViewBag.ProductManager = mgr;
            int pCount = 0;


            //Page Pagination Start 
            int page = 1;
            if (!string.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }
            else
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Request["shownum"]))
            {
                pageSize = Convert.ToInt32(Request["shownum"]);
            }

            ViewBag.CID = id;

            string sortby = string.Empty;
            bool asc = false;
            ViewBag.Sort = Request["s"];
            ViewBag.CID = cid;
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                ViewBag.Sort = Request["s"];
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sortby;
            }

            ViewBag.pageSize = pageSize;
            ViewBag.CurrentPage = page;

            // ViewBag.Products = mgr.GetProducts(id, null, CurrentLanguage, page, pageSize, out pCount, null, null, sortby, null, null);

            // ViewBag.Products = mgr.GetProducts(id, null, CurrentLanguage, page, pageSize, out pCount, null, false, sortby, asc, null ,null,null,null);

            if (!String.IsNullOrWhiteSpace(Request["minprice-filter"]))
            {
                try
                {
                    min = Convert.ToDouble(Request["minprice-filter"]);
                }
                catch (Exception e)
                {
                    min = 10;
                }

            }

            if (!String.IsNullOrWhiteSpace(Request["maxprice-filter"]))
            {
                try
                {
                    max = Convert.ToDouble(Request["maxprice-filter"]);
                }
                catch (Exception e)
                {
                    max = 100;
                }

            }

            ViewBag.Products = mgr.GetProducts((id == "70169" ? null : id), null, CurrentLanguage, page, pageSize, out pCount, null, true, sortby, asc, true, true, min, max);

            ViewBag.Pages = pCount;
            CMS.Category cat = mgr.GetCategory(id, CurrentLanguage);

            if (!string.IsNullOrEmpty(cat.ParentID))
            {
                ViewBag.ParentCat = mgr.GetCategory(cat.ParentID, CurrentLanguage);
            }
            //ltlCatDesc.Text = cat.Description; 70169

            ViewBag.Description = cat.ShortDescription;
            ViewBag.Title = Resources.Strings.Client + " : " + cat.Name;
            ViewBag.SubCats = mgr.GetCategoryCategories(null, CurrentLanguage);
            

            return View(cat);

        }
        public ActionResult Classifieds()
        {
            ProductManager pMgr = new ProductManager(ConnectionString);

            ViewBag.Categories = pMgr.GetCategoryCategories("20063", CurrentLanguage);

            return View("categories");
        }

        public ActionResult ClassifiedCategory(string id)
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            int dummy;

            Category category = pMgr.GetCategory(id, CurrentLanguage);
            int page = 1, pages = 1;
            if (!string.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }

            string sortby = string.Empty;
            bool asc = false;
            ViewBag.Sort = Request["s"];
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                ViewBag.Sort = Request["s"];
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sortby;
            }
            double? minprice = null;
            double? maxprice = null;
            if (!string.IsNullOrEmpty(Request["range"]))
            {
                string range = Request["range"];
                string[] rangelimit = range.Replace(" - ", "-").Split(("-").ToCharArray());
                minprice = Convert.ToDouble(rangelimit[0]);
                maxprice = Convert.ToDouble(rangelimit[1]);
            }
            List<Product> products = pMgr.GetProducts(id, null, CurrentLanguage, page, 15, out pages, string.Empty, true, sortby, asc, true, minprice, maxprice);
            ViewBag.Products = products;
            ViewBag.CurrentPage = page;
            ViewBag.Pages = pages;
            //ViewBag.OtherCategories = pMgr.GetUserCategoryCategories(outlet.OwnerID.Value, "7", CurrentLanguage);
            ViewBag.SubCategories = pMgr.GetCategoryCategories(id, CurrentLanguage).FindAll(x => x.ProductCount > 0);
            ViewBag.Brands = pMgr.GetCategoryBrands(id, CurrentLanguage).FindAll(x => x.ProductCount > 0);
            return View("classifieds", category);
        }

        public ActionResult Clearance(string sort, string cid)
        {
            ProductManager mgr = new ProductManager(ConnectionString);
            ViewBag.ProductManager = mgr;
            int page = 1, pages = 1;
            if (!string.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }

            string sortby = string.Empty;
            bool asc = false;
            ViewBag.Sort = Request["s"];
            ViewBag.CID = cid;
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                ViewBag.Sort = Request["s"];
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sortby;
            }
            double? minprice = null;
            double? maxprice = null;
            if (!string.IsNullOrEmpty(Request["range"]))
            {
                string range = Request["range"];
                string[] rangelimit = range.Replace(" - ", "-").Split(("-").ToCharArray());
                minprice = Convert.ToDouble(rangelimit[0]);
                maxprice = Convert.ToDouble(rangelimit[1]);
            }
            ViewBag.Products = mgr.GetProductsOnSale(cid, null, CurrentLanguage, page, 15, out pages, null, true, sortby, asc, minprice, maxprice);
            ViewBag.Pages = pages;
            ViewBag.CurrentPage = page;
            ViewBag.Description = Resources.Strings.Client + " : " + Resources.Strings.Clearance;
            ViewBag.Title = Resources.Strings.Client + " : " + Resources.Strings.Clearance;
            ViewBag.OtherCategories = mgr.GetCategoryCategories("7", CurrentLanguage).FindAll(x => x.ProductSaleCount > 0); ;
            if (!string.IsNullOrEmpty(cid))
            {
                ViewBag.Category = mgr.GetCategory(cid, CurrentLanguage);
                ViewBag.SubCategories = mgr.GetCategoryCategories(cid, CurrentLanguage);
            }

            return View();
        }

        public ActionResult MultiCatsProds(string cat1id, string cat2id, string name)
        {
            DirectoryManager mgr = new DirectoryManager(ConnectionString);


            ProductManager pMgr = new ProductManager(ConnectionString);

            Category cat = pMgr.GetCategory(cat1id, CurrentLanguage);
            Category parentcat = pMgr.GetCategory(cat.ParentID, CurrentLanguage);
            int page = 1, pages = 1;
            if (!string.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }

            string sortby = string.Empty;
            bool asc = false;
            ViewBag.Sort = Request["s"];
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                ViewBag.Sort = Request["s"];
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sortby;
            }
            double? minprice = null;
            double? maxprice = null;
            if (!string.IsNullOrEmpty(Request["range"]))
            {
                string range = Request["range"];
                string[] rangelimit = range.Replace(" - ", "-").Split(("-").ToCharArray());
                minprice = Convert.ToDouble(rangelimit[0]);
                maxprice = Convert.ToDouble(rangelimit[1]);
            }
            List<Product> products = pMgr.GetProductsFromMultiCats(null, CurrentLanguage, page, 15, out pages, string.Empty, true, sortby, asc, true, minprice, maxprice, cat1id, cat2id);
            ViewBag.CurrentPage = page;
            ViewBag.Pages = pages;
            ViewBag.Products = products;
            ViewBag.SubCategories = pMgr.GetCategoryCategories(parentcat.ParentID, CurrentLanguage).FindAll(x => x.ProductCount > 0);
            ViewBag.Name = name;
            ViewBag.Cat1ID = cat1id;
            ViewBag.Cat2ID = cat2id;
            return View();
        }
        public ActionResult Search(string sch, string sort)
        {
            ProductManager mgr = new ProductManager(ConnectionString);
            ViewBag.ProductManager = mgr;
            int pCount = 0;
            int page = 1;
            if (!string.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }

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
            ViewBag.Pages = pCount;
            ViewBag.Description = Resources.Strings.Search;
            ViewBag.Cat = cat;
            ViewBag.Title = Resources.Strings.Client + " : " + Resources.Strings.Search;

            return View();
        }
        [IsLoggedIn]
        public ActionResult AddShopListing()
        {
            ProductManager pMgr = new ProductManager(ConnectionString);
            List<Category> mainCats = pMgr.GetCategoryCategoriesLeaf(null, CurrentLanguage);
            ViewBag.MainCats = mainCats;
            /*List<Category> subCats = new List<Category>();
            foreach (Category cat in mainCats)
            {
                subCats.AddRange(pMgr.GetCategoryCategories(cat.ID, CurrentLanguage));
            }
            ViewBag.SubCats = subCats;
            */
            return View();
        }
        [IsLoggedIn]
        public ActionResult AddShopListing2(string catid)
        {
            ViewBag.CatID = catid;
            ProductManager pMgr = new ProductManager(ConnectionString);
            Category cat = pMgr.GetCategory(catid, CurrentLanguage);
            ViewBag.SelectedCategory = cat.Name;
            ViewBag.CategorySpecs = pMgr.GetCategorySpecs(cat.ID, CurrentLanguage, true).FindAll(x => x.ID != "13");
            ViewBag.Categories = pMgr.GetCategoryCategories(cat.ID, CurrentLanguage);
            ViewBag.UserLogged = CurrentUser != null;
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
            return View("addshoplisting2");
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddShopListing3(string id)
        {
            string catID = Request["catid"];
            CMS.Config.CMSConfigurations config = CMSConfigurations.GetSection();
            if (CurrentUser != null)
            {
                ProductManager mgr = new ProductManager(ConnectionString);

                string filename = string.Format("{0}\\{1}\\{2}\\{3}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, GeneralHelper.GenerateNewID());

                string title = Request["title"];
                string description = Request["description"];
                string price = Request["price"];
                string oldprice = Request["oldprice"];
                string hdnfiles = Request["hdnImages"];
                string ignorenumbers = Request["hdnoldimages"];
                string files = ignorenumbers + (string.IsNullOrEmpty(ignorenumbers) || string.IsNullOrEmpty(hdnfiles) ? "" : ",") + hdnfiles;
                if (!string.IsNullOrEmpty(id))
                {
                    Product prod = mgr.GetProduct(id, CurrentLanguage);
                    if (prod.OwnerID != CurrentUser.ID && prod.OwnerID != (Guid)HttpContext.Application["AdminID"])
                    {
                        Response.Redirect("error");
                    }
                    filename = !string.IsNullOrEmpty(prod.ThumbURL) ? prod.ThumbURL : filename;
                }
                string prodID = mgr.SaveProduct(CurrentUser.ID, id, Request["subcat"], title, description.Replace("\n", "<br/>"), Convert.ToDouble(price), string.IsNullOrEmpty(oldprice) ? null : (double?)Convert.ToDouble(oldprice), filename, GetImages(files.Split((",").ToCharArray()), string.Empty), false);//muimages.Images
                //now save the specs based on category
                List<Spec> specs = new List<Spec>();
                Category cat = mgr.GetCategory(catID, CurrentLanguage);
                foreach (Spec spec in mgr.GetCategorySpecs(cat.ID, CurrentLanguage, true))
                {
                    switch (spec.Type)
                    {
                        case SpecType.Text:
                        case SpecType.Numeric:
                        case SpecType.MultiLine:
                            spec.Value = Request["spec" + spec.ID];
                            specs.Add(spec);
                            break;
                        case SpecType.AdminSelection:
                            int specsource = Convert.ToInt32(spec.Source);
                            spec.Value = Request["spec" + spec.ID];
                            specs.Add(spec);
                            break;
                        case SpecType.Available:
                            spec.Value = Request["spec" + spec.ID];
                            specs.Add(spec);
                            break;
                    }
                }
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
                // string body = string.Format(Resources.Strings.EmailWelcomeBody, System.Configuration.ConfigurationManager.AppSettings["SiteURL"], HttpUtility.UrlEncode(resetID.ToString()), HttpUtility.UrlEncode(email));
                // Helper.SendEmail(System.Configuration.ConfigurationManager.AppSettings["FromEmail"], CurrentUser.Email, Resources.Strings.EmailAdPending, body);
                //muimages.SaveUsingConfig(@"\content\products\", filename);
            }
            return View();
        }
        public ActionResult Categories(string id=null)
        {
            //ProductManager mgr = new ProductManager(ConnectionString);
            //Category cat = mgr.GetCategory(id, CurrentLanguage);
            //List<Category> cats = mgr.GetCategoryCategories(id, CurrentLanguage);
            //ViewBag.Title = cat.Name;
            //ViewBag.Description = cat.Description;
            //ViewBag.Category = mgr.GetCategory(id, CurrentLanguage);
            //ViewBag.Path = "shop";
            //return View(cats);

            ProductManager mgr = new ProductManager(ConnectionString);
            List<Category> cats = mgr.GetCategoryCategories(null, CurrentLanguage);
            ViewBag.Title = Resources.Strings.project_name+" | "+ Resources.Strings.Categories;
            ViewBag.Description = Resources.Strings.Description_Page;
            return View(cats);
            
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

        [HttpGet]
        public ActionResult Category(string id, int page = 1,string brand=null)
        {
            int pages = 1, outletscount, pageSize = 12;
            double? min=null, max=null;
            ProductManager pmgr = new ProductManager(ConnectionString);
            StaticPageManager mgr = new StaticPageManager(ConnectionString);
            ViewBag.CurrentBrand = brand;
            string sortby = string.Empty;
            bool asc = false;
            ViewBag.Sort = Request["s"];
            int cID = -1;
            Category cat = new Category(); 
            if (!string.IsNullOrEmpty(id))
                cat = pmgr.GetCategory(id, CurrentLanguage);
          
            if (!string.IsNullOrEmpty(Request["category"]))
            {

                int.TryParse(Request["category"], out cID);
                if (cID != 0)
                {
                    ViewBag.SubID = cID.ToString();
                }

            }
            else if (!string.IsNullOrEmpty(Request["categoryID"]))
            {
                int.TryParse(Request["categoryID"], out cID);
                if (cID != 0)
                {
                    ViewBag.SubID = cID.ToString();
                }
            }

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                id = Request["id"];

                if (string.IsNullOrEmpty(Request["category"]) && string.IsNullOrEmpty(Request["categoryID"]))
                {
                    int.TryParse(Request["id"], out cID);
                }
                ViewBag.CID = id;
            }
            else if(!string.IsNullOrEmpty(id))
            {
                ViewBag.CID = id;
            }
            ViewBag.CurrentSub = cID.ToString();

            if (!string.IsNullOrEmpty(Request["s"]))
            {
                ViewBag.Sort = Request["s"];
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sortby;
            }

            if (!string.IsNullOrEmpty(Request["price"]))
            {
                ViewBag.PirceFilter = Request["price"];
                string filter = Request["price"];
                min = string.IsNullOrEmpty(filter.Split('-')[0]) ? (double?)null : double.Parse(filter.Split('-')[0]);
                max = string.IsNullOrEmpty(filter.Split('-')[1]) ? (double?)null : double.Parse(filter.Split('-')[1]);

            }
            else
            {
                ViewBag.PirceFilter = "";
             
                min =  (double?)null ;
                max =  (double?)null ;
            }


            StaticPage Spage = mgr.GetStaticPage(-1, CurrentLanguage);
            ViewBag.Title = cat.Name;
            ViewBag.Description = cat.ShortDescription;

            
            ViewBag.Products = pmgr.GetProducts( cID.ToString() == "-1"? id : cID.ToString(), brand == "-1" ? null : brand, CurrentLanguage, page, pageSize, out pages, null, true, sortby, asc, true,min, max);
           
          
            if (!string.IsNullOrEmpty(cat.ParentID))
            {
                ViewBag.ParentCategory = pmgr.GetCategory(cat.ParentID, CurrentLanguage);
                if(!string.IsNullOrEmpty(ViewBag.ParentCategory.ParentID) && ViewBag.ParentCategory.ParentID == "60043" && ViewBag.ParentCategory.ParentID == "70136")
                {
                   ViewBag.TopCategory = pmgr.GetCategory(ViewBag.ParentCategory.ParentID, CurrentLanguage);
                }
                
                ViewBag.SubCategories = pmgr.GetCategoryCategories(id, CurrentLanguage);
            }
         
            int dummy;
            if (CurrentUser != null)
            {
                ViewBag.CurrentUser = CurrentUser;
                ViewBag.UserWisthList = pmgr.ProductsGetUserWishlist(CurrentUser.ID, CurrentLanguage, 1, int.MaxValue, out dummy, 1);
            }
            ViewBag.CurrentPage = page;
            ViewBag.Pages = pages;
            if(!string.IsNullOrEmpty(id))
            ViewBag.Brands = pmgr.GetCategoryBrands(id, CurrentLanguage);
            
            return View(cat);
        }
        public ActionResult allshops()
        {
            string catID = "7";
            if (!string.IsNullOrEmpty(Request["cid"]))
            {
                catID = Request["cid"];
            }
            int pageCount;
            ProductManager mgr = new ProductManager(ConnectionString);
            ViewBag.Allshops = mgr.ShopGetCategoryProducts("7", CurrentLanguage, 1, 15, out pageCount, null);
            ViewBag.Title = string.Format(Resources.Strings.DefaultTitle);



            StaticPageManager pmgr = new StaticPageManager(ConnectionString);
            StaticPage page = pmgr.GetStaticPage(-1, CurrentLanguage);
            ViewBag.Title = page.Name;
            ViewBag.Description = page.Text;
            ViewBag.page = page;

            DirectoryManager Dmgr = new DirectoryManager(ConnectionString);
            ProductManager pMgr = new ProductManager(ConnectionString);
            DirectoryOutlet outlet;
            if ((!string.IsNullOrEmpty(Request["id"]) || !string.IsNullOrEmpty(Request["sid"])) && Request["sid"] != "-1")
            {
                if (!string.IsNullOrEmpty(Request["sid"]))
                {
                    outlet = Dmgr.GetOutlet(Request["sid"], true, CurrentLanguage);
                }
                else
                {
                    Product prod = pMgr.GetProduct(Request["id"], CurrentLanguage);
                    outlet = Dmgr.GetOutletByOwner(prod.OwnerID, CurrentLanguage);
                }

                ViewBag.Shopmenu = pMgr.GetUserCategoryCategories(outlet.OwnerID.Value, "7", CurrentLanguage);
            }
            else
            {
                List<Category> cats = pMgr.GetCategoryCategories(!string.IsNullOrEmpty(Request["cid"]) ? Request["cid"] : "7", CurrentLanguage);
                ViewBag.Shopmenu = cats.FindAll(x => x.ProductCount > 0);
            }






            return View();

        }

    }
}