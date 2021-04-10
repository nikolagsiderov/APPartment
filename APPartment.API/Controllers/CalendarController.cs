using System;
using System.Collections.Generic;
using System.Linq;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event;
using APPartment.Infrastructure.UI.Common.ViewModels.GeneralCalendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        // api/calendar/events
        [HttpGet]
        [Route("events")]
        public ActionResult GetEvents()
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var events = new BaseCRUDService(currentUserID).GetCollection<EventPostViewModel>(x => x.HomeID == currentHomeID);

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
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
