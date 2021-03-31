using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APPartment.UI.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected long? CurrentUserID { get; set; }
        protected long? CurrentHomeID { get; set; }
        protected string CurrentHomeName { get; set; }
        protected string CurrentUserName { get; set; }
        protected string CurrentAreaName { get; set; }
        protected string CurrentControllerName { get; set; }
        protected string ImagesPath { get; } = @"wwwroot\BaseObject_Images\";

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor.HttpContext != null && contextAccessor.HttpContext.Session != null)
            {
                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("UserID")))
                {
                    CurrentUserID = long.Parse(contextAccessor.HttpContext.Session.GetString("UserID"));
                }
                else

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("Username")))
                {
                    CurrentUserName = contextAccessor.HttpContext.Session.GetString("Username");
                }

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("HomeID")))
                {
                    CurrentHomeID = long.Parse(contextAccessor.HttpContext.Session.GetString("HomeID"));
                }

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("HomeName")))
                {
                    CurrentHomeName = contextAccessor.HttpContext.Session.GetString("HomeName").ToString();
                }

                CurrentControllerName = this.GetType().Name.Replace("Controller", "");

                var currentControllerCustomAttributes = this.GetType().GetCustomAttributesData();

                if (currentControllerCustomAttributes.Any())
                {
                    var currentControllerAreaAttributes = currentControllerCustomAttributes.Where(x => x.AttributeType.Equals(typeof(AreaAttribute)));

                    if (currentControllerAreaAttributes.Any())
                    {
                        CurrentAreaName = currentControllerAreaAttributes
                            .FirstOrDefault()
                            .ConstructorArguments
                            .FirstOrDefault()
                            .Value
                            .ToString();
                    }
                    else
                        CurrentAreaName = "default";
                }
                else
                    CurrentAreaName = "default";
            }
        }
    }
}