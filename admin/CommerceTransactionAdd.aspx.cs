using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS;
using CMS.Managers;
using CMS.Helpers;
using CMS.Config;

/// <summary>
/// Summary description for CommerceTransactionAdd
/// </summary>
public partial class CommerceTransactionAdd : KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            this.dpDate.SelectedDate = new DateTime?(DateTime.Now);
            this.ddlTransType.DataSource = GeneralHelper.GetEnumAsData(typeof(TransactionType)).FindAll((KeyValuePair<int, string> x) => x.Key > 0);
            this.ddlTransType.DataTextField = "value";
            this.ddlTransType.DataValueField = "key";
            this.ddlTransType.DataBind();
        }
        Guid pUserID = new Guid(base.Request["uid"]);
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        this.ltlBalance.Text = commerceManager.GetUserBalance(pUserID).ToString();
        MemberManager memberManager = new MemberManager(base.ConnectionString);
        User user = memberManager.GetUser(pUserID, base.CurrentLanguage);
        this.ltlUserName.Text = user.Name;
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        Guid pUserID = new Guid(base.Request["uid"]);
        CommerceManager commerceManager = new CommerceManager(base.ConnectionString);
        commerceManager.AddTransaction(null, pUserID, (TransactionType)Convert.ToInt32(this.ddlTransType.SelectedValue), Convert.ToDouble(this.txtAmount.Text), null, null, this.txtNotes.Text, null, true, null, CMSConfigurations.GetSection().StockEnabled,1);
        base.TransactionSuccess("Transaction added successfully", "commerceusertransactions?id=" + pUserID.ToString());
    }
}