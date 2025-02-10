<%@ Application Language="C#" %>
<%@ Import Namespace="vectorsAdmin" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
        Application["adminconstr"] = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        Application["constr"] = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        Application["PageSize"] = CMS.Config.CMSConfigurations.GetSection().PageSize;
        Application["VisitorsOnline"] = 0;
        System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(
       new CMS.Providers.AssemblyResourceProvider());
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        try
        {
           
        }
        catch
        {
        }

    }

    void Session_Start(object sender, EventArgs e)
    {
        CMS.Managers.LanguageManager langManager = new CMS.Managers.LanguageManager((string)Application["adminconstr"]);
        Session["lang"] = langManager.GetLanguage(CMS.Config.CMSConfigurations.GetSection().DefaultLanguage.ToString());
        Session["SurveyNotified"] = false;
        Session["CompareList"] = new List<CMS.Product>();
        Application.Lock();
        Application["VisitorsOnline"] = ((int)Application["VisitorsOnline"]) + 1;
        Application.UnLock();
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        Application.Lock();
        Application["VisitorsOnline"] = ((int)Application["VisitorsOnline"]) - 1;
        Application.UnLock();
    }

</script>
