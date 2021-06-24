using APPartment.Infrastructure.Services.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APPartment.Infrastructure.Controllers.Web
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

        protected APPI APPI { get; set; }

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor.HttpContext != null && contextAccessor.HttpContext.Session != null)
            {
                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("CurrentUserID")))
                    CurrentUserID = long.Parse(contextAccessor.HttpContext.Session.GetString("CurrentUserID"));

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("CurrentUsername")))
                    CurrentUserName = contextAccessor.HttpContext.Session.GetString("CurrentUsername");

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("CurrentHomeID")))
                    CurrentHomeID = long.Parse(contextAccessor.HttpContext.Session.GetString("CurrentHomeID"));

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("CurrentHomeName")))
                    CurrentHomeName = contextAccessor.HttpContext.Session.GetString("CurrentHomeName").ToString();

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
                        CurrentAreaName = "Home";
                }
                else
                    CurrentAreaName = "Home";

                APPI = new APPI(CurrentUserID, CurrentHomeID, CurrentControllerName, CurrentAreaName);
            }
        }
    }
}