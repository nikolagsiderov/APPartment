using Microsoft.AspNetCore.Mvc;
using APPartment.Infrastructure.Services.Base;
using Microsoft.AspNetCore.Http;
using System.Linq;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;

namespace APPartment.Infrastructure.Controllers.Api
{
    [ApiController]
    public abstract class BaseAPIController : ControllerBase
    {
        protected long? CurrentUserID = null;
        protected long? CurrentHomeID = null;
        protected string CurrentController { get; set; }
        protected string CurrentArea { get; set; }

        protected BaseCRUDService BaseCRUDService;

        public BaseAPIController(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor != null)
            {
                if (contextAccessor.HttpContext.Request.Headers.ContainsKey("CurrentUserID"))
                    CurrentUserID = long.Parse(contextAccessor.HttpContext.Request.Headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (contextAccessor.HttpContext.Request.Headers.ContainsKey("CurrentHomeID"))
                    CurrentHomeID = long.Parse(contextAccessor.HttpContext.Request.Headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                CurrentController = this.GetType().Name.Replace("Controller", "");
                CurrentArea = this.GetType().GetCustomAttributes(typeof(APPArea), true).Any() ? this.GetType().GetCustomAttributes(typeof(APPArea), true).Cast<APPArea>().Single().AreaName : null;

                BaseCRUDService = new BaseCRUDService(CurrentUserID, CurrentHomeID);
            }
        }
    }
}
