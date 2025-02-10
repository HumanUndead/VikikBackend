using CMS;
using CMS.Helpers;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
namespace vikik.Controllers
{
    public class OrderController : KensoftController
    {
        /*public ActionResult OrderPaid()
        {
            
            StringBuilder parameters = new StringBuilder();
            string SHAOutSecret = "DaRkKn!gHt1984ok";
            string[] keys = Request.QueryString.AllKeys;
            Array.Sort(keys, StringComparer.InvariantCulture);
            foreach (string key in keys)
            {
                if (key.ToUpper() != "SHASIGN")
                    parameters.AppendFormat("{0}={1}{2}", key.ToUpper(), Request[key], SHAOutSecret);
            }
            string stringtohash = parameters.ToString();
            string SHA = GeneralHelper.GetHashString(stringtohash);
            if (SHA == Request["SHASIGN"] && Request["status"] == PayfortStatus.PAYMENTREQUESTED.ToString("d"))
            {
                CommerceManager cMgr = new CommerceManager(ConnectionString);
                string orderID = Request["orderID"];
                double amount = Convert.ToDouble(Request["amount"]);
                string paymentID = Request["payid"];
                string cardNo = Request["cardno"];
                string ip = Request["ip"];
                string acceptance = Request["acceptance"];
                Cart completeCart = cMgr.GetCartOrder<SellableProduct>(orderID);
                if (Math.Round(amount) != Math.Round(completeCart.OrderTotal))
                {
                    return View("_404");
                }
                else
                {
                    ViewBag.PaymentReference = paymentID;
                    Transaction previousTransaction = cMgr.GetPendingCartTransaction(completeCart.ID);
                    if (previousTransaction != null && Math.Round(previousTransaction.Amount) == Math.Round(amount))
                    {
                        cMgr.CompleteTransaction(previousTransaction.ID, true);
                        cMgr.AddTransaction(null, CurrentUser.ID, TransactionType.Payment, amount, paymentID, (int)GatewayType.PayFort, string.Format("Card #: {0}, Acceptance: {1}", cardNo, acceptance), previousTransaction.ID, false, completeCart.ID,true);
                        //CartHelper.NewCart();
                        completeCart.ShippingPrice = 3;
                        SendOrderEmail(completeCart);
                        CartHelper.NewCart();
                        return View("Accept", completeCart);
                    }
                    else
                    {
                        return View("_404");
                    }
                }
            }
            else
            {
                return View("_404");
            }
        }*/

       
        public ActionResult Art()
        {
            return View();
        }
        public ActionResult OrderDetails(string orderID)
        {
            ViewBag.Title = Resources.Strings.Client + " | " + Resources.Strings.OrderDetails;
            ViewBag.CurrentUser = CurrentUser;

            CommerceManager cMgr = new CommerceManager(ConnectionString);
            ProductManager pmgr = new ProductManager(ConnectionString);

            ViewBag.Order = cMgr.GetCartOrder<CMS.SellableProduct>(Request["id"]);

            ViewBag.Notes = cMgr.GetCartOrder<CMS.SellableProduct>(Request["id"]).Notes;

            Cart completeCart = cMgr.GetCartOrder<SellableProduct>(Request["id"]);
            //
            List<Product> prods = new List<Product>();
            foreach (var item in completeCart.Items)
            {
                prods.Add(pmgr.GetProduct(item.Item.ProductID.ToString(), CurrentLanguage));
            }
            ViewBag.Prods = prods;
            //
            LookupManager lMgr = new LookupManager(ConnectionString);
            GeneralManager gMgr = new GeneralManager(ConnectionString);
            ViewBag.City = gMgr.CitiesGet(null, CurrentLanguage, "");

            foreach (var item in ViewBag.City)
            {

                if (item.Name == cMgr.GetUserAddress(CurrentUser.ID, completeCart.OrderAddress.ID).City)
                {
                   ViewBag.ShippingPrice = Convert.ToDouble(item.Value);
                    ViewBag.FormattedTotal = Convert.ToDouble(item.Value) + completeCart.TotalWOShipping;
                }
            }
            ViewBag.ProductManager = new ProductManager(ConnectionString);
            ViewBag.CurrentLanguage = CurrentLanguage;
            return View(completeCart);
        }
        [IsLoggedIn]
        public ActionResult UserOrders(int page = 1, int pageSize = 12)
        {
            CommerceManager cmgr = new CommerceManager(ConnectionString);
            MemberManager mmgr = new MemberManager(ConnectionString);
            //ViewBag.Title = Resources.Strings.Client + " | " + Resources.Strings.MyOrders;
            FillSEO(-7);
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.Addresses = cmgr.GetUserAddresses(CurrentUser.ID);
            int totalPages, totalRows;
            Language eng = new Language() { ID = "1" };
            
            ViewBag.UserOrders = cmgr.GetUserOrders(CurrentUser.ID, page, pageSize, out totalPages, out totalRows, CartStatus.Cancelled, CartStatus.Shipped, eng);
            NewsLetterManager nmgr = new NewsLetterManager(ConnectionString);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            return View();
        }
     
        public void SendOrderEmail(Cart cart)
        {
            ProductManager mgr = new ProductManager(ConnectionString);
            StringBuilder items = new StringBuilder("<table border=1 cellpadding=5><tr style=\"background:#f05829;color:#ffffff;\"><th>ID</th><th>Category</th><th>Item</th><th>Color</th><th>Size</th><th>Quantity</th><th>Unit Price</th><th>Total Price</th><th>Notes</th></tr>");
            foreach (CartItem product in cart.Items)
            {
                Product prod = mgr.GetProduct(product.Item.ProductID.ToString(), CurrentLanguage);
                string brandname = string.Empty;
                brandname = prod.BrandName;
                //items.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", prod.ID, prod.CategoryName, product.Name, product.Quantity, product.FormattedUnitPrice, product.FormattedTotalPrice, product.Notes));

                items.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>", prod.ID, prod.CategoryName, product.Name, prod.Color.Label, prod.Size.Label,product.Quantity, product.FormattedUnitPrice, product.FormattedTotalPrice, product.Notes));
            }
            items.Append("</table>");


            CommerceManager cMgr = new CommerceManager(ConnectionString);
            LookupManager lMgr = new LookupManager(ConnectionString);
            GeneralManager gMgr = new GeneralManager(ConnectionString);
            Double ShippingPrice = 0.0;
            foreach (var item in gMgr.CitiesGet(null, CurrentLanguage,""))
            {

                if (item.Name == cMgr.GetUserAddress(CurrentUser.ID, cart.OrderAddress.ID).City)
                {
                   ShippingPrice = Convert.ToDouble(item.Value);
                }
            }
            items.Append(string.Format("<br/><br/><b>Delivery Cost:</b> {0}", ShippingPrice));

            //items.Append(string.Format("<br/><br/><b>Delivery Cost:</b> {0}", cart.FormattedShippingPrice));
            items.Append(string.Format("<br/><b>Total Price:</b> {0}", cart.FormattedOrderTotal));

            MemberManager mMgr = new MemberManager(ConnectionString);
            User user = mMgr.GetUser(cart.OwnerID, CurrentLanguage);
            StringBuilder customerInformation = new StringBuilder("");
            customerInformation.AppendFormat("<table><tr><td><b>Order #</b></td><td>{0}</td><td><b>Date</b></td><td>{1}</td><td></tr></table>", cart.OrderID, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            customerInformation.AppendFormat("<table><tr><th colspan=\"2\">Customer Details</th></tr><tr><td><b>Name</b></td><td>{0}</td></tr><tr><td><b>Email</b></td><td>{1}</td></tr><tr><td><b>Mobile</b></td><td>{2}</td></tr><tr><td colspan=2><b>Address</b></td></tr><tr><td colspan=2>{3}</td></tr><tr><td colspan=2><b>City</b></td></tr><tr><td colspan=2>{4}</td></tr></table>", user.Name, cart.Email, cart.OrderAddress.Phone1 + "<br />" + cart.OrderAddress.Phone2, cart.OrderAddress.Address1 + "<br/>" + cart.OrderAddress.Address2, cart.OrderAddress.City);

            try
            {  //Order/OrderDetails?id=360305
                StringBuilder adminEmail = new StringBuilder("");
                adminEmail.Append(string.Format("You have a new order, to view details please <a href=\"http://www.vikik/order/view/{0}\">click here</a><br/><br/>", cart.ID.ToString(), cart.Delivery == DeliveryMethod.PayOnline ? (int)GatewayType.TwoCO : (int)GatewayType.None));
                adminEmail.Append(customerInformation);
                adminEmail.Append(items);
                if (!string.IsNullOrEmpty(cart.Notes))
                {
                    adminEmail.AppendFormat("<br/><br/><b>Notes</b><br/>", cart.Notes);
                }
             // Helper.SendEmail("site@vikikfashion.com", "rahma_sadder@yahoo.com", "Order Confirmation from vikikfashion.com", adminEmail.ToString());//for testing only remove it once done and use the one above
                Helper.SendEmail("site@vikikfashion.com", "order@vikikfashion.com", "Order Confirmation from vikikfashion.com", adminEmail.ToString());//for testing only remove it once done and use the one above

                StringBuilder deliveryitems = new StringBuilder("<table border=1 cellpadding=5><tr style=\"background:#f05829;color:#ffffff;\"><th>Brand</th><th>Category</th><th>Item</th><th>Quantity</th><th>Notes</th></tr>");
                foreach (CartItem product in cart.Items)
                {
                    Product prod = mgr.GetProduct(product.Item.ProductID.ToString(), CurrentLanguage);
                    string brandname = string.Empty;
                    brandname = prod.BrandName;
                    deliveryitems.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", brandname, prod.CategoryName, product.Name, product.Quantity, product.Notes));
                }
                deliveryitems.Append("</table>");

                StringBuilder DeliveryEmail = new StringBuilder("");
                DeliveryEmail.Append(customerInformation);
                DeliveryEmail.Append(deliveryitems);
                if (!string.IsNullOrEmpty(cart.Notes))
                {
                    DeliveryEmail.AppendFormat("<br/><br/><b>Notes</b><br/>", cart.Notes);
                }
                // Helper.SendEmail("site@vikikfashion.com", "rahma_sadder@yahoo.com", "BILL from vikikfashion.com", DeliveryEmail.ToString());//for testing only remove it once done and use the one above
                Helper.SendEmail("site@vikikfashion.com", "delivery@vikikfashion.com", "BILL from vikikfashion.com", DeliveryEmail.ToString());//for testing only remove it once done and use the one above



                StringBuilder CustomerEmail = new StringBuilder("");
                CustomerEmail.Append(customerInformation);
                CustomerEmail.Append(items);

                if (!string.IsNullOrEmpty(cart.Notes))
                {
                    CustomerEmail.AppendFormat("<br/><br/><b>Notes</b><br/>", cart.Notes);
                }
               
                //Helper.SendEmail("site@vikikfashion.com", "rahma_sadder@yahoo.com", "Invoice from vikikfashion.com", CustomerEmail.ToString());//for testing only remove it once done and use the one above
                Helper.SendEmail("site@vikikfashion.com", cart.Email, "Invoice from vikikfashion.com", CustomerEmail.ToString());//for testing only remove it once done and use the one above

            }
            finally
            {

            }
        }

        public ActionResult PayOnDelivery()
        {
            
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            ProductManager pmgr = new ProductManager(ConnectionString);

            ViewBag.Title = Resources.Strings.Client + " | " + Resources.Strings.OrderDetails;
            string orderID = Request["orderID"];

            Cart completeCart = cMgr.GetCartOrder<SellableProduct>(orderID);

            ViewBag.shippingPrice = CartHelper.CurrentCart.ShippingPrice;
            ViewBag.FormattedTotal = CartHelper.CurrentCart.FormattedTotal;

            double amount = completeCart.OrderTotal;

            LookupManager lMgr = new LookupManager(ConnectionString);
            GeneralManager gMgr = new GeneralManager(ConnectionString);
            ViewBag.City = gMgr.CitiesGet(null, CurrentLanguage, "");
            //skip
            Transaction previousTransaction = cMgr.GetPendingCartTransaction(completeCart.ID);
            
            if (previousTransaction != null && Math.Round(previousTransaction.Amount) == Math.Round(amount))
            {
                double previousBalance = cMgr.GetUserBalance(completeCart.OwnerID);
                ViewBag.PreviousBalance = previousBalance;
                if (previousBalance > 0)
                    ViewBag.Total = CartHelper.CurrentCart.Total - previousBalance;
                else
                    ViewBag.Total = CartHelper.CurrentCart.Total;
                if (Request["usepoints"] == "on")
                {
                    double PointValue = Convert.ToDouble(ConfigurationManager.AppSettings["PointValue"]);
                    double PointsValue = cMgr.GetUserPoints(CurrentUser.ID) * PointValue;
                    if (PointsValue > ViewBag.Total)
                    {
                        PointsValue = (int)Math.Round(ViewBag.Total);
                    }
                    ViewBag.UsedPoints = PointsValue / PointValue;
                    ViewBag.Total = ViewBag.Total - PointsValue;

                    cMgr.RemovePoints(completeCart.OwnerID, ViewBag.UsedPoints,string.Format("User Used points for order {0}",completeCart.OrderID),null); //remove the points
                    cMgr.AddTransaction(null, completeCart.OwnerID, TransactionType.Credit, PointsValue, null, null, "Converting points to purchase credits for order #" + orderID, null, true, completeCart.ID, false, 1); //add the points as credits
                }
                cMgr.CompleteTransaction(previousTransaction.ID, true);
                //cMgr.AddTransaction(null, CurrentUser.ID, TransactionType.Payment, amount, paymentID, (int)GatewayType.PayFort, string.Format("Card #: {0}, Acceptance: {1}", cardNo, acceptance), previousTransaction.ID, false, completeCart.ID);
                //CartHelper.NewCart();
                //completeCart.ShippingPrice = 0;
                SendOrderEmail(completeCart);
                //
                List<Product> prods = new List<Product>();
                foreach (var item in completeCart.Items)
                {
                    prods.Add(pmgr.GetProduct(item.Item.ProductID.ToString(), CurrentLanguage));
                }
                ViewBag.Prods = prods;
                //
                ViewBag.ProductManger = new ProductManager(ConnectionString);
                ViewBag.CurrentLanguage = CurrentLanguage;
                CartHelper.NewCart();
                return View("DeliveryPay", completeCart);
            }
            else
            {
                return View("_404");
            }

        }
  

        public ActionResult StartPay(string startToken, string startEmail)
        {
            /*
            //var card=JsonConvert.DeserializeObject<PayfortCard>(startToken);
            string url = "https://api.start.payfort.com/charges";
            string data = string.Format("amount={0}&currency={1}&email={2}&card={3}&description={4}", 
                Request["amount"],"JD", startEmail,startToken,"Cart order with cubicM");

            /*WebRequest myReq = WebRequest.Create(url);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            myReq.Method = "POST";
            myReq.ContentLength = data.Length;
            //myReq.ContentType = "application/json; charset=UTF-8";

            string usernamePassword = "test_sec_k_af587d59b88ca12b55293:" + ":" + "";

            UTF8Encoding enc = new UTF8Encoding();

            myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(enc.GetBytes(usernamePassword)));


            using (Stream ds = myReq.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }


            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            Response.Write(content);*/
            /*
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("amount", Request["amount"]),new KeyValuePair<string, string>("currency", "JD"),new KeyValuePair<string, string>("email", startEmail),
                new KeyValuePair<string, string>("card", startToken),new KeyValuePair<string, string>("description", "Cart order with cubicM")
            };

            var content = new FormUrlEncodedContent(pairs);

            var client = new HttpClient(); //{ BaseAddress = new Uri(url) };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(
           String.Format("{0}:{1}", "test_sec_k_af587d59b88ca12b55293", "")));
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", encoded);
            // call sync
            var response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                
            }
           */
            return View();
        }

        double SingleCartTotal(List<CartItem> cart)
        {
            double amount = 0;

            foreach (var item in cart)
            {
                amount += item.TotalPrice;
            }

            return amount;
        }

        void FillSEO(int pageID)
        {
            StaticPageManager pmgr = new StaticPageManager(ConnectionString);
            StaticPage page = pmgr.GetStaticPage(pageID, CurrentLanguage);
            ViewBag.Title = page.Title;
            ViewBag.Description = page.Description;
        }
    }
}