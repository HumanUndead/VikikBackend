using CMS;
using CMS.Config;
using CMS.Controls;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OrderDetails
/// </summary>
public partial class OrderDetails : KensoftPage
{
    bool PointsEnabled = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(base.Request["id"]);
        this.ltlOrderID.Text = cartOrder.OrderID;
        this.ltlOrderDate.Text = cartOrder.OrderDate.Value.ToString("dd/MM/yyyy HH:mm");
        this.ltlStatus.Text = CMS.EnumTranslator.ResourceManager.GetString(cartOrder.Status.ToString());
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
        if(isNumeric)
            this.ltlCity.Text = lookupManager.GetLookupItem(Convert.ToInt32(cartOrder.OrderAddress.City), base.CurrentLanguage).Name;
        else
            this.ltlCity.Text = cartOrder.OrderAddress.City;
        isNumeric = int.TryParse(cartOrder.OrderAddress.Country, out n);
        if(isNumeric)
            this.ltlCountry.Text = lookupManager.GetLookupItem(Convert.ToInt32(cartOrder.OrderAddress.Country), base.CurrentLanguage).Name;
        else
            this.ltlCountry.Text = cartOrder.OrderAddress.Country;


        this.ltlItemCount.Text = cartOrder.ItemsCount.ToString();
        this.ltlOrderTotal.Text = cartOrder.OrderTotal.ToString();
        this.ltlPayment.Text = CMS.EnumTranslator.ResourceManager.GetString(cartOrder.Delivery.ToString());
        if (cartOrder.Status > CartStatus.Paid)
        {
            this.lnkTrack.HRef = string.Format("http://iskynettrack.skynetwwe.info/ShipmentTrackSingle.aspx?textfield={0}&radiobutton=SB", cartOrder.TrackingNo);
            this.lnkTrack.Visible = true;
        }
        switch (cartOrder.Status)
        {
            case CartStatus.New:
            case CartStatus.Paid:
                this.lnkPackOrder.Visible = true;
                return;
            case CartStatus.Pending:
                break;
            case CartStatus.ReadyToShip:
                this.lnkShipped.Visible = true;
                return;
            case CartStatus.Shipped:
                this.lnkComplete.Visible = true;
                break;
            default:
                return;
        }
    }

    protected void btnSaveTracking_Click(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(base.Request["id"]);
        commerceManager.SaveCartTracking(cartOrder.ID, this.txtTracking.Text);
        commerceManager.UpdateCartStatus(cartOrder.ID, CartStatus.ReadyToShip);
        base.TransactionSuccess("Order Packed Successfully", "orderprint?id=" + base.Request["id"]);
    }

    protected void lnkComplete_ServerClick(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(base.Request["id"]);
        commerceManager.UpdateCartStatus(cartOrder.ID, CartStatus.Complete);
        if (PointsEnabled)
        {
            int points = Convert.ToInt32(Math.Round(cartOrder.TotalWOShipping * 1.5 / 10));
            commerceManager.AddPoints(cartOrder.OwnerID, points);
        }
        base.TransactionSuccess("Order Completed Successfully", "orders");
    }

    protected void lnkShipped_ServerClick(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(base.Request["id"]);
        commerceManager.UpdateCartStatus(cartOrder.ID, CartStatus.Shipped);
        base.TransactionSuccess("Order Shipped Successfully", "orders");
    }

    protected void lnkCancel_ServerClick(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(base.Request["id"]);
        commerceManager.AddTransaction(null, cartOrder.OwnerID, TransactionType.Cancel, cartOrder.OrderTotal, null, null, "Order Cancelled by " + base.CurrentUser.Name + " IP: " + base.Request.ServerVariables["REMOTE_ADDR"], cartOrder.TransactionID, false, new Guid?(cartOrder.ID), CMSConfigurations.GetSection().StockEnabled,1);
        base.TransactionSuccess("Order Cancelled Successfully", "orders");
    }

    protected void lnkRefund_ServerClick(object sender, EventArgs e)
    {
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        Cart cartOrder = commerceManager.GetCartOrder<SellableProduct>(base.Request["id"]);
        if ((cartOrder.Delivery == DeliveryMethod.PayOnline && cartOrder.Status >= CartStatus.Paid) || (cartOrder.Delivery == DeliveryMethod.PayOnDelivery && cartOrder.Status >= CartStatus.Complete))
        {
            commerceManager.AddTransaction(null, cartOrder.OwnerID, TransactionType.Refund, cartOrder.OrderTotal, null, null, "Order refunded by " + base.CurrentUser.Name + " IP: " + base.Request.ServerVariables["REMOTE_ADDR"], cartOrder.TransactionID, false, new Guid?(cartOrder.ID), CMSConfigurations.GetSection().StockEnabled,1);
            base.TransactionSuccess("Order Refunded Successfully", "orders");
            if (PointsEnabled)
            {
                int points = Convert.ToInt32(Math.Round(cartOrder.TotalWOShipping * 2.5));
                commerceManager.RemovePoints(cartOrder.OwnerID, points);
            }
            return;
        }
        Notify notify = new Notify();
        notify.Message = "Order cannot be refunded";
        notify.AlertType = NotifyType.warning;
        this.Controls.Add(notify);
    }
}