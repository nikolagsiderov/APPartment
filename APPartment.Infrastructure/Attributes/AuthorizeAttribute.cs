﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace APPartment.Infrastructure.Attributes
{
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var currentUserID = filterContext.HttpContext.Session.GetString("CurrentUserID");
            var currentHomeID = filterContext.HttpContext.Session.GetString("CurrentHomeID");

            if (string.IsNullOrEmpty(currentUserID))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    area = "default",
                    controller = "Account",
                    action = "Login"
                }));
            }
            else if (string.IsNullOrEmpty(currentHomeID))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    area = "default",
                    controller = "Home",
                    action = "Login"
                }));
            }
        }
    }
}
