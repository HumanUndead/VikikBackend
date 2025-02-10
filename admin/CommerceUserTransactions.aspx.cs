using CMS;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommerceUserTransactions
/// </summary>
public partial class CommerceUserTransactions : KensoftPage
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
        Guid pUserID = new Guid(base.Request["id"]);
        this.PageNumber = num;
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        int num2;
        this.repTransactions.DataSource = commerceManager.GetUserTransctions(pUserID, num, 30, out num2);
        this.repTransactions.DataBind();
        this.liPrev.Visible = (this.PageNumber > 1);
        this.liNext.Visible = (this.PageNumber < num2);
        List<int> list = new List<int>();
        for (int i = 1; i <= num2; i++)
        {
            list.Add(i);
        }
        this.repPages.DataSource = list;
        this.repPages.DataBind();
        this.ltlBalance.Text = commerceManager.GetUserBalance(pUserID).ToString();
        MemberManager memberManager = new MemberManager(base.ConnectionString);
        User user = memberManager.GetUser(pUserID, base.CurrentLanguage);
        this.ltlUserName.Text = user.Name;
    }
}