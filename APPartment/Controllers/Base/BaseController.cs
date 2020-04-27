using APPartment.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        public long? CurrentUserId { get; set; }
        public long? CurrentHouseId { get; set; }
        public string CurrentHouseName { get; set; }
        public string CurrentUserName { get; set; }

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

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("HouseId")))
                {
                    CurrentHouseId = long.Parse(contextAccessor.HttpContext.Session.GetString("HouseId"));
                }

                if (!string.IsNullOrEmpty(contextAccessor.HttpContext.Session.GetString("HouseName")))
                {
                    CurrentHouseName = contextAccessor.HttpContext.Session.GetString("HouseName").ToString();
                }
            }
        }
    }
}