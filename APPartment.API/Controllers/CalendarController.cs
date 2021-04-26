using System;
using System.Collections.Generic;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event;
using APPartment.Infrastructure.UI.Common.ViewModels.GeneralCalendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : BaseAPIController
    {
        public CalendarController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        // api/calendar/events
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
