﻿using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPartment.Infrastructure.Services.Base;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace APPartment.Infrastructure.Controllers.Api
{
    [Area(APPAreas.Default)]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public abstract class BaseAPIController : ControllerBase
    {
        protected long CurrentUserID = 0L;
        protected long CurrentHomeID = 0L;
        protected BaseCRUDService BaseCRUDService;

        public BaseAPIController(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor != null)
            {
                if (contextAccessor.HttpContext.Request.Headers.ContainsKey("CurrentUserID"))
                    CurrentUserID = long.Parse(contextAccessor.HttpContext.Request.Headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (contextAccessor.HttpContext.Request.Headers.ContainsKey("CurrentHomeID"))
                    CurrentHomeID = long.Parse(contextAccessor.HttpContext.Request.Headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                BaseCRUDService = new BaseCRUDService(CurrentUserID, CurrentHomeID);
            }
        }
    }
}