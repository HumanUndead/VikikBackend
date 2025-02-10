using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;

public partial class Admin_SubAdmin : KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnBack_Click(object sender, ImageClickEventArgs e)
    {

        Response.Redirect(string.Format("Default.aspx?mod={0}", Request["mod"].Replace("-p","-g")));
    }

    protected void page_Init(object sender, EventArgs e)
    {
        CMS.Config.CMSConfigurations config = CMS.Config.CMSConfigurations.GetSection();
        switch (Request["mod"])
        {
            case "pro-g":
                CMS.Admin.ProductGrid proGrid = new CMS.Admin.ProductGrid();
                phControl.Controls.Add(proGrid);
                ltlTitle.Text = Resources.CMS.Products;
                btnBack.Visible = false;
                break;
            case "pro-p":
                CMS.Admin.ProductPanel proPanel = new CMS.Admin.ProductPanel();
                proPanel.OldPostNumbering = false;
                proPanel.CommerceEnabled = true;
                proPanel.ShippingEnabled = true;
                proPanel.OwnerEnabled = true;
                proPanel.DateEnabled = true;
                phControl.Controls.Add(proPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.ProductAdd : Resources.CMS.ProductEdit);
                btnBack.Visible = true;
                break;
            default:
                Literal ltlWelcome = new Literal();
                ltlWelcome.Text = string.Format("<h1>{0}</h1>{1}", Resources.CMS.CMSTitle, Resources.CMS.CMSWelcome);
                phControl.Controls.Add(ltlWelcome);
                btnBack.Visible = false;
                break;
        }
    }
}