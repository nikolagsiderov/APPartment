using APPartment.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected long? CurrentUserId { get; set; }
        protected long? CurrentHomeId { get; set; }
        protected string CurrentHomeName { get; set; }
        protected string CurrentUserName { get; set; }
        protected string CurrentControllerName { get; set; }
        protected string ImagesPath { get; } = @"wwwroot\BaseObject_Images\";

        public BaseController(IHttpContextAccessor contextAccessor, DataAccessContext context)
        {
            if (contextAccessor.HttpContext != null && contextAccessor.HttpContext.Session != null)
            {
                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("UserId")))
                {
                    CurrentUserId = long.Parse(contextAccessor.HttpContext.Session.GetString("UserId"));
                }

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("Username")))
                {
                    CurrentUserName = contextAccessor.HttpContext.Session.GetString("Username");
                }

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("HomeId")))
                {
                    CurrentHomeId = long.Parse(contextAccessor.HttpContext.Session.GetString("HomeId"));
                }

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("HomeName")))
                {
                    CurrentHomeName = contextAccessor.HttpContext.Session.GetString("HomeName").ToString();
                }

                if (ControllerContext.RouteData != null)
                {
                    CurrentControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                }
            }
        }
    }
}