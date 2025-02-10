using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Configuration;
namespace vikik
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new MenuActionFilter());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["constr"] = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Application["AdminID"] = new Guid("649470DD-2A5F-41C1-934D-03774B56EBCA");

        }
        void Session_Start(object sender, EventArgs e)
        {
            CMS.Managers.LanguageManager langManager = new CMS.Managers.LanguageManager((string)Application["constr"]);
            Session["lang"] = langManager.GetLanguage(CMS.Config.CMSConfigurations.GetSection().DefaultLanguage.ToString());
            CMS.Helpers.CartHelper.CurrentCart.ShippingPrice = Convert.ToDouble(ConfigurationManager.AppSettings["ShippingPrice"]);
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}
