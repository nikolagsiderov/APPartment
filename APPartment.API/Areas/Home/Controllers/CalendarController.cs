using System;
using System.Collections.Generic;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event;
using APPartment.Infrastructure.UI.Common.ViewModels.GeneralCalendar;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Areas.Home.Controllers
{
    [APPArea(APPAreas.Home)]
    [Route("api/[area]/[controller]")]
    public class CalendarController : BaseAPIController
    {
        public CalendarController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet]
        [Route("events")]
        public ActionResult GetEvents()
        {
            try
            {
                var events = BaseCRUDService.GetCollection<EventPostViewModel>(x => x.HomeID == CurrentHomeID);

                var result = new List<EventViewModel>();

                foreach (var @event in events)
                {
                    result.Add(new EventViewModel()
                    {
                        ID = @event.ID,
                        Title = @event.Name,
                        Start = @event.StartDate.ToString(),
                        End = @event.EndDate.ToString(),
                        AllDay = true
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
