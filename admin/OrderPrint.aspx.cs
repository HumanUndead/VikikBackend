using CMS;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OrderPrint
/// </summary>
public partial class OrderPrint:KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(base.Request["id"]);
        this.ltlOrderID.Text = cartOrder.OrderID;
        this.ltlOrderDate.Text = cartOrder.OrderDate.Value.ToString("dd/MM/yyyy HH:mm");
        this.repItems.DataSource = cartOrder.Items;
        this.repItems.DataBind();
        this.ltlAddress1.Text = cartOrder.OrderAddress.Address1;
        this.ltlAddress2.Text = cartOrder.OrderAddress.Address2;
        this.ltlPhone1.Text = cartOrder.OrderAddress.Phone1;
        this.ltlPhone2.Text = cartOrder.OrderAddress.Phone2;
        this.ltlPostal.Text = cartOrder.OrderAddress.Postal;
        this.ltlOwnerName.Text = cartOrder.OwnerName;
        this.ltlEmail.Text = cartOrder.Email;
        LookupManager lookupManager = new LookupManager(base.ConnectionString);
        int n;
        bool isNumeric = int.TryParse(cartOrder.OrderAddress.City, out n);

        if (isNumeric)
            this.ltlCity.Text = lookupManager.GetLookupItem(Convert.ToInt32(cartOrder.OrderAddress.City), base.CurrentLanguage).Name;
        else
            this.ltlCity.Text = cartOrder.OrderAddress.City;
        isNumeric = int.TryParse(cartOrder.OrderAddress.Country, out n);
        if (isNumeric)
            this.ltlCountry.Text = lookupManager.GetLookupItem(Convert.ToInt32(cartOrder.OrderAddress.Country), base.CurrentLanguage).Name;
        else
            this.ltlCountry.Text = cartOrder.OrderAddress.Country;

        this.ltlItemCount.Text = cartOrder.ItemsCount.ToString();
        this.ltlOrderTotal.Text = cartOrder.OrderTotal.ToString();
        this.ltlPayment.Text = CMS.EnumTranslator.ResourceManager.GetString(cartOrder.Delivery.ToString());
        ltlShipment.Text = cartOrder.TrackingNo;
    }
}