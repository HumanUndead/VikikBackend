using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CMS;
using CMS.Managers;

public partial class Admin_UsersExporter : KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int dummy;
        CMS.Managers.MemberManager Manager = new CMS.Managers.MemberManager(ConnectionString);
        /*GridView gvUsers = new GridView();
        gvUsers.DataSource = ;
        gvUsers.DataBind();
        //ExportHelper.ExportGridView(gvUsers, "IcopSubscribers.xls", ExportType.Excel);*/
        ExportHelper.ExportToExcelNew(Manager.GetUsersDataSet(CurrentLanguage, 1, 500000, out dummy, string.Empty), "IACusers.xls");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //important so an error wont happen
    }
}
