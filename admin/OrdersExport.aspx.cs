using CMS;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for OrdersExport
/// </summary>
public partial class OrdersExport: KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        LookupManager lookupManager = new LookupManager(base.ConnectionString);
        CartStatus pMinStatus = CartStatus.New;
        CartStatus pMaxStatus = CartStatus.Shipped;
        if (!string.IsNullOrEmpty(base.Request["min"]))
        {
            pMinStatus = (CartStatus)Convert.ToInt32(base.Request["min"]);
        }
        if (!string.IsNullOrEmpty(base.Request["max"]))
        {
            pMaxStatus = (CartStatus)Convert.ToInt32(base.Request["max"]);
        }
        new GridView();
        DataTable dataTable = new DataTable("ShipmentsData");
        dataTable.Columns.AddRange(new DataColumn[]
        {
            new DataColumn("CONSIGNORREF", typeof(string)),
            new DataColumn("CONSIGNEE", typeof(string)),
            new DataColumn("CONSIGNEEADDRESS", typeof(string)),
            new DataColumn("CONSIGNEETOWN", typeof(string)),
            new DataColumn("CONSIGNEEZIPCODE", typeof(string)),
            new DataColumn("CONSIGNEECOUNTRY", typeof(string)),
            new DataColumn("CONSIGNEETELEPHONE", typeof(string)),
            new DataColumn("CONSIGNEEMOBILE", typeof(string)),
            new DataColumn("CONSIGNEEFAX", typeof(string)),
            new DataColumn("CONSIGNEEEMAILADDRESS", typeof(string)),
            new DataColumn("CONSIGNEEATTENTION", typeof(string)),
            new DataColumn("DESTINATIONCODE", typeof(string)),
            new DataColumn("PIECES", typeof(string)),
            new DataColumn("TOTALWEIGHT", typeof(string)),
            new DataColumn("TOTALVOLWEIGHT", typeof(string)),
            new DataColumn("CURRENCY", typeof(string)),
            new DataColumn("VALUEAMT", typeof(string)),
            new DataColumn("TYPEOFSHIPMENT", typeof(string)),
            new DataColumn("CONTENTS", typeof(string)),
            new DataColumn("SERVICE", typeof(string)),
            new DataColumn("COD", typeof(string))
        });
        List<Cart> list;
        if (!string.IsNullOrEmpty(base.Request["uid"]))
        {
            int num;
            list = commerceManager.GetUserOrders(new Guid(base.Request["uid"]), 1, 2147483647, out num, out num, pMinStatus, pMaxStatus, base.CurrentLanguage);
        }
        else
        {
            int num;
            list = commerceManager.GetOrders(1, 2147483647, out num, out num, pMinStatus, pMaxStatus, base.CurrentLanguage);
        }
        foreach (Cart current in list)
        {
            dataTable.Rows.Add(new object[]
            {
                current.OrderID,
                current.OwnerName,
                current.OrderAddress.Address1 + " " + current.OrderAddress.Address2,
                lookupManager.GetLookupItem(Convert.ToInt32(current.OrderAddress.City), base.CurrentLanguage).Name,
                current.OrderAddress.Postal,
                "UAE",
                current.OrderAddress.Phone1,
                string.IsNullOrEmpty(current.OrderAddress.Phone2) ? current.OrderAddress.Phone1 : current.OrderAddress.Phone2,
                null,
                current.Email,
                null,
                null,
                current.ItemsCount,
                current.TotalWeight,
                current.TotalVolWeight,
                "AED",
                current.OrderTotal,
                "NON DOCS",
                null,
                "COURIER",
                (current.Delivery == DeliveryMethod.PayOnDelivery) ? ("USWS |COD|AED|" + current.OrderTotal) : string.Empty
            });
        }
        ExportHelper.ExportToExcelNew(new DataSet
        {
            Tables =
            {
                dataTable
            }
        }, "Orders" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls");
    }
}