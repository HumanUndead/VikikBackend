using CMS;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Orders
/// </summary>
public partial class Orders : KensoftPage
{
    public int PageNumber
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int num = 1;
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
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
        if (!string.IsNullOrEmpty(base.Request["pnum"]))
        {
            num = Convert.ToInt32(base.Request["pnum"]);
        }
        this.PageNumber = num;
        int num2;
        if (!string.IsNullOrEmpty(base.Request["uid"]))
        {
            int num3;
            this.repOrders.DataSource = commerceManager.GetUserOrders(new Guid(base.Request["uid"]), num, 30, out num2, out num3, pMinStatus, pMaxStatus, base.CurrentLanguage);
        }
        else
        {
            int num3;
            this.repOrders.DataSource = commerceManager.GetOrders(num, 30, out num2, out num3, pMinStatus, pMaxStatus, base.CurrentLanguage);
        }
        this.repOrders.DataBind();
        this.liPrev.Visible = (this.PageNumber > 1);
        this.liNext.Visible = (this.PageNumber < num2);
        List<int> list = new List<int>();
        for (int i = 1; i <= num2; i++)
        {
            list.Add(i);
        }
        this.repPages.DataSource = list;
        this.repPages.DataBind();
    }
}