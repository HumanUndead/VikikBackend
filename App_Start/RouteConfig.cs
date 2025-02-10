using System.Web.Mvc;
using System.Web.Routing;

namespace vikik
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
             name: "list Cash order",
             url: "lists/order/deliverypay",
             defaults: new { controller = "Lists", action = "PayOnDelivery" }
         );
            routes.MapRoute(
            name: "Featured Page",
            url: "Home/Page/{id}/{dummy}",
            defaults: new { controller = "Home", action = "Page", dummy = UrlParameter.Optional }
        );
            routes.MapRoute(
            name: "ComingSoon",
            url: "ComingSoon",
            defaults: new { controller = "Home", action = "ComingSoon" }
        );
            routes.MapRoute(
           name: "Details2",
           url: "Member/Login",
           defaults: new { controller = "Member", action = "Login" }
       );
            routes.MapRoute(
             name: "Details",
             url: "Member/Profile",
             defaults: new { controller = "Member", action = "Details" }
         );
            routes.MapRoute(
          name: "FAQ",
          url: "faq",
          defaults: new { controller = "Home", action = "FAQ" }
      );
            routes.MapRoute(
            name: "Contact Us",
            url: "contactus",
            defaults: new { controller = "Home", action = "ContactUs" }
        );
            routes.MapRoute(
          name: "Terms And Conditions",
          url: "TermsAndConditions",
          defaults: new { controller = "Home", action = "TermsAndConditions" }
      );
            routes.MapRoute(
          name: "Privacy Policy",
          url: "PrivacyPolicy",
          defaults: new { controller = "Home", action = "PrivacyPolicy" }
      );
            routes.MapRoute(
            name: "Search",
            url: "Search",
            defaults: new { controller = "Home", action = "Search" }
            );
            routes.MapRoute(
          name: "Delivery",
          url: "Delivery",
          defaults: new { controller = "Home", action = "Delivery" }
      );
            routes.MapRoute(
              name: "listpage",
              url: "lists/page",
              defaults: new { controller = "Lists", action = "Page" }
          );
            routes.MapRoute(
             name: "register",
             url: "register",
             defaults: new { controller = "Member", action = "Register" }
         );


            routes.MapRoute(
          name: "list View Cart",
          url: "lists/cart",
          defaults: new { controller = "Lists", action = "ViewCart" }
      );
            routes.MapRoute(
             name: "list review Cart",
             url: "lists/cart/review",
             defaults: new { controller = "Lists", action = "review" }
         );
            routes.MapRoute(
              name: "list add to Cart",
              url: "lists/cart/add/{pid}",
              defaults: new { controller = "Lists", action = "AddToCart" }
          );
            routes.MapRoute(
              name: "list add to Cart 2",
              url: "lists/cart/add",
              defaults: new { controller = "Lists", action = "AddToCart" }
          );
            routes.MapRoute(
              name: "list Remove From Cart",
              url: "lists/cart/remove",
              defaults: new { controller = "Lists", action = "RemoveFromCart" }
          );
            routes.MapRoute(
              name: "docreatelist",
              url: "lists/create",
              defaults: new { controller = "Lists", action = "DoCreateList" }
          );
            routes.MapRoute(
              name: "createlist",
              url: "lists/createlist",
              defaults: new { controller = "Lists", action = "CreateList" }
          );
            routes.MapRoute(
               name: "weddinglist",
               url: "lists/weddinglist",
               defaults: new { controller = "Lists", action = "WeddingList" }
           );
            routes.MapRoute(
               name: "myweddinglist",
               url: "lists/myweddinglist",
               defaults: new { controller = "Lists", action = "MyList", type = 1 }
           );
            routes.MapRoute(
               name: "addtomylist",
               url: "lists/add/{type}/{prodid}",
               defaults: new { controller = "Lists", action = "AddProductToList" }
           );

            routes.MapRoute(
              name: "category2",
              url: "store",
              defaults: new { controller = "Catalog", action = "Category" }
          );
            routes.MapRoute(
            name: "Shop Category Request",
            url: "shop",
            defaults: new { controller = "Shop", action = "Category", dummy = UrlParameter.Optional }
        );

            routes.MapRoute(
              name: "Shop Categories",
              url: "shop/categories/{id}/{dummy}",
              defaults: new { controller = "Shop", action = "Categories", dummy = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "shop cat page",
                url: "shop/{cid}/{dummy2}",
               //url: "shop/{dummy}/{cid}",
               defaults: new { controller = "Shop", action = "ShopCat" }
           );

            routes.MapRoute(
               name: "AllShopCategories",
                url: "shop/cats/{cid}/{dummy2}",
               defaults: new { controller = "Shop", action = "AllShopCategories", cid = UrlParameter.Optional, dummy = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "product details",
               url: "catalog/product/{id}/{dummy}",
               defaults: new { controller = "Catalog", action = "ProductDetails" }
           );
            routes.MapRoute(
               name: "My Orders",
               url: "catalog/myorders",
               defaults: new { controller = "Catalog", action = "MyOrders" }
           );
            routes.MapRoute(
               name: "Order",
               url: "catalog/order/{id}",
               defaults: new { controller = "Catalog", action = "Order" }
           );
            routes.MapRoute(
           name: "View Cart",
           url: "cart",
           defaults: new { controller = "cart", action = "ViewCart" }
       );
            routes.MapRoute(
           name: "pages",
           url: "pages/{id}/{dummy}",
           defaults: new { controller = "Home", action = "Pages", dummy = UrlParameter.Optional }
       );
            routes.MapRoute(
             name: "review Cart",
             url: "cart/review",
             defaults: new { controller = "cart", action = "review" }
         );
            routes.MapRoute(
              name: "add to Cart",
              url: "cart/add/{pid}",
              defaults: new { controller = "cart", action = "AddToCart" }
          );
            routes.MapRoute(
              name: "add to Cart 2",
              url: "cart/add",
              defaults: new { controller = "cart", action = "AddToCart" }
          );
            routes.MapRoute(
             name: "set item quantity",
             url: "cart/SetQty",
             defaults: new { controller = "cart", action = "SetQty" }
         );
            routes.MapRoute(
              name: "Remove From Cart",
              url: "cart/remove",
              defaults: new { controller = "cart", action = "RemoveFromCart" }
          );
            routes.MapRoute(
             name: "Remove From Cart 2",
             url: "cart/remove/{id}",
             defaults: new { controller = "cart", action = "RemoveFromCart" }
         );
            routes.MapRoute(
            name: "Move To Wishlist",
            url: "cart/MoveToWishlist",
            defaults: new { controller = "cart", action = "MoveToWishlist" }
        );
            //   routes.MapRoute(
            //    name: "Search",
            //    url: "Search",
            //    defaults: new { controller = "Shop", action = "Search" }
            //);
            routes.MapRoute(
              name: "PreCheckout",
              url: "cart/precheckout",
              defaults: new { controller = "cart", action = "precheckout" }
          );
            routes.MapRoute(
              name: "Checkout",
              url: "cart/checkout",
              defaults: new { controller = "cart", action = "checkout" }
          );
            routes.MapRoute(
             name: "Clearance",
             url: "clearance",
             defaults: new { controller = "shop", action = "clearance" }
         );
            routes.MapRoute(
            name: "Clearance2",
            url: "clearance/{cid}/{dummy}",
            defaults: new { controller = "shop", action = "clearance" }
        );
            routes.MapRoute(
          name: "addshoplisting4",
          url: "addshoplisting/done",
          defaults: new { controller = "shop", action = "addshoplisting3" }
      );
            routes.MapRoute(
              name: "addshoplisting3",
              url: "addshoplisting/{id}/done",
              defaults: new { controller = "shop", action = "addshoplisting3", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "addshoplisting2",
               url: "addshoplisting/{catid}",
               defaults: new { controller = "shop", action = "addshoplisting2" }
           );
            routes.MapRoute(
               name: "addshoplisting",
               url: "addshoplisting",
               defaults: new { controller = "shop", action = "addshoplisting" }
           );
            routes.MapRoute(
               name: "categories",
               url: "categories",
               defaults: new { controller = "shop", action = "categories" }
           );

            routes.MapRoute(
           name: "logout",
           url: "member/logout",
           defaults: new { controller = "Member", action = "Logout" }
       );
            routes.MapRoute(
           name: "login",
           url: "member/login",
           defaults: new { controller = "Member", action = "Login" }
       );
            routes.MapRoute(
            name: "ChangePassword",
            url: "member/ChangePassword",
            defaults: new { controller = "Member", action = "ChangePassword" }
            );
            routes.MapRoute(
           name: "forgotpass",
           url: "member/forgotpassword",
           defaults: new { controller = "Member", action = "ForgotPassword" }
       );
            routes.MapRoute(
           name: "doresetpassword",
           url: "doresetpassword/{email}/{rid}",
           defaults: new { controller = "Member", action = "DoResetPassword" }
       );
            routes.MapRoute(
            name: "MyAds",
            url: "user/ads",
            defaults: new { controller = "User", action = "MyAds" }
        );
            routes.MapRoute(
           name: "MyWishlist",
           url: "user/wishlist",
           defaults: new { controller = "User", action = "MyWishlist" }
       );
            routes.MapRoute(
          name: "addtowishlist",
          url: "user/wishlist/add/{id}",
          defaults: new { controller = "User", action = "AddToWishlist" }
      );
            routes.MapRoute(
            name: "upload",
            url: "UploadFiles",
            defaults: new { controller = "FileUpload", action = "UploadFiles" }
        );
            routes.MapRoute(
              name: "download",
              url: "download",
              defaults: new { controller = "FileUpload", action = "download" }
          );
            routes.MapRoute(
              name: "delete",
              url: "delete/{id}/do",
              defaults: new { controller = "FileUpload", action = "delete" }
          );
            routes.MapRoute(
             name: "Start Payment Integration",
             url: "order/startpay",
             defaults: new { controller = "order", action = "StartPay" }
         );
            routes.MapRoute(
              name: "Approve order",
              url: "order/accept",
              defaults: new { controller = "order", action = "OrderPaid" }
          );
            routes.MapRoute(
             name: "Cash order",
             url: "order/deliverypay",
             defaults: new { controller = "order", action = "PayOnDelivery" }
         );
            routes.MapRoute(
              name: "addressadd",
              url: "handlers/address/add",
              defaults: new { controller = "Handlers", action = "AddressAdd" }
          );
            routes.MapRoute(
               name: "categoryhandler",
               url: "handlers/products/{id}/{page}",
               defaults: new { controller = "Handlers", action = "Products" }
           );
            routes.MapRoute(
               name: "clearancehandler",
               url: "handlers/clearance/{page}",
               defaults: new { controller = "Handlers", action = "Products" }
           );
            routes.MapRoute(
              name: "shopssuggestions",
              url: "handlers/shopssearch",
              defaults: new { controller = "Handlers", action = "ShopsSearch" }
          );

            routes.MapRoute(
          name: "AppleAppSiteAssociation",
          url: ".well-known/apple-app-site-association",
          defaults: new { controller = "Home", action = "GetAppleAppSiteAssociation" }
      );
            routes.MapRoute(
          name: "assetlinks",
          url: ".well-known/assetlinks.json",
          defaults: new { controller = "Home", action = "Getassetlinks" }
      ); 
            
            routes.MapRoute(
          name: "assetlinks2",
          url: ".well-known/assetlinks",
          defaults: new { controller = "Home", action = "Getassetlinks" }
      );
            routes.MapRoute(
                name: "Default",
                url: "{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute("capture payment", "PaymentComplete", new
            {
                controller = "Home",
                action = "PaymentComplete"
            });
            routes.MapRoute(
                name: "Default 2",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Post",
               url: "{action}/{id}/{dummy}/{name}",
               defaults: new { controller = "Home", action = "Post", id = UrlParameter.Optional }
           );

        }
    }
}
