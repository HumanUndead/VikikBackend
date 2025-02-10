using CMS;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommerceUsers
/// </summary>
public partial class CommerceUsers : KensoftPage
{
    public int PageNumber
    {
        get;
        set;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        int num = 1;
        if (!string.IsNullOrEmpty(base.Request["pnum"]))
        {
            num = Convert.ToInt32(base.Request["pnum"]);
        }
        PageNumber = num;
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        int num2;
        this.repUsers.DataSource = commerceManager.GetUsersWithBalance(base.CurrentLanguage, num, 30, out num2, this.Page.Request.QueryString["srch"]);
        this.repUsers.DataBind();
        this.liPrev.Visible = (PageNumber > 1);
        this.liNext.Visible = (PageNumber < num2);
        List<int> list = new List<int>();
        for (int i = 1; i <= num2; i++)
        {
            list.Add(i);
        }
        this.repPages.DataSource = list;
        this.repPages.DataBind();
    }
}