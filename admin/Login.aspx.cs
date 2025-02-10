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
using CMS.Managers;
using CMS.Controls;

public partial class Admin_Login : KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lgnAdmin_Authenticate(object sender, AuthenticateEventArgs e)
    {
        MemberManager memMgr = new MemberManager((string)Application["constr"]);
        LanguageManager lngMgr = new LanguageManager((string)Application["constr"]);
        SQLResult result;
        CMS.User user = memMgr.UserLogin(lgnAdmin.UserName, lgnAdmin.Password, lngMgr.GetLanguage("1"), out result);
        if (user != null)
        {
            Session["User"] = user;
            FormsAuthentication.SetAuthCookie(user.Email, true);
            Response.Redirect(string.IsNullOrEmpty(Request["returnurl"]) ? "default.aspx" : Request["returnurl"]);
        }
        else
        {
            e.Authenticated = false;
            Notify noty = new Notify();
            noty.Message = Resources.CMS.WrongUserPass;
            noty.AlertType = NotifyType.error;
            this.Controls.Add(noty);
        }
    }
}
