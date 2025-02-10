using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CMS;
using CMS.Managers;

namespace vikik
{
    public class IsLoggedInAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] == null /*&& string.IsNullOrEmpty(filterContext.HttpContext.Request["rt"]) /*|| string.IsNullOrEmpty( filterContext.HttpContext.Request.Cookies["User"]["id"])*/)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Profile", controller = "Member", rt = HttpUtility.UrlEncode(filterContext.HttpContext.Request.Path + "?" + filterContext.HttpContext.Request.QueryString) }));
            }
            //else
            //{
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Member", rt = HttpUtility.UrlEncode(filterContext.HttpContext.Request.Path + "?" + filterContext.HttpContext.Request.QueryString) }));
            //}
            
        }
    }
}