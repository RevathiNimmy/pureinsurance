using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thinktecture.IdentityServer.Web.GlobalFilter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PreventFromUrlFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToString();
            //       if (filterContext.HttpContext.Request.UrlReferrer == null ||
            //filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            //       {
            //           filterContext.Result = new RedirectToRouteResult(new
            //                                     RouteValueDictionary(new { controller = "Account", action = "SignIn", area = "" }));
            //       }

            try
            {

                if (filterContext.HttpContext.Request.UrlReferrer != null)
                {
                    
                    var queryUrl = filterContext.HttpContext.Request.UrlReferrer.Query;
                    if (queryUrl.Contains("%3C") || queryUrl.Contains("%3E"))
                    {
                       
                        var routeValueDictionary = new RouteValueDictionary
                        (
                            new
                            {
                                action ="Error",
                                controller = controllerName
                            }
                        );
                       
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValueDictionary));
                        base.OnActionExecuting(filterContext);
                    }
                }
            }
            catch (System.Web.HttpRequestValidationException exception)
            {
                
                var routeValueDictionary = new RouteValueDictionary
                       (
                           new
                           {
                               action = "Error",
                               controller = controllerName
                           }
                       );
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValueDictionary));
            }
        }

    }
}