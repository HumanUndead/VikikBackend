using CMS;
using CMS.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace vikik.Controllers
{
    public class HandlersController : KensoftController
    {
        // GET: Handlers
        public ActionResult AddressAdd(string id,string addressLabel, string address1, string address2, string phone1, string phone2, string postal, string defaultAddress, string country = "Jordan", string city = "Amman")
        {
            CommerceManager cMgr = new CommerceManager(ConnectionString);
            LookupManager lMgr = new LookupManager(ConnectionString);
            string addressID=cMgr.SaveUserAddress(CurrentUser.ID, id, addressLabel, address1,address2,country,city, string.IsNullOrEmpty( phone1) ? CurrentUser.Phone : phone1, phone2,postal);
            double rate = 0;

            //string ratestr = lMgr.GetLookupItem(Convert.ToInt32(city), CurrentLanguage).Value;
            //if(!string.IsNullOrEmpty(ratestr))
            //{
            //    rate = Convert.ToDouble(ratestr);
            //}
            return RedirectToAction("ViewCart", "Cart");
            //Dictionary<string, object> result = new Dictionary<string, object>();
            //result.Add("ID", addressID);
            //ViewBag.Result = JsonConvert.SerializeObject(result);
            //if (Request["defaultAddress"] == "on")
            //    cMgr.SetDefaultAddress(CurrentUser.ID, id);
            //return View("_Result");




        }
        public ActionResult Products(string id, int page, string sort, string sch)
        {
            int Count;
            string sortby = string.Empty;
            bool asc = false;
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sort;
            }
            ProductManager pMgr = new ProductManager(ConnectionString);
            //ViewBag.Products = pMgr.GetProducts(id, null, CurrentLanguage, page, 30, out Count, sch, true, sortby, asc);
            if (string.IsNullOrEmpty(sch))
                return View();
            else
                return View("clearance");
        }

        public ActionResult Clearance(int page, string sort)
        {
            int Count;
            string sortby = string.Empty;
            bool asc = false;
            if (!string.IsNullOrEmpty(Request["s"]))
            {
                string[] sorting = Request["s"].Split((":").ToCharArray());
                sortby = sorting[0];
                asc = sorting[1] == "a";
                ViewBag.SortBy = sort;
            }
            ProductManager pMgr = new ProductManager(ConnectionString);
            ViewBag.Products = pMgr.GetProductsOnSale(null, null, CurrentLanguage, page, 30, out Count, null, true, sortby, asc);
            return View();
        }
        public ActionResult ShopsSearch(string query)
        {
            DirectoryManager dMgr = new DirectoryManager(ConnectionString);
            List<KeyValuePair<string,string>> suggestions=dMgr.GetOutletSuggestions(query);
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("\"query\":\"{0}\",",query);
            sb.Append("\"suggestions\": [");
            int i = 0;
            foreach(KeyValuePair<string, string> sug in suggestions)
            {
                if(i>0)
                {
                    sb.Append(",");
                }
                sb.AppendFormat("{{ \"value\": \"{0}\", \"data\": \"{1}\" }}",sug.Value,sug.Key);
                i++;
            }
            sb.Append("]}");
            ViewBag.Result = sb.ToString();
            return View("_result");
            
        }
    }
}