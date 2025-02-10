using CMS;
using CMS.Config;
using CMS.Helpers;
using CMS.Managers;
using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace vikik.Controllers
{
    [IsLoggedIn]
    public class UserController : KensoftController
    {
        public ActionResult Logout()
        {
            ViewBag.LoggingOut = true;
            Session["User"] = null;
            ViewBag.CurrentUser = null; ;
            FormsAuthentication.SignOut();
            HttpCookie cookie = new HttpCookie("User");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            Response.Redirect("~/index");
            return View("index");
        }

        public ActionResult UserShop()
        {
            int dummy;
            ProductManager mgr = new ProductManager(ConnectionString);
            DirectoryManager dmgr = new DirectoryManager(ConnectionString);
            ViewBag.UserShop = dmgr.GetOutletByOwner(CurrentUser.ID, CurrentLanguage);
            FillSEO(-23);
            ViewBag.Products = mgr.GetUserProducts(CurrentUser.ID, null, Request["bid"], CurrentLanguage, 1, int.MaxValue, out dummy, string.Empty, null, false, null, null, null).FindAll(x => x.CategoryID != "70166");
            var shopCats = mgr.GetCategories(CurrentLanguage);
            var userCats = mgr.GetCategoryCategories("70166", CurrentLanguage);
            ViewBag.ShopCategories = shopCats.FindAll(x => userCats.Find(y => y.ID == x.ID) == null || x.ID != userCats.Find(y => y.ID == x.ID).ID);

            return View();
        }

        public ActionResult AddShop()
        {

            DirectoryManager dmgr = new DirectoryManager(ConnectionString);
            LanguageManager lng = new LanguageManager(ConnectionString);
            ViewBag.Title = Resources.Strings.Client + " | " + Resources.Strings.Add + " " + Resources.Strings.Shop;
            var langs = lng.GetAllLanguages();

            DirectoryOutlet directory = dmgr.GetOutletByOwner(CurrentUser.ID, CurrentLanguage);

            Dictionary<string, DirectoryOutlet> langItems = new Dictionary<string, DirectoryOutlet>();

            if (directory != null && Convert.ToInt32(directory.ID) > 1)
            {
                foreach (var item in langs)
                {
                    var outlet = dmgr.GetOutletByOwner(CurrentUser.ID, item);
                    if (outlet != null && Convert.ToInt32(outlet.ID) > 1)
                    {
                        langItems.Add(item.Prefix, outlet);
                    }
                }

            }
            ViewBag.UserShop = dmgr.GetOutletByOwner(CurrentUser.ID, CurrentLanguage);
            ViewBag.LangItems = langItems;
            ViewBag.Langs = langs;
            return View();
        }


        [HttpPost]
        public ActionResult DoAddShop()
        {

            DirectoryManager dmgr = new DirectoryManager(ConnectionString);
            LanguageManager lng = new LanguageManager(ConnectionString);

            var langs = lng.GetAllLanguages();
            string pID = Request["shopID"];
            string email = Request["email"];
            string location = Request["lat"] + "," + Request["lng"];
            string street = Request["address1"];
            DirectoryOutlet directory = dmgr.GetOutletByOwner(CurrentUser.ID, CurrentLanguage);
            if (directory.Categories == null)
                directory.Categories = new List<DirectoryCategory>();
            if (directory != null && Convert.ToInt32(directory.ID) >= 1)
                pID = dmgr.SaveOutletBasicInfo(pID, CurrentUser.ID, directory.ThumbURL, directory.Images, directory.Attachment, email, directory.Facebook, directory.Linkdin, location, directory.HasVISA, directory.HasMaster, directory.HasAMEX, directory.HasNational, directory.HasPayPal, directory.HasPayPal, directory.Approved, directory.Categories, directory.MenuID, directory.CatalogID, directory.PhotosID, directory.Youtube, directory.Featured);
            else
                pID = dmgr.SaveOutletBasicInfo(pID, CurrentUser.ID, null, null, null, email, null, street, location, false, false, false, false, false, false, false, new List<DirectoryCategory>(), null, null, null, null, false);


            foreach (var item in langs)
            {
                directory = dmgr.GetOutletByOwner(CurrentUser.ID, item);
                DirectoryOutlet englishDirectory = dmgr.GetOutletByOwner(CurrentUser.ID, new Language() { ID = "1" });
                string name = Request["name" + item.Prefix];
                string description = Request["description" + item.Prefix];

                dmgr.SaveOutlet(pID, string.IsNullOrEmpty(name) ? englishDirectory.Name : name, string.IsNullOrEmpty(description) ? englishDirectory.Description : description, directory.Keywords, directory.HTMLDescription, directory.Twitter, directory.Instagram, directory.GooglePlus, directory.Brands, item);
            }

            return Redirect("~/user/UserShop");
        }

        public ActionResult AddProduct(string id)
        {
            ViewBag.Title = Resources.Strings.Client + " | " + (string.IsNullOrEmpty(id) ? Resources.Strings.Add : Resources.Strings.Edit) + " " + Resources.Strings.Product;
            ProductManager pmgr = new ProductManager(ConnectionString);
            GeneralManager gmgr = new GeneralManager(ConnectionString);
            LanguageManager lmgr = new LanguageManager(ConnectionString);
            var langs = lmgr.GetAllLanguages();
            ViewBag.Langs = langs;
            Product product = pmgr.GetProduct(id, CurrentLanguage);
            if (string.IsNullOrEmpty(product.ID))
                product = null;
            ViewBag.SpecList = pmgr.GetCategorySpecs("60043", CurrentLanguage);
            ViewBag.Product = product;
            ViewBag.Editable = product != null && product.OwnerID == CurrentUser.ID;
            //ViewBag.SelectCategories = pmgr.GetCategoryCategoriesLeaf("60043", CurrentLanguage);
            ViewBag.AddShopCategories = pmgr.GetCategories(CurrentLanguage);
            LookupManager lck = new LookupManager(ConnectionString);
            //optimization point - need fix for spped
            ViewBag.LookUpCats = lck.GetLookupCategories("1013", CurrentLanguage);
            ViewBag.LookUpItems = lck.GetLookUpCategoryItems(1013, CurrentLanguage);
            if (!string.IsNullOrEmpty(id) && product.OwnerID == CurrentUser.ID)
            {
                Dictionary<string, Product> langItems = new Dictionary<string, Product>();
                foreach (var item in langs)
                {
                    Product prod = pmgr.GetProduct(id, item);
                    if (prod != null)
                        langItems.Add(item.Prefix, prod);
                }
                ViewBag.LangItems = langItems;
                var prodGroup = pmgr.GetProduct(id, CurrentLanguage);
                if (prodGroup.GroupID != null)
                    ViewBag.ProductGrouped = pmgr.ProductsGetGrouped((int)prodGroup.GroupID, CurrentLanguage, false);
            }
            if (product != null && product.OwnerID != CurrentUser.ID)
                return RedirectToAction("ProductDetails", "Catalog", new { dummy = "product", id });
            ViewBag.ShopBrands = pmgr.GetBrands(CurrentLanguage);
            ViewBag.RRPFactor = Convert.ToDouble(ConfigurationManager.AppSettings["RRPFactor"]);
            ViewBag.Sizes = gmgr.ItemSizesGet();
            ViewBag.LCK = lck;
            ViewBag.Lang = CurrentLanguage;
            return View();
        }

        public ActionResult UserBought()
        {
            ViewBag.Title = Resources.Strings.Client + " | " + "What I Bought";
            ProductManager pmgr = new ProductManager(ConnectionString);
            int page = Convert.ToInt32(Request["p"]) < 1 ? 1 : Convert.ToInt32(Request["p"]);
            List<Tuple<Product, int>> prods = pmgr.UserBoughtProducts(CurrentUser.ID, page, 10, out int count, CurrentLanguage);
            prods.AddRange(pmgr.UserRelistingProductsGet(CurrentUser.ID, 1, int.MaxValue, out count, CurrentLanguage,false));
            ViewBag.UserProducts = prods;
            ViewBag.CurrentPage = page;
            ViewBag.ShopCategories = pmgr.GetCategories(CurrentLanguage);
            ViewBag.Pages = count;
            return View();
        }

        [HttpPost]
        public ActionResult DeactivateProduct()
        {
            string rt = Request["rt"];
            ProductManager pmgr = new ProductManager(ConnectionString);
            string id = Request["id"];
            var prod = pmgr.GetProduct(id, CurrentLanguage);
            pmgr.SaveProductBasicInfo(prod, !prod.Approved, true);
            pmgr.CategoryUpdateProductCount(null);
            return Redirect("~/" + rt);
        }


        [HttpPost]
        public ActionResult SoldProduct()
        {
            string rt = Request["rt"];
            ProductManager pmgr = new ProductManager(ConnectionString);
            string id = Request["id"];
            var prod = pmgr.GetProduct(id, CurrentLanguage);
            prod.Quantity = 0;
            pmgr.SaveProductBasicInfo(prod, false, true);
            pmgr.CategoryUpdateProductCount(null);

            CommerceManager cmgr = new CommerceManager(ConnectionString);
            pmgr.UserRelistingSold(CurrentUser.ID,Convert.ToInt32(prod.ID),true);
             
                
            return Redirect("~/" + rt);

        }

        public ActionResult DeleteSubImage()
        {
            string productID = Request["prodID"];
            string subImage = Request["img"];
            ProductManager pmgr = new ProductManager(ConnectionString);
            Product prod = pmgr.GetProduct(productID, CurrentLanguage);
            if (prod.ImageList.ToList().Find(x => x == subImage) != null)
            {


                string[] imgs = prod.Images.Split(',');
                imgs = imgs.Where(x => x != subImage).ToArray();
                prod.Images = string.Join(",", imgs);
                pmgr.SaveProductBasicInfo(prod);
                string imageName = Path.GetFileNameWithoutExtension(prod.ThumbURL);
                string imgPath = Directory.GetParent(Server.MapPath("~/" + prod.FullImagePath)).FullName;

                if (Directory.Exists(imgPath))
                {
                    foreach (var file in Directory.GetFiles(imgPath).Where(x => x.Contains(imageName + subImage)))
                    {
                        System.IO.File.Delete(Path.Combine(imgPath, file));
                    }
                }

            }

            return Redirect("~/user/AddProduct?id=" + productID);
        }

        [HttpPost]
        public ActionResult ProductDoAdd(HttpPostedFileBase image, HttpPostedFileBase[] images)
        {
            ProductManager pmgr = new ProductManager(ConnectionString);
            LanguageManager lmgr = new LanguageManager(ConnectionString);
            GeneralManager gmgr = new GeneralManager(ConnectionString);
            var langs = lmgr.GetAllLanguages();
            string pID = Request["id"];
            bool newProduct = string.IsNullOrEmpty(pID);
            bool newBrand = false;
            Product produt = pmgr.GetProduct(pID, CurrentLanguage);

            string thumb = produt != null && !string.IsNullOrEmpty(produt.ThumbURL) ? produt.ThumbURL : null;

            //save some of the admin setting
            produt = produt == null ? new Product() : produt;
            CommerceManager cmgr = new CommerceManager(ConnectionString);
            produt.Price = Convert.ToDouble(Request["price"]);
            produt.OldPrice = string.IsNullOrEmpty(Request["oldPrice"]) ? (double?)null : Convert.ToDouble(Request["oldPrice"]);
            produt.Quantity = Convert.ToInt32(Request["defaultSizeQty"]);
            produt.Size = new ItemSize();
            produt.Size.ID = Convert.ToInt32(Request["defaultSize"]);
            produt.Sellable = true;
            produt.Recurrent = false;
            produt.OverrideShipping = string.IsNullOrEmpty(Request["overrideShipping"]) ? (double?)null : Convert.ToDouble(Request["overrideShipping"]);
            produt.OwnerID = CurrentUser.ID;
            produt.ShopItem = true;
            produt.Searchable = true;
            produt.Barcode = Request["defaultSKU"];
            string catID = Request["mainCatID"];
            if (!string.IsNullOrEmpty(Request["catID"]))
                catID = Request["catID"];
            if (!string.IsNullOrEmpty(Request["subCatID"]))
                catID = Request["subCatID"];
            if (string.IsNullOrEmpty(produt.CategoryID))
                produt.CategoryID = catID;
            string brandID = null;
            if (!string.IsNullOrEmpty(Request["brandID"]))
            {
                brandID = Request["brandID"];
                if (brandID == "-1")
                {
                    brandID = pmgr.SaveBrandBasicInfo(null, null, 100);
                    newBrand = true;
                }
                produt.BrandID = brandID;
            }

            //save product basic info
            pID = pmgr.SaveProductBasicInfo(produt, newProduct ? false : produt.Approved, false);
            cmgr.SaveSellableProduct(pID, (double)produt.Price, false, 0, Period.Days);


            //uploading the image/s

            string ImagesPath = "~/content/products/" + pID + "/";
            if (!Directory.Exists(Server.MapPath(ImagesPath).ToLower()))
                Directory.CreateDirectory(Server.MapPath(ImagesPath).ToLower());
            string imagesServerPath = Server.MapPath(ImagesPath).ToLower();

            if (image != null)
            {

                //Checking file is available to save.  
                if (image != null && image.ContentLength > 0)
                {

                    string filename = pID;

                    string fileExtension = Path.GetExtension(image.FileName);
                    string filePath = imagesServerPath + pID + fileExtension;

                    image.SaveAs(filePath);

                    produt.ThumbURL = pID + "\\" + filename;
                    SaveImageSizes(imagesServerPath, filename, filePath);
                    //File.Delete(filePath);
                }
            }

            if (images != null && images.Length > 0 && images[0] != null)
            {
                string basePath = produt != null ? Server.MapPath("~/content/products/" + Path.GetDirectoryName(produt.ThumbURL) + "\\") : imagesServerPath;
                int startName = 1;
                if (produt != null && !string.IsNullOrEmpty(produt.Images))
                    startName = FindImageName(produt.Images);
                string imgs = "";
                foreach (var img in images)
                {
                    if (!string.IsNullOrEmpty(imgs))
                        imgs += ",";

                    string fileExtension = Path.GetExtension(img.FileName);
                    string filePath = basePath + pID + startName + fileExtension;
                    string filename = pID + startName;

                    imgs += startName;

                    img.SaveAs(filePath);
                    SaveImageSizes(basePath, filename, filePath);
                    startName += 1;
                }
                if (produt != null && string.IsNullOrEmpty(produt.Images))
                    produt.Images += imgs;
                else
                    produt.Images += "," + imgs;

            }

            //handling different sizes
            produt.ID = pID;
            int productGroup = -1;
            if (produt.GroupID == null)
            {
                productGroup = gmgr.ItemGroupSave(produt.GroupID, produt.Name + " " + produt.ID);
                pmgr.ProductGroupSave(produt.ID, productGroup.ToString());
            }
            else
            {
                productGroup = (int)produt.GroupID;
            }

            //save images and product group
            pID = pmgr.SaveProductBasicInfo(produt, newProduct ? false : produt.Approved, false);

            //saving the specs
            List<Spec> SpecList = pmgr.GetCategorySpecs("60043", CurrentLanguage);
            if (!string.IsNullOrEmpty(Request["sp30041"]))
            {
                var newList = pmgr.GetCategorySpecs("70166", CurrentLanguage);
                if (newList != null)
                    SpecList.AddRange(newList);
            }
            foreach (var spec in SpecList)
            {
                if (spec.ID != "20039")
                {
                    if (spec.Type == SpecType.OnOff)
                    {
                        spec.Value = Request["sp" + spec.ID] == "on" ? "1" : "0";
                    }
                    else
                    {
                        spec.Value = Request["sp" + spec.ID];
                    }

                }
                else
                {
                    string currentValue = Request["sp20039"];
                    spec.Value = currentValue;
                }
            }
            pmgr.SaveProductSpecs(pID, SpecList, CurrentLanguage);

            //saving the translation info
            foreach (var item in langs)
            {
                pmgr.SaveProductInfo(pID, Request["name" + item.Prefix], Request["description" + item.Prefix], item.ID);
                if (!string.IsNullOrEmpty(Request["brandID"]) && newBrand)
                {

                    Brand brand = new Brand();
                    brand.ID = brandID;
                    brand.Name = Request["newBrand"];
                    brand.Lang = item;
                    pmgr.SaveBrand(brand);
                }
            }
            Product product = pmgr.GetProduct(pID, CurrentLanguage);

            //updating cats count
            pmgr.CategoryUpdateProductCount(null);

            //updating old sizes
            var oldSizes = pmgr.ProductsGetGrouped(productGroup, CurrentLanguage, false);
            foreach (var item in oldSizes)
            {
                item.Barcode = Request["SizeSKU" + item.ID];
                item.Quantity = Convert.ToInt32(Request["SizeQty" + item.ID]);
                item.Searchable = false;
                product.Size = new ItemSize() { ID = Convert.ToInt32(Request["Size" + item.ID]) };
                string pgID = pmgr.SaveProductBasicInfo(item, true, true);


            }

            //adding new sizes
            string sizesToAdd = Request["sizesToAdd"];
            if (!string.IsNullOrEmpty(sizesToAdd) && sizesToAdd.Split(',').Length > 0)
            {
                List<Product> productSies = new List<Product>();
                if (produt.GroupID.HasValue)
                    productSies = pmgr.ProductsGetGrouped((int)produt.GroupID, CurrentLanguage, false);
                foreach (var size in sizesToAdd.Split(','))
                {
                    product.Size = new ItemSize() { ID = Convert.ToInt32(Request["Size" + size]) };
                    product.Searchable = false;
                    product.Barcode = Request["SizeSKU" + size];
                    product.ID = productSies.Find(x => x.Size != null && x.Size.ID == product.Size.ID) != null ? productSies.Find(x => x.Size != null && x.Size.ID == product.Size.ID).ID : null;
                    product.Quantity = Convert.ToInt32(Request["SizeQty" + size]);
                    string pgID = pmgr.SaveProductBasicInfo(product, true, true);
                    pmgr.ProductGroupSave(pgID, productGroup.ToString());
                    foreach (var item in langs)
                    {
                        var langItem = pmgr.GetProduct(pID, item);
                        pmgr.SaveProductInfo(pgID, langItem.Name, langItem.Description, item.ID);
                    }
                    cmgr.SaveSellableProduct(pgID, (double)product.Price, false, 0, Period.Days);

                    CopyImages(product.ThumbURL, pgID + "/" + pgID);
                }
            }

            //informing the admin
            Helper.SendEmail("site@vikikfashion.com", "admin@vikikfashion.com", "Product Pending Approved", string.Format("You have a new product pending for approval <br><br> Product Name: {0}, <br>Product ID: {1}", product.Name, product.ID));
            if (UserController.IsMerchent(CurrentUser))
                return RedirectToAction("UserShop");
            else
                return RedirectToAction("UserBought");
        }

        void CopyImages(string oldPath, string newPath, string basePath = "~/content/products/")
        {
            // newpath to be newid
            string imagesServerPath = Server.MapPath(basePath + newPath).ToLower();
            if (!Directory.Exists(imagesServerPath))
                Directory.CreateDirectory(imagesServerPath);
            if (Directory.Exists(imagesServerPath))
            {

                foreach (var img in Directory.GetFiles(Server.MapPath(basePath + "/" + Path.GetDirectoryName(oldPath))))
                {
                    string newName = Path.GetFileNameWithoutExtension(img).Contains("_") ? Path.GetFileNameWithoutExtension(newPath) + "_" + Path.GetFileNameWithoutExtension(img).Split('_')[1] : Path.GetFileName(newPath);
                    System.IO.File.Copy(img, imagesServerPath + "/" + newName + Path.GetExtension(img));
                }
            }
        }

        public ActionResult ProductDistribution()
        {
            string productID = Request["id"];
            ViewBag.Title = Resources.Strings.Client + " | " + "Re-distribute";

            ProductManager pmgr = new ProductManager(ConnectionString);
            //var userProducts = pmgr.GetUserProducts(CurrentUser.ID, Request["cid"], Request["bid"], CurrentLanguage, 1, 100, out int dummy, string.Empty, null, false, null, null, null);
            var userProducts = pmgr.GetProductsWithSpecs(null, null, CurrentLanguage, 1, int.MaxValue, out int dummy, null, "updatedate", true, null, CurrentUser.ID, null, null, null, null, null, null);
            var currentProduct = userProducts.Find(x => x.ID == productID);
            ViewBag.ProdutID = currentProduct != null ? currentProduct.ID : null;
            ViewBag.BaseID = productID;
            ViewBag.OrderID = Request["orderID"];
            if (!string.IsNullOrEmpty(ViewBag.ProdutID) && ViewBag.ProdutID != productID)
                ViewBag.Product = pmgr.GetProduct(ViewBag.ProdutID, CurrentLanguage);
            else
                ViewBag.Product = pmgr.GetProduct(productID, CurrentLanguage);
            var ShopCategories = pmgr.GetCategoryCategories("70166", CurrentLanguage);
            ShopCategories.Add(pmgr.GetCategory("70166", CurrentLanguage));
            ViewBag.ShopCategories = ShopCategories;
            LanguageManager lmgr = new LanguageManager(ConnectionString);
            var langs = lmgr.GetAllLanguages();
            var SpecList = pmgr.GetCategorySpecs("60043", CurrentLanguage);
            SpecList.AddRange(pmgr.GetCategorySpecs("70166", CurrentLanguage));
            ViewBag.SpecList = SpecList;
            ViewBag.Langs = langs;

            LookupManager lck = new LookupManager(ConnectionString);
            //optimization point - need fix for spped
            var LookUpCats = lck.GetLookupCategories("1013", CurrentLanguage);
            var LookUpItems = lck.GetLookUpCategoryItems(1013, CurrentLanguage);
            LookUpCats.AddRange(lck.GetLookupCategories("4013", CurrentLanguage));
            LookUpItems.AddRange(lck.GetLookUpCategoryItems(4013, CurrentLanguage));

            ViewBag.LookUpCats = LookUpCats;
            ViewBag.LookUpItems = LookUpItems;

            if (!string.IsNullOrEmpty(productID))
            {
                Dictionary<string, Product> langItems = new Dictionary<string, Product>();
                foreach (var item in langs)
                {
                    Product prod = pmgr.GetProduct(productID, item);
                    if (prod != null)
                        langItems.Add(item.Prefix, prod);
                }
                ViewBag.LangItems = langItems;
            }

            ViewBag.ShopBrands = pmgr.GetBrands(CurrentLanguage);

            return View();
        }
        [HttpPost]
        public ActionResult RedistributionDoAdd(HttpPostedFileBase[] images)
        {
            string pID = Request["id"];
            int OrderID = Convert.ToInt32(Request["OrderID"]);
            if (string.IsNullOrEmpty(pID))
            {
                pID = Request["sp30041"];
            }
            bool newProduct = string.IsNullOrEmpty(pID) || Request["isNew"] == "on";

            ProductManager pmgr = new ProductManager(ConnectionString);
            LanguageManager lgr = new LanguageManager(ConnectionString);
            var langs = lgr.GetAllLanguages();
            Product produt = pmgr.GetProduct(pID, CurrentLanguage);
            produt.ID = newProduct ? null : produt.ID;
            produt.Price = Convert.ToDouble(Request["price"]);
            produt.OldPrice = string.IsNullOrEmpty(Request["oldPrice"]) ? (double?)null : Convert.ToDouble(Request["oldPrice"]);
            produt.Quantity = Convert.ToInt32(Request["quantity"]);

            produt.Sellable = true;
            produt.Recurrent = false;
            produt.OverrideShipping = string.IsNullOrEmpty(Request["overrideShipping"]) ? (double?)null : Convert.ToDouble(Request["overrideShipping"]);
            produt.OwnerID = CurrentUser.ID;
            produt.ShopItem = true;
            produt.Searchable = true;
            produt.Barcode = Request["SKU"];
            string catID = Request["mainCatID"];
            if (!string.IsNullOrEmpty(Request["catID"]))
                catID = Request["catID"];
            if (!string.IsNullOrEmpty(Request["subCatID"]))
                catID = Request["subCatID"];
            
            string brandID = null;
            if (!string.IsNullOrEmpty(Request["brandID"]))
            {
                brandID = Request["brandID"];
                produt.BrandID = brandID;
            }
            produt.CategoryID = catID;
            List<Spec> SpecList = produt.Specs;
            pID = pmgr.SaveProductBasicInfo(produt, newProduct ? false : produt.Approved, false);

            foreach (var item in langs)
            {
                pmgr.SaveProductInfo(pID, Request["name" + item.Prefix], Request["description" + item.Prefix], item.ID);
            }

            if (newProduct)
            {
                CopyImages(produt.ThumbURL, pID);
                if (!string.IsNullOrEmpty(Request["sp30041"]))
                {
                    Spec spec = new Spec();
                    spec.Value = Request["sp30041"];
                    spec.ID = "30041";
                    SpecList.Add(spec);
                }
                pmgr.UserRelistingProductsSave(CurrentUser.ID, OrderID, Convert.ToInt32(pID), Convert.ToInt32(Request["sp30041"]));

            }

            if (images != null && images.Length > 0 && images[0] != null)
            {
                string basePath = Server.MapPath("~/content/products/" + pID + "/") ;
                int startName = 1;
                if (produt != null && !string.IsNullOrEmpty(produt.Images))
                    startName = FindImageName(produt.Images);
                string imgs = "";
                foreach (var img in images)
                {
                    if (!string.IsNullOrEmpty(imgs))
                        imgs += ",";

                    string fileExtension = Path.GetExtension(img.FileName);
                    string filePath = basePath + pID + startName + fileExtension;
                    string filename = pID + startName;

                    imgs += startName;

                    img.SaveAs(filePath);
                    SaveImageSizes(basePath, filename, filePath);
                    startName += 1;
                }
                if (produt != null && string.IsNullOrEmpty(produt.Images))
                    produt.Images += imgs;
                else
                    produt.Images += "," + imgs;

            }


            pmgr.SaveProductSpecs(pID, produt.Specs, CurrentLanguage);
            produt.Searchable = true;
            produt.ID = pID;
            pID = pmgr.SaveProductBasicInfo(produt, newProduct ? false : produt.Approved, false);
            pmgr.CategoryUpdateProductCount(null);

            return RedirectToAction("UserBought");
        }

        private void uploadProdcutImage(string BaseID, HttpPostedFileBase Image, HttpPostedFileBase[] Images, ref Product produt, string newID)
        {
            ProductManager pmgr = new ProductManager(ConnectionString);
            Product baseProd = pmgr.GetProduct(BaseID, CurrentLanguage);
            string basePath = "~/content/products/";
            string newPath = newID + "/";
            string imagesServerPath = Server.MapPath(basePath + newPath).ToLower();
            if (!Directory.Exists(imagesServerPath))
                Directory.CreateDirectory(imagesServerPath);
            if (Directory.Exists(imagesServerPath))
            {

                foreach (var img in Directory.GetFiles(Server.MapPath(basePath + "/" + Path.GetDirectoryName(baseProd.ThumbURL))))
                {
                    string newName = Path.GetFileNameWithoutExtension(img).Contains("_") ? newID + "_" + Path.GetFileNameWithoutExtension(img).Split('_')[1] : newID;
                    System.IO.File.Copy(img, imagesServerPath + newName + Path.GetExtension(img));
                }
            }
            string thumb = newPath + newID;
            produt.ThumbURL = thumb;
            if (Image != null)
            {


                string filePath = Server.MapPath(basePath + thumb) + Path.GetExtension(Image.FileName);

                Image.SaveAs(filePath);

                SaveImageSizes(imagesServerPath, thumb, filePath);
            }

            if (Images != null && Images.Length > 0 && Images[0] != null)
            {
                int startName = 1;
                if (produt != null && !string.IsNullOrEmpty(produt.Images))
                    startName = FindImageName(produt.Images);
                string imgs = "";
                foreach (var img in Images)
                {
                    if (!string.IsNullOrEmpty(imgs))
                        imgs += ",";

                    string fileExtension = Path.GetExtension(img.FileName);
                    string filePath = imagesServerPath + newID + startName + fileExtension;
                    string filename = newID + startName;

                    imgs += startName;

                    img.SaveAs(filePath);
                    SaveImageSizes(imagesServerPath, filename, filePath);
                    startName += 1;
                }
                if (produt != null && string.IsNullOrEmpty(produt.Images))
                    produt.Images += imgs;
                else
                    produt.Images += "," + imgs;

            }
        }


        public ActionResult ConfirmOrder()
        {
            CommerceManager cmgr = new CommerceManager(ConnectionString);
            string orderID = null;
            if (!string.IsNullOrEmpty(Request["orderID"]))
            {
                orderID = Request["orderID"];
                Cart order = cmgr.GetCartOrder<SellableProduct>(orderID);
                CartHelper.NewCart();
                HttpContext.Session["cart"] = order;
                return RedirectToAction("ViewCart", "Cart");

            }
            else
            {
                return Redirect("_404");
            }
        }

        void SaveImageSizes(string imagesServerPath, string fileName, string filePath)
        {
            string postNumber = string.Empty;
            CMSConfigurations config = CMSConfigurations.GetSection();
            foreach (CMSImageInfo imginfo in config.Images)
            {
                string namerest = (string.IsNullOrEmpty(imginfo.OverrideName) ? imginfo.Name : imginfo.OverrideName);
                ImageHelper.SaveImage(imagesServerPath + fileName + "_" + namerest, filePath, "", imginfo.Width, imginfo.Height, imginfo.Anchor, imginfo.ResizeMode, (imginfo.ImageFormat == "png" ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg), imginfo.BaseImage, imginfo.Padding);
            }
            ImageHelper.SaveImage(imagesServerPath + fileName + "_thumb" + ".png", filePath, "", 192, 141, ImageHelper.Anchor.Center, ImageHelper.PicResizeMode.Transparent, System.Drawing.Imaging.ImageFormat.Png, 0);
        }

        public ActionResult AddFastCart(string id)
        {
            ViewBag.Title = Resources.Strings.Client + " | " + Resources.Strings.Add + " " + Resources.Strings.FastCart;
            CommerceManager cmgr = new CommerceManager(ConnectionString);
            FastCart fastCart = cmgr.GetFastCart(new Guid(id), CurrentLanguage);

            return View(fastCart);
        }

        void FillSEO(int pageID)
        {
            StaticPageManager pmgr = new StaticPageManager(ConnectionString);
            StaticPage page = pmgr.GetStaticPage(pageID, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
        }


        [IsLoggedIn]
        public ActionResult ShopOrders()
        {
            int num;
            int arg;
            ViewBag.Title = Resources.Strings.Client + " | " + "Shop Orders";
            CommerceManager cmgr = new CommerceManager(base.ConnectionString);

            List<Cart> userOrders = cmgr.GetShopOrders(CurrentUser.ID, 1, int.MaxValue, out num, out arg, CartStatus.Cancelled, CartStatus.Complete, base.CurrentLanguage);

            ViewBag.OpenOrders = userOrders.FindAll(x => x.Status >= CartStatus.New && x.Status < CartStatus.Complete).Count;
            ViewBag.CompleteOrders = userOrders.FindAll(x => x.Status == CartStatus.Complete).Count;
            ViewBag.CancelledOrders = userOrders.FindAll(x => x.Status == CartStatus.Cancelled).Count;
            ViewBag.OrdersCount = arg;

            return base.View(userOrders);
        }

        [IsLoggedIn]
        public ActionResult ShopOrder(string ID)
        {

            ViewBag.Title = Resources.Strings.Client + " | " + "Shop Order";

            CommerceManager cmgr = new CommerceManager(base.ConnectionString);
            Cart cartOrder = cmgr.GetCartOrder<SellableProduct>(ID);

            ViewBag.Title = Resources.Strings.Client + "Order #" + cartOrder.OrderID;
            ViewBag.UserTransaction = cmgr.GetUserTransctions(1, int.MaxValue, out int dummy, cartOrder.OwnerID);
            ViewBag.Name = cartOrder.OrderAddress.Label;
            ViewBag.Street = cartOrder.OrderAddress.Address1;
            ViewBag.Phone = cartOrder.OrderAddress.Phone2;

            switch (cartOrder.Status)
            {
                case CartStatus.New:
                case CartStatus.Paid:
                    ViewBag.Case = CartStatus.Paid.ToString();
                    break;
                case CartStatus.Pending:
                    break;
                case CartStatus.ReadyToShip:
                    ViewBag.Case = CartStatus.ReadyToShip.ToString();
                    break;
                case CartStatus.Shipped:
                    ViewBag.Case = CartStatus.Shipped.ToString();
                    break;
                default:
                    ViewBag.Case = CartStatus.Complete.ToString();
                    break;
            }
            return base.View(cartOrder);
        }

        [IsLoggedIn]
        public ActionResult ManageOrder()
        {
            string id = Request["id"];

            CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
            MemberManager mgr = new MemberManager(ConnectionString);
            CartStatus status = (CartStatus)Enum.Parse(typeof(CartStatus), Request["action"]);
            DirectoryManager dgr = new DirectoryManager(ConnectionString);
            DirectoryOutlet shop = dgr.GetOutletByOwner(CurrentUser.ID, CurrentLanguage);
            Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(id);
            switch (status)
            {
                case CartStatus.Paid:
                    string trackingNo = Request["trackingNo"];
                    commerceManager.SaveCartTracking(cartOrder.ID, trackingNo);
                    commerceManager.UpdateCartStatus(cartOrder.ID, CartStatus.ReadyToShip);
                    Helper.SendEmail("site@vikikfashion.com", "order@vikikfashion.com", CartStatus.ReadyToShip.ToString() + " | " + cartOrder.OrderID, string.Format(Resources.Strings.OrderUpdateBody, cartOrder.OrderID, CartStatus.ReadyToShip, shop.Name));
                    break;
                case CartStatus.ReadyToShip:
                    commerceManager.UpdateCartStatus(cartOrder.ID, CartStatus.Shipped);
                    Helper.SendEmail("site@vikikfashion.com", "order@vikikfashion.com", CartStatus.Shipped.ToString() + " | " + cartOrder.OrderID, string.Format(Resources.Strings.OrderUpdateBody, cartOrder.OrderID, CartStatus.Shipped, shop.Name));
                    break;
                case CartStatus.Shipped:
                case CartStatus.Complete:
                    commerceManager.UpdateCartStatus(cartOrder.ID, CartStatus.Complete);
                    Helper.SendEmail("site@vikikfashion.com", "order@vikikfashion.com", CartStatus.Complete.ToString() + " | " + cartOrder.OrderID, string.Format(Resources.Strings.OrderUpdateBody, cartOrder.OrderID, CartStatus.Complete, shop.Name));
                    bool PointsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["PointsEnabled"]);
                    //double PointValue = Convert.ToDouble(ConfigurationManager.AppSettings["PointValue"]);
                    if (PointsEnabled)
                    {
                        int points = Convert.ToInt32(Math.Floor(cartOrder.TotalWOShipping));
                        commerceManager.AddPoints(cartOrder.OwnerID, points,string.Format("auto by complete order {0}",cartOrder.OrderID),CurrentUser.ID);
                        User user = mgr.GetUser(cartOrder.OwnerID);
                        Helper.SendEmail("site@vikikfashion.com", "order@vikikfashion.com", Resources.Strings.OrderComplete, string.Format(Resources.Strings.OrderCompleteBody, cartOrder.OrderID));
                        Helper.SendEmail("site@vikikfashion.com", CurrentUser.Email, Resources.Strings.OrderComplete, string.Format(Resources.Strings.OrderCompleteBody, cartOrder.OrderID));
                        Helper.SendEmail("site@vikikfashion.com", user.Email, Resources.Strings.OrderComplete, string.Format(Resources.Strings.OrderCompleteBody, cartOrder.OrderID));

                    }
                    break;
                default:
                    break;
            }
            return RedirectToAction("ShopOrder", "User", new { id });
        }

        [IsLoggedIn]
        public ActionResult Order(string ID)
        {
            CommerceManager cmgr = new CommerceManager(base.ConnectionString);
            Cart cartOrder = cmgr.GetCartOrder<SellableProduct>(ID);
            if (cartOrder.OwnerID != base.CurrentUser.ID)
            {
                return base.View("_404");
            }
            ViewBag.Title = Resources.Strings.Client + "Order #" + cartOrder.OrderID;
            ViewBag.UserTransaction = cmgr.GetUserTransctions(1, int.MaxValue, out int dummy, CurrentUser.ID);
            ViewBag.Name = cartOrder.OrderAddress.Label;
            ViewBag.Street = cartOrder.OrderAddress.Address1;
            ViewBag.Phone = cartOrder.OrderAddress.Phone2;
            return View("order", cartOrder);
        }

        public ActionResult MyWishlist()
        {
            int dummy;
            FillSEO(-8);
            ProductManager mgr = new ProductManager(ConnectionString);
            ViewBag.WishlistProducts = mgr.ProductsGetUserWishlist(CurrentUser.ID, CurrentLanguage, 1, 100, out dummy, 1);
            return View("MyWishlist");
        }

        //public ActionResult FastCart()
        //{
        //    ViewBag.Title = Resources.Strings.Client + " | " + Resources.Strings.FastCart;
        //    CommerceManager cmgr = new CommerceManager(ConnectionString);
        //    ProductManager pmgr = new ProductManager(ConnectionString);
        //    ViewBag.AddShopCategories = pmgr.GetCategories(CurrentLanguage);
        //    var fastCarts = cmgr.GetFastCarts(CurrentUser.ID);
        //    return View(fastCarts);
        //}

        public ActionResult AddCart()
        {
            ViewBag.Title = Resources.Strings.Client + " | " + Resources.Strings.AddNew + " " + Resources.Strings.FastCart;

            return View();
        }


        public ActionResult AddToWishlist(string id)
        {

            string prodID = id;
            ProductManager pMgr = new ProductManager(ConnectionString);
            if (pMgr.ProductIsInWishlist(id, CurrentUser.ID, 1))
            {
                pMgr.ProductRemoveFromWishlist(id, CurrentUser.ID, 1);
                // btnwishlisttext.InnerText = Resources.Vikik.AddToWishlist;
            }
            else
            {
                pMgr.ProductAddToWishlist(id, CurrentUser.ID, 1);
                //btnwishlisttext.InnerText = Resources.Vikik.RemoveFromWishlist;
            }

            if (!string.IsNullOrEmpty(Request["rt"]))
            {
                string rt = Request["rt"];
                rt = rt.Replace("__", "?").Replace("_", "&");
                return Redirect(Url.Content("~/" + rt));
            }


            return Redirect(Url.Action("MyWishlist", "User"));
        }

        public static bool IsMerchent(User currentUser)
        {
            bool result = false;
            if (currentUser == null || currentUser.MemberOf.Find(x => x.ID == new Guid("22222222-2222-2222-2222-222222222222")) == null)
                result = false;
            else
                result = true;
            return result;
        }

        public static bool IsDoctor(User currentUser)
        {
            bool result = false;
            if (currentUser == null || currentUser.MemberOf.Find(x => x.ID == new Guid("55555555-5555-5555-5555-555555555555")) == null)
                result = false;
            else
                result = true;
            return result;
        }

        int FindImageName(string imgList)
        {
            int value = 1;
            if (!string.IsNullOrEmpty(imgList))
            {
                foreach (var img in imgList.Split(','))
                {
                    int m = -1;
                    int.TryParse(img, out m);
                    value = value <= m ? value + 1 : value;
                }
            }
            return value;
        }

        string RemoveThumb(string imgs, string img)
        {
            List<string> ImageList = null;
            if (!string.IsNullOrEmpty(imgs) && imgs.Split(',').Length > 0)
            {
                ImageList = new List<string>();
                ImageList = imgs.Split(',').ToList();
                ImageList.Remove(img);

                return string.Join(",", ImageList.ToArray());
            }
            else
                return imgs;
        }

        [HttpPost]
        public ActionResult DeleteProduct()
        {
            string rt = Request["rt"];
            string pID = Request["id"];
            string action = Request["action"];
            ProductManager pmgr = new ProductManager(ConnectionString);
            var product = pmgr.GetProduct(pID, CurrentLanguage);
            if (product.OwnerID == CurrentUser.ID)
            {
                if (action == "1")
                    pmgr.DeleteProduct(pID);
                else
                    pmgr.SaveProductBasicInfo(product, product.Approved, false);
            }
            return Redirect("~" + rt);
        }

        public ActionResult DeleteImage()
        {
            string rt = Request["rt"];
            string img = Request["img"];
            string objCase = Request["objCase"];
            string index = Request["index"];
            int objID = Convert.ToInt32(Request["objID"]);
            ProductManager pmgr = new ProductManager(ConnectionString);


            switch (objCase)
            {
                case "Product":
                    Product obj = pmgr.GetProduct(objID.ToString(), CurrentLanguage);
                    string filePath = "/" + obj.FullImagePath + index + "_thumb.png";
                    if (System.IO.File.Exists(Server.MapPath(filePath)))
                    {
                        filePath = "/content/products/" + objID;
                        List<string> files = Directory.GetFiles(Server.MapPath(filePath)).ToList();
                        foreach (var item in files.FindAll(x => System.IO.Path.GetFileName(x).Split('_')[0] == img))
                        {
                            System.IO.File.Delete(item);
                        }

                    }
                    obj.Images = RemoveThumb(obj.Images, index);
                    pmgr.SaveProductBasicInfo(obj, obj.Approved, true);
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(rt))
                return Redirect("~/");
            else
                return Redirect("~/" + rt);
        }

    }
}