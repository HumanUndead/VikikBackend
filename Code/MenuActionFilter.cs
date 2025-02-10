using CMS;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using CMS.Config;
namespace vikik
{
    public class MenuActionFilter:ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            string ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Language CurrentLanguage = (Language)filterContext.HttpContext.Session["lang"];
            MenuManager mMgr = new MenuManager(ConnectionString);
            filterContext.Controller.ViewBag.MenuItems = mMgr.GetSubMenuItems("63", CurrentLanguage);
            filterContext.Controller.ViewBag.MenuManager = mMgr;

            StaticPageManager mgr = new StaticPageManager(ConnectionString);
            StaticPage page = mgr.GetStaticPage(8, CurrentLanguage);
            filterContext.Controller.ViewBag.email1 = page.SubTitle;
            filterContext.Controller.ViewBag.email2 = page.Text;

            StaticPage page2 = mgr.GetStaticPage(9, CurrentLanguage);
            filterContext.Controller.ViewBag.phone1 = page2.SubTitle;
            filterContext.Controller.ViewBag.phone2 = page2.Text;

            StaticPage page3 = mgr.GetStaticPage(10008, CurrentLanguage);
            filterContext.Controller.ViewBag.whatsapp = page3.Text;    
            
            StaticPage pagePOpUp = mgr.GetStaticPage(10011, CurrentLanguage);
            filterContext.Controller.ViewBag.POpUp = pagePOpUp;


            filterContext.Controller.ViewBag.CurrentLanguage = CurrentLanguage;

            //ArticleManager Amgr = new ArticleManager(ConnectionString);
            //filterContext.Controller.ViewBag.Cities = Amgr.GetArticleType("", CurrentLanguage);

           ProductManager pmgr = new ProductManager(ConnectionString); 
            filterContext.Controller.ViewBag.Categories = pmgr.GetCategoryCategories(null, CurrentLanguage);
            filterContext.Controller.ViewBag.AllCategories = pmgr.GetCategories(CurrentLanguage);

            filterContext.Controller.ViewBag.CurrentLanguage = CurrentLanguage;
            filterContext.Controller.ViewBag.ConnectionString = ConnectionString;
            filterContext.Controller.ViewBag.MemberManager = new MemberManager(ConnectionString);
            filterContext.Controller.ViewBag.CartItemsCount = CMS.Helpers.CartHelper.CurrentCart.ItemsCount;
            User CurrentUser = HttpContext.Current.Session["User"] != null ? (User)HttpContext.Current.Session["User"] : null;
            if (CurrentUser != null)
            {
                ProductManager pMgr = new ProductManager(ConnectionString);
                filterContext.Controller.ViewBag.WishlisttemsCount = pMgr.ProductsGetUserWishlist(CurrentUser.ID,CurrentLanguage,1,int.MaxValue,out int dummy,1).Count;
                if (vikik.Controllers.UserController.IsMerchent(CurrentUser))
                {
                    CommerceManager cgr = new CommerceManager(ConnectionString);
                    filterContext.Controller.ViewBag.ShopOrdersCount = cgr.GetShopOrders(CurrentUser.ID,1,999,out int totalpages,out int rowCount,CartStatus.New,CartStatus.Pending,CurrentLanguage).Count;
                }
                filterContext.Controller.ViewBag.IsAdmin = CurrentUser.ID==new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA");
            }

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.OneDayOldPrice = "2";
            filterContext.Controller.ViewBag.OneDayPrice = 2;
            filterContext.Controller.ViewBag.OneDayDays = 1;

            filterContext.Controller.ViewBag.SevenDayOldPrice = "14";
            filterContext.Controller.ViewBag.SevenDayPrice = 11;
            filterContext.Controller.ViewBag.SevenDayDays = 7;

            filterContext.Controller.ViewBag.FourteenDayOldPrice = "28";
            filterContext.Controller.ViewBag.FourteenDayPrice = 19;
            filterContext.Controller.ViewBag.FourteenDayDays = 14;

            filterContext.Controller.ViewBag.OneMonthOldPrice = "60";
            filterContext.Controller.ViewBag.OneMonthPrice = 30;
            filterContext.Controller.ViewBag.OneMonthDays = 30;
            base.OnActionExecuting(filterContext);
        }
    }
}