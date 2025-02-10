using CMS;
using CMS.Helpers;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Routing;

namespace vikik.Controllers
{

    public class CartController : KensoftController
    {
        // GET: Cart
        [IsLoggedIn]
        public ActionResult ViewCart()
        {
            CommerceManager mgr = new CommerceManager(ConnectionString);
            ViewBag.Addresses = mgr.GetUserAddresses(CurrentUser.ID);

            LookupManager lMgr = new LookupManager(ConnectionString);
            ViewBag.LookupManager = lMgr;
            ViewBag.CurrentLanguage = CurrentLanguage;

            GeneralManager gMgr = new GeneralManager(ConnectionString);
            FillSEO(7);
            //Cities 
            ViewBag.Cities = gMgr.CitiesGet(null,CurrentLanguage, "");


            ViewBag.Country = lMgr.GetLookUpCategoryItems(7, CurrentLanguage);
            ViewBag.City = gMgr.CitiesGet(null, CurrentLanguage, "");

            ViewBag.BaseShipping = 0;
           // CartHelper.CurrentCart.ShippingPrice = 30;

            //CartHelper.CurrentCart.ShippingPrice = (CartHelper.CurrentCart.ItemsCount >= 3 ? 0 : 0);//GetShippingPrice(ViewBag.BaseShipping);
            double previousBalance = mgr.GetUserBalance(CurrentUser.ID);
            ViewBag.PreviousBalance = previousBalance;
            int Points = mgr.GetUserPoints(CurrentUser.ID);
            ViewBag.AvailablePoints = Points;
            if (previousBalance > 0)
            {
                double total = CartHelper.CurrentCart.Total - previousBalance;
                ViewBag.Total = GeneralHelper.FormatCurrency(total);
            }
            else
                ViewBag.Total = CartHelper.CurrentCart.FormattedTotal;
            if (CartHelper.CurrentCart.ItemsCount < 3)
            {
                //ViewBag.Message = string.Format(Resources.Project.AddMoreItems, 3 - CartHelper.CurrentCart.ItemsCount);
            }


            //if (CartHelper.CurrentCart.Notes != null)
            //{
            //    string[] note = Convert.ToString(CartHelper.CurrentCart.Notes).Split(new string[] { "\n" }, StringSplitOptions.None);
            //    foreach (var item in note)
            //    {
            //        if (item.Contains("prodSizeColorID") && item.Split(':').Length == 2)
            //        {
            //            if (item.Split(':').Length > 1)
            //            {
            //                string prodId = item.Split(':')[1];
            //                ProductManager Pmgr = new ProductManager(ConnectionString);
            //              ViewBag.Color =Pmgr.GetProduct(prodId, CurrentLanguage).Color.Label;
            //              ViewBag.Size = Pmgr.GetProduct(prodId, CurrentLanguage).Size.Label;
            //            }
            //        }
            //    }
            //}


            ViewBag.User = CurrentUser;
            return View("Cart", CartHelper.CurrentCart);


            //double FreeShippingLimit = Convert.ToDouble(ConfigurationManager.AppSettings["OrderTotalLimit"]);
            //CartHelper.CurrentCart.ShippingPrice = Convert.ToDouble(ConfigurationManager.AppSettings["ShippingPrice"]);

            //CommerceManager mgr = new CommerceManager(ConnectionString);

            //StaticPageManager pmgr = new StaticPageManager(ConnectionString);
            //StaticPage mpage = pmgr.GetStaticPage(-14, CurrentLanguage);
            //ViewBag.Description = mpage.Description;
            //ViewBag.Title = mpage.Title;
            //FillSEO(-5);
            //mpage = pmgr.GetStaticPage(-15, CurrentLanguage);
            ////ltlDeliveryNote.Text = mpage.Text;

            //if (CurrentUser != null)
            //{
            //    var addresses = mgr.GetUserAddresses(CurrentUser.ID);
            //    ViewBag.HomeAddress = mgr.GetDefaultAddress(CurrentUser.ID);
            //    ViewBag.WorkAddresses = ViewBag.HomeAddress != null ? addresses.FirstOrDefault(x => x.ID != ViewBag.HomeAddress.ID) : null;
            //}

            //LookupManager lMgr = new LookupManager(ConnectionString);
            //ViewBag.LookupManager = lMgr;
            //ViewBag.CurrentLanguage = CurrentLanguage;
            ////ViewBag.Countries = lMgr.GetLookupCategories(CurrentLanguage).FindAll(x => x.ID != 1013).ToList();
            //ViewBag.Locations = mgr.GetUserAddresses(new Guid("08B14F61-CB44-4125-A00C-B97D19B4E013"));

            //ViewBag.BaseShipping = Convert.ToDouble(ConfigurationManager.AppSettings["ShippingPrice"]);

            //if (CurrentUser != null)
            //{
            //    double previousBalance = mgr.GetUserBalance(CurrentUser.ID);

            //    ViewBag.PreviousBalance = previousBalance;
            //    int Points = mgr.GetUserPoints(CurrentUser.ID);
            //    ViewBag.AvailablePoints = Points;
            //    if (previousBalance > 0)
            //        ViewBag.Total = CartHelper.CurrentCart.Total - previousBalance;
            //    else
            //        ViewBag.Total = CartHelper.CurrentCart.Total;
            //}
            //List<List<CartItem>> prods = CartHelper.CurrentCart.Items.GroupBy(x => x.OwnerID).Select(grp => grp.ToList()).ToList();
            //ViewBag.Parcals = prods;
            //if (CartHelper.CurrentCart.TotalWOShipping >= FreeShippingLimit)
            //    CartHelper.CurrentCart.ShippingPrice = 0;
            //return View("Cart", CartHelper.CurrentCart);
        }

        void FillSEO(int pageID)
        {
            StaticPageManager pmgr = new StaticPageManager(ConnectionString);
            StaticPage page = pmgr.GetStaticPage(pageID, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
        }

        [IsLoggedIn]
        public ActionResult Review()
        {
            CommerceManager cMgr = new CommerceManager(ConnectionString);

            if (CartHelper.CurrentCart == null || string.IsNullOrEmpty(Request["checkoutAddressHidden"]) || !(CartHelper.CurrentCart.ItemsCount > 0))
            {
                return View("_404");
            }
            else
            {
                LookupManager lMgr = new LookupManager(ConnectionString);
                GeneralManager gMgr = new GeneralManager(ConnectionString);
                ViewBag.City = gMgr.CitiesGet(null,CurrentLanguage, "");

                string id = Request["checkoutAddressHidden"];
                foreach (var item in ViewBag.City)
                {

                    if (item.Name == cMgr.GetUserAddress(CurrentUser.ID, id).CityName)
                    {
                        CartHelper.CurrentCart.ShippingPrice = Convert.ToDouble(item.Value);
                    }
                }


                ViewBag.total = CartHelper.CurrentCart.FormattedOrderTotal;

                CartHelper.CurrentCart.OrderAddress = cMgr.GetUserAddress(CurrentUser.ID, id);
                
                ViewBag.Title = Resources.Strings.Project +" | " + Resources.Strings.CartReview;
              
                CartHelper.CartOwnerID = CurrentUser.ID;
                CartHelper.CurrentCart.Delivery = DeliveryMethod.PayOnDelivery;
                //CartHelper.CurrentCart.ShippingPrice = 3;
                //save 1-owner  2-cart(products)  3-address for shippingf
                foreach(CartItem item in CartHelper.CurrentCart.Items)
                {
                    //cMgr.SaveSellableProduct(item.Item.ProductID.ToString(), item.Item.UnitPrice, false, 0, Period.Days);
                    cMgr.SaveSellableProduct(item.Item.ProductID.ToString(), item.Item.UnitPrice, false, 0, 0);
                }
                CartHelper.CheckOutWithAddress(GatewayType.None);

                return View(CartHelper.CurrentCart);
            }
            //else
            //{
            //    CommerceManager cMgr = new CommerceManager(ConnectionString);
            //    MemberManager mmgr = new MemberManager(ConnectionString);
            //    ProductManager pmgr = new ProductManager(ConnectionString);
            //    foreach (string key in Request.Form.Keys)
            //    {
            //        if (key.Contains("pid"))
            //        {
            //            string index = key.Replace("pid", "");
            //            string id = Request.Form[key];
            //            string quantity = Request.Form["quantity" + index];

            //            foreach (CartItem item in CartHelper.CurrentCart.Items)
            //            {
            //                if (item.Item.UniqueProductID == id)
            //                {
            //                    item.Quantity = Convert.ToInt32(quantity);
            //                }
            //            }
            //        }
            //    }
            //    if (CurrentUser == null)
            //    {
            //        CartHelper.CartOwnerID = new Guid(System.Configuration.ConfigurationManager.AppSettings["anonymoususer"]);
            //    }
            //    else
            //    {
            //        CartHelper.CartOwnerID = CurrentUser.ID;
            //    }
            //    Address orderAddress = new Address();
            //    if (Request["address"] == "-1")
            //    {
            //        var habel = Request["phone"];
            //        orderAddress.ID = cMgr.SaveUserAddress(CurrentUser.ID, null, Request["fullName"], Request["addressStreet"], Request["lat"] + "," + Request["lng"], "Jordan", "Amman", string.IsNullOrEmpty(CurrentUser.Phone) ? "790" : CurrentUser.Phone, Request["phone"], Request["currentOption"]);
            //        if (Request["currentOption"] == "home")
            //            cMgr.SetDefaultAddress(CurrentUser.ID, orderAddress.ID);
            //        orderAddress = cMgr.GetUserAddress(CurrentUser.ID, orderAddress.ID);
            //    }
            //    else
            //    {
            //        if (Request["currentOption"] != "collect")
            //            orderAddress = cMgr.GetUserAddress(CurrentUser.ID, Request["address"]);
            //        else
            //            orderAddress = cMgr.GetUserAddress(new Guid("08B14F61-CB44-4125-A00C-B97D19B4E013"), Request["address"]);
            //    }
            //    // old system where single shop

            //    CartHelper.CurrentCart.OrderAddress = orderAddress;
            //    ViewBag.BaseShipping = ConfigurationManager.AppSettings["ShippingPrice"];
            //    CartHelper.CurrentCart.Delivery = DeliveryMethod.PayOnDelivery;

            //    //support free shipping if the total larger than specific limit
            //    double FreeShippingLimit = Convert.ToDouble(ConfigurationManager.AppSettings["OrderTotalLimit"]);
            //    CartHelper.CurrentCart.ShippingPrice = Convert.ToDouble(ConfigurationManager.AppSettings["ShippingPrice"]);

            //    if (CartHelper.CurrentCart.TotalWOShipping >= FreeShippingLimit)
            //        CartHelper.CurrentCart.ShippingPrice = 0;

            //    ViewBag.Title = Resources.Strings.CartReview;
            //    ViewBag.ClientName = CurrentUser.Name;
            //    ViewBag.ClientEmail = CurrentUser.Email;
            //    //CartHelper.CheckOutWithAddress(GatewayType.None);


            //    string SHAInSecret = "HuMaNUnDeaD1984ok";
            //    string test = string.Format("AMOUNT={1}{0}CN={6}{0}CURRENCY={2}{0}EMAIL={7}{0}LANGUAGE={3}{0}ORDERID={4}{0}OWNERADDRESS={8}{0}OWNERCTY={9}{0}{10}OWNERTOWN={11}{0}{12}PSPID={5}{0}", SHAInSecret, CartHelper.CurrentCart.Total * 100, "AED", "en_US", CartHelper.CurrentCart.OrderID, "cubicm", CurrentUser.Name, CurrentUser.Email, CartHelper.CurrentCart.OrderAddress.Address1, "AE", string.IsNullOrEmpty(CartHelper.CurrentCart.OrderAddress.Phone1) ? "" : string.Format("OWNERTELNO={1}{0}", SHAInSecret, CartHelper.CurrentCart.OrderAddress.Phone1), CartHelper.CurrentCart.OrderAddress.City, string.IsNullOrEmpty(CartHelper.CurrentCart.OrderAddress.Postal) ? "" : string.Format("OWNERZIP={1}{0}", SHAInSecret, CartHelper.CurrentCart.OrderAddress.Postal));
            //    ViewBag.SHA = GeneralHelper.GetHashString(test);
            //    double previousBalance = cMgr.GetUserBalance(CurrentUser.ID);
            //    ViewBag.PreviousBalance = previousBalance;
            //    if (previousBalance > 0)
            //        ViewBag.Total = CartHelper.CurrentCart.Total - previousBalance;
            //    else
            //        ViewBag.Total = CartHelper.CurrentCart.Total;
            //    if (Request["usepoints"] == "on")
            //    {
            //        ViewBag.UsePoints = "on";
            //        double poinFactor = Convert.ToDouble(ConfigurationManager.AppSettings["LoyaltyPointFactor"]);
            //        int PointsValue = Convert.ToInt32(cMgr.GetUserPoints(CurrentUser.ID) * poinFactor * GeneralHelper.CurrentCurrencyRatio);
            //        if (PointsValue > ViewBag.Total)
            //        {
            //            PointsValue = (int)Math.Round(ViewBag.Total);
            //        }
            //        ViewBag.UsedPoints = PointsValue / poinFactor;
            //        ViewBag.Total = ViewBag.Total - PointsValue;
            //    }
            //    else
            //    {
            //        ViewBag.UsePoints = "off";
            //    }
            //    CartHelper.CurrentCart.OwnerID = CurrentUser.ID;
            //    return View(CartHelper.CurrentCart);
            //}
        }

        [IsLoggedIn]
        public ActionResult AddToCart(string pid)
        {
            int prodSizeColorID = 0;
            if (!String.IsNullOrEmpty(Request["prodSizeColorID"]))
            {
                 pid = Request["prodSizeColorID"];

            }
            
            int Quantity = Convert.ToInt32(Request["quantity"]);
            if (Quantity == 0)
                Quantity = 1;
            Language curLang = (Language)Session["lang"];
            CommerceManager cmgr = new CommerceManager(ConnectionString);
            SellableProduct prod = cmgr.GetSellableProduct(pid, curLang);
            int cartItemQty = CartHelper.CurrentCart != null && CartHelper.CurrentCart.Items != null && CartHelper.CurrentCart.Items.Find(x => x.ID.ToString() == pid) != null ? CartHelper.CurrentCart.Items.Find(x => x.ID.ToString() == pid).Quantity : 0;
            if (prod.Quantity >= Quantity)
            {
                if (cartItemQty + Quantity <= prod.Quantity)
                {
                    //CartHelper.CurrentCart.Notes = "prodSizeColorID:" + prodSizeColorID+"\n";
                    CartHelper.AddItem(prod, Quantity);
                }
                else
                {
                    ViewBag.Error = Resources.Strings.ItemOutOfStock;
                    return View("_404");
                }
            }
            else if (prod.Quantity >= 1) //the user somehow managed to add more than the avalabile quantity
            {
                if (cartItemQty == 0)
                {
                    //CartHelper.CurrentCart.Notes = "prodSizeColorID:" + prodSizeColorID;
                    CartHelper.AddItem(prod, prod.Quantity);
                }
                else
                {
                    ViewBag.Error = Resources.Strings.ItemOutOfStock;
                    return View("_404");
                }
            }
            else
            { //here the user managed to add an out-of-stock product - as prod will be null
                ViewBag.Error = Resources.Strings.ItemOutOfStock;
                return View("_404");
            }

            return RedirectToAction("ViewCart");
        }


        public ActionResult SetQty(string pid)
        {
            int Quantity = Convert.ToInt32(Request["quantity"]);
            if (Quantity == 0)
                Quantity = 1;
            Language curLang = (Language)Session["lang"];
            CommerceManager cmgr = new CommerceManager(ConnectionString);
            SellableProduct prod = cmgr.GetSellableProduct(pid, curLang);
            var item = CartHelper.CurrentCart.Items.Find(x => x.ID == Convert.ToInt32(pid));
            item.Quantity = Quantity > item.Item.Quantity ? item.Item.Quantity : Quantity;
            return RedirectToAction("ViewCart");
        }

        public ActionResult RemoveFromCart(string id)
        {

            int prodID = Convert.ToInt32(Request["id"]);
            if (prodID == 0)
                CartHelper.RemoveItem(Convert.ToInt32(id));
            else
                CartHelper.RemoveItem(prodID);

            //CartHelper.RemoveItem(prodID);
            //if (CartHelper.CurrentCart.Items.Count == 0)
            //    return Redirect(Url.Content("~/") + "Catalog/Product/" + Request["id"] + "/Product");
            return ViewCart();
        }

        [IsLoggedIn]
        public ActionResult MoveToWishlist()
        {
            int prodID = Convert.ToInt32(Request["id"]);
            CartHelper.RemoveItem(prodID);
            ProductManager pMgr = new ProductManager(ConnectionString);
            if (!pMgr.ProductIsInWishlist(Request["id"], CurrentUser.ID, 1))
            {
                pMgr.ProductAddToWishlist(Request["id"], CurrentUser.ID, 1);
            }
            if (CartHelper.CurrentCart.Items.Count == 0)
                return Redirect(Url.Content("~/") + "Catalog/Product/" + Request["id"] + "/Product");
            return Redirect("ViewCart");
        }



        [IsLoggedIn]
        public double GetShippingPrice(double BaseShipping)
        {
            if (CartHelper.CurrentCart.OrderAddress != null)
            {
                LookupManager lMgr = new LookupManager(ConnectionString);
                double rate = Convert.ToDouble(lMgr.GetLookupItem(Convert.ToInt32(CartHelper.CurrentCart.OrderAddress.City), CurrentLanguage).Value);
                double price = 0;
                double totalWeight = 0;
                foreach (CMS.CartItem item in CartHelper.CurrentCart.Items)
                {
                    CMS.SellableProduct prod = (CMS.SellableProduct)item.Item;
                    double quantity = item.Quantity;
                    double weight = prod.ShippingWeight;
                    totalWeight += quantity * weight;
                }
                totalWeight = Math.Ceiling(totalWeight);
                if (totalWeight <= 5)
                {
                    price = BaseShipping;
                }
                else
                {
                    price = BaseShipping + (totalWeight - 5) * rate;
                }
                return price;
            }
            else
            {
                return 0;
            }
        }

    }
}