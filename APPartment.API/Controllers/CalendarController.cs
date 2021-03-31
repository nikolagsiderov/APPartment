using System;
using System.Collections.Generic;
using System.Linq;
using APPartment.UI.ViewModels.GeneralCalendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
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

                var viewModel = new EventViewModel();
                var result = new List<EventViewModel>();
                var start = DateTime.Today.AddDays(-14);
                var end = DateTime.Today.AddDays(-11);

                for (var i = 1; i <= 5; i++)
                {
                    result.Add(new EventViewModel()
                    {
                        ID = i,
                        Title = "Event " + i,
                        Start = start.ToString(),
                        End = end.ToString(),
                        AllDay = false
                    });

                    start = start.AddDays(7);
                    end = end.AddDays(7);
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
