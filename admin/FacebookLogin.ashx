<%@ WebHandler Language="C#" Class="FacebookLogin" %>

using System;
using System.Web;

public class FacebookLogin : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        var accessToken = context.Request["pageaccessToken"];
        context.Session["AccessToken"] = accessToken;

        context.Response.Redirect("default.aspx");
    }

    public bool IsReusable
    {
        get { return false; }
    }
}