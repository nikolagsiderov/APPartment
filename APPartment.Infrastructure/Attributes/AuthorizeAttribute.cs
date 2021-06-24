using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace APPartment.Infrastructure.Attributes
{
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (SkipAuthorization(filterContext) == false)
            {
                var currentUserID = filterContext.HttpContext.Session.GetString("CurrentUserID");
                var currentHomeID = filterContext.HttpContext.Session.GetString("CurrentHomeID");

                if (string.IsNullOrEmpty(currentUserID))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        area = "Account",
                        controller = "Account",
                        action = "Login"
                    }));
                }
                else if (string.IsNullOrEmpty(currentHomeID))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        area = "Home",
                        controller = "Home",
                        action = "Login"
                    }));
                }
            }
        }

        private static bool SkipAuthorization(ActionExecutingContext filterContext)
        {
            Contract.Assert(filterContext != null);
            return (filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}
