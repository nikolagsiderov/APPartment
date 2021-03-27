using APPartment.UI.Services.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.UI.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected BaseWebService BaseWebService;
        protected NotificationService NotificationService;
        protected long? CurrentUserID { get; set; }
        protected long? CurrentHomeID { get; set; }
        protected string CurrentHomeName { get; set; }
        protected string CurrentUserName { get; set; }
        protected string CurrentControllerName { get; set; }
        protected string ImagesPath { get; } = @"wwwroot\BaseObject_Images\";

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor.HttpContext != null && contextAccessor.HttpContext.Session != null)
            {
                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("UserID")))
                {
                    CurrentUserID = long.Parse(contextAccessor.HttpContext.Session.GetString("UserID"));
                    BaseWebService = new BaseWebService(CurrentUserID);
                }
                else
                    BaseWebService = new BaseWebService(0);

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("Username")))
                {
                    CurrentUserName = contextAccessor.HttpContext.Session.GetString("Username");
                }

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("HomeID")))
                {
                    CurrentHomeID = long.Parse(contextAccessor.HttpContext.Session.GetString("HomeID"));
                    NotificationService = new NotificationService(CurrentUserID, CurrentHomeID);
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