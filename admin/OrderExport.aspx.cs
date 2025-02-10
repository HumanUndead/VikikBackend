using CMS;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for OrderExport
/// </summary>
public partial class OrderExport : KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        LookupManager lookupManager = new LookupManager(base.ConnectionString);
        Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(base.Request["id"]);
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
        dataTable.Rows.Add(new object[]
        {
            cartOrder.OrderID,
            cartOrder.OwnerName,
            cartOrder.OrderAddress.Address1 + " " + cartOrder.OrderAddress.Address2,
            lookupManager.GetLookupItem(Convert.ToInt32(cartOrder.OrderAddress.City), base.CurrentLanguage).Name,
            cartOrder.OrderAddress.Postal,
            "UAE",
            cartOrder.OrderAddress.Phone1,
            string.IsNullOrEmpty(cartOrder.OrderAddress.Phone2) ? cartOrder.OrderAddress.Phone1 : cartOrder.OrderAddress.Phone2,
            null,
            cartOrder.Email,
            null,
            null,
            cartOrder.ItemsCount,
            cartOrder.TotalWeight,
            cartOrder.TotalVolWeight,
            "AED",
            cartOrder.OrderTotal,
            "NON DOCS",
            null,
            "COURIER",
            (cartOrder.Delivery == DeliveryMethod.PayOnDelivery) ? ("USWS|COD|AED|" + cartOrder.OrderTotal) : string.Empty
        });
        ExportHelper.ExportToExcelNew(new DataSet
        {
            Tables =
            {
                dataTable
            }
        }, "Order" + cartOrder.OrderID + ".xls");
    }
}