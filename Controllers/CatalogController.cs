using CMS;
using CMS.Helpers;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace vikik.Controllers
{
    public class CatalogController : KensoftController
    {
        public ActionResult Category(string id)
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
            return View("category", category);
        }

        public ActionResult ProductDetails(string id)
        {
            int dummy;
            ProductManager mgr = new ProductManager(ConnectionString);
            MemberManager ugr = new MemberManager(ConnectionString);
            Product prod = mgr.GetProduct(id, CurrentLanguage);
            if (prod.GroupID.HasValue)
            {//size & color
                //var sizes = mgr.ProductsGetGrouped((int)prod.GroupID, CurrentLanguage, null);
                //prod = sizes.Find(x => x.Sellable = true);
                //sizes.Remove(prod);
                //ViewBag.Sizes = sizes;
                var SizesColor = mgr.ProductsGetGrouped((int)prod.GroupID, CurrentLanguage, null);
                prod = SizesColor.Find(x => x.Searchable == true);
                SizesColor.Remove(prod);
                prod = mgr.GetProduct(prod.ID, CurrentLanguage);//searchable prod
                ViewBag.productV = prod;
                ViewBag.SizesColor = SizesColor;
            }
            ViewBag.Title = prod.HTMLTitle;
            ViewBag.Description = prod.HTMLDescription;
            GeneralManager gmgr = new GeneralManager(ConnectionString);
             gmgr.ItemSizesGet();
            //ViewBag.prod = mgr.GetProduct("270709", CurrentLanguage);


            /////////////////////////////////////////////////////
            //ViewBag.RelatedProducts = mgr.GetProducts(prod.CategoryID, null, CurrentLanguage, 1, 8, out dummy,null,null,null,null,null,null,null,true,true,false).FindAll(x => x.ID != id).ToList();
           
            //LookupManager lMgr = new LookupManager(ConnectionString);
            
          
            //if (CurrentUser != null)
            //{
            //    ViewBag.CurrentUser = CurrentUser;
            //    ViewBag.IsInWishlist = mgr.ProductsGetUserWishlist(CurrentUser.ID, CurrentLanguage, 1, int.MaxValue, out dummy, 1).Find(x => x.ID == id) != null ? true : false;
            //}
            //ViewBag.ParentCategory = mgr.GetCategory( mgr.GetCategory( prod.CategoryID,CurrentLanguage).ParentID, CurrentLanguage);
            //if(ViewBag.ParentCategory != null && ViewBag.ParentCategory.ID == "70166")
            //{
            //    if(prod.Specs != null && prod.Specs.Find(x => x.ID == "30041") != null)
            //    {
            //        ViewBag.BaseProduct = mgr.GetProduct(prod.Specs.Find(x => x.ID == "30041").Value, CurrentLanguage);
            //    }
            //    ViewBag.Owner = ugr.GetUser(prod.OwnerID,CurrentLanguage);


            //}
                return View(prod);
        }

        void FillSEO(int pageID)
        {
            StaticPageManager pmgr = new StaticPageManager(ConnectionString);
            StaticPage page = pmgr.GetStaticPage(pageID, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
        }

        [IsLoggedIn]
        public ActionResult MyOrders()
        {
            int num;
            int arg;
            FillSEO(-7);
            CommerceManager cmgr = new CommerceManager(base.ConnectionString);
            List<Cart> userOrders = cmgr.GetUserOrders(base.CurrentUser.ID, 1, int.MaxValue, out num, out arg, CartStatus.Cancelled, CartStatus.Complete, base.CurrentLanguage);
            var userTransaction = cmgr.GetUserTransctions(1, int.MaxValue, out int dummy, CurrentUser.ID, null, null, null, null, null, null);
        //    var userTransaction = cmgr.GetUserTransctions(1, int.MaxValue, out int dummy, CurrentUser.ID);
            ViewBag.UserTransaction = userTransaction;
            ViewBag.OpenOrders = userOrders.FindAll(x => x.Status >= CartStatus.New && x.Status < CartStatus.Complete).Count;
            ViewBag.CompleteOrders = userOrders.FindAll(x => x.Status == CartStatus.Complete).Count;
            ViewBag.CancelledOrders = userOrders.FindAll(x => x.Status == CartStatus.Cancelled).Count;
            ViewBag.OrdersCount = arg;
            int range = (20 >= arg ? arg : 20);
            return base.View(userOrders.GetRange(0, range));
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
            ViewBag.UserTransaction = cmgr.GetUserTransctions(int.MaxValue, 1 , out int dummy ,CurrentUser.ID );
            ViewBag.Name = cartOrder.OrderAddress.Label;
            ViewBag.Street = cartOrder.OrderAddress.Address1;
            ViewBag.Phone = cartOrder.OrderAddress.Phone2;
            return base.View("order", cartOrder);
        }

        public ActionResult ProductDetailsSize()
        {
            return View();  
        }
        

    }
}