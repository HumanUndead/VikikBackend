using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS;

public partial class Admin_Admin : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            Response.Redirect("~/login.aspx");
        }
        bool allowed = true;
        if (Membership.GetUser(((User)Session["User"]).UserName) == null)
        {
            Response.Redirect("~/login.aspx");
        }
        Page.Title = Resources.CMS.AdminTitle;
        if (Membership.GetUser(((User)Session["User"]).UserName) != null)
        {
            if (!((CMS.Security.CMSRolesProvider)Roles.Provider).IsUserInRole(((User)Session["User"]).UserName, "Admin"))
            {
                Response.Redirect("~/login.aspx");
            }
            ltlLoginName.Text = Membership.GetUser(((User)Session["User"]).UserName).UserName;
        }
    }

    protected void sitemenu_DataBound(object sender, EventArgs e)
    {
        /*for (int i = 0; i < sitemenu.Items.Count; i++)
        {
            MenuItem item = sitemenu.Items[i];
            SiteMapNode currentNode = SiteMap.Provider.FindSiteMapNode(item.DataPath);
            bool allowed = true;
            foreach (string role in currentNode.Roles)
            {
                allowed = false;
                if (((CMS.Security.CMSRolesProvider)Roles.Provider).IsUserInRole(((User)Session["User"]).UserName, role) || ((CMS.Security.CMSRolesProvider)Roles.Provider).IsUserInRole(((User)Session["User"]).UserName, "Admin"))
                {
                    allowed = true;
                    break;
                }
            }
            if (allowed)
            {
                TrimMenuItems(item);
            }
            if (!allowed)
            {
                sitemenu.Items.Remove(item);
                i--; //return the removed item index
            }
        }*/
    }

    private void TrimMenuItems(MenuItem mainItem)
    {
        for (int i = 0; i < mainItem.ChildItems.Count; i++)
        {
            MenuItem item = mainItem.ChildItems[i];
            SiteMapNode currentNode = SiteMap.Provider.FindSiteMapNode(item.DataPath);
            bool allowed = true;
            foreach (string role in currentNode.Roles)
            {
                allowed = false;
                if (((CMS.Security.CMSRolesProvider)Roles.Provider).IsUserInRole(((User)Session["User"]).UserName, role) || ((CMS.Security.CMSRolesProvider)Roles.Provider).IsUserInRole(((User)Session["User"]).UserName, "Admin"))
                {
                    allowed = true;
                    break;
                }
            }
            if (allowed)
            {
                if (currentNode["mm"] != null && !Convert.ToBoolean(currentNode["mm"]))
                {
                    mainItem.ChildItems.Remove(item);
                    i--;
                }
                else
                {
                    TrimMenuItems(item);
                }
            }
            if (!allowed)
            {
                mainItem.ChildItems.Remove(item);
                i--;
            }
        }
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Session["User"] = null;
        Response.Redirect(Request.RawUrl);
    }



    #region Main Menu Construction
    /// <summary>
    /// The menu construction is based on the site map, it add the root node as the first element along with its direct children,
    /// then it traverses the other levels as they appear in the site map.
    /// <remarks>
    /// Any sitemap node without a "mm"(main menu) attribute will be ignored.
    /// </remarks>
    /// </summary>
    /// <returns></returns>
    private MenuItemCollection BuildMasterMenu()
    {
        MenuItemCollection menuItems = new MenuItemCollection();
        SiteMapNode Root;
        // add the root node as first element - usually 'Home'.
        if ((Root = SiteMap.Providers["xmlsitemapprovider"].RootNode)["hidden"] == null)
            menuItems.Add(CreateItem(Root));
        AddItems(menuItems, SiteMap.Providers["xmlsitemapprovider"].RootNode);
        return menuItems;
    }
    private void AddItems(MenuItemCollection parentLevel, SiteMapNode parentNode)
    {
        foreach (SiteMapNode node in parentNode.ChildNodes)
        {
            //ignore items not marked as part of the main menu.
            if (node["mm"] == null) continue;
            MenuItem item = CreateItem(node);
            parentLevel.Add(item);
            AddItems(item.ChildItems, node);
        }
    }
    private MenuItem CreateItem(SiteMapNode node)
    {
        MenuItem item = new MenuItem();
        item.Text = node.Title;
        item.NavigateUrl = node.Url;
        item.Target = "_self";
        return item;
    }
    #endregion
}
