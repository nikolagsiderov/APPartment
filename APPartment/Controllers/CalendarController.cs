using System;
using System.Collections.Generic;
using APPartment.DisplayModels.Calendar;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.Controllers
{
    public class CalendarController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new EventViewModel());
        }

        public JsonResult GetEvents(DateTime start, DateTime end)
        {
            var viewModel = new EventViewModel();
            var events = new List<EventViewModel>();
            start = DateTime.Today.AddDays(-14);
            end = DateTime.Today.AddDays(-11);

            for (var i = 1; i <= 5; i++)
            {
                events.Add(new EventViewModel()
                {
                    Id = i,
                    Title = "Event " + i,
                    Start = start.ToString(),
                    End = end.ToString(),
                    AllDay = false
                });

                start = start.AddDays(7);
                end = end.AddDays(7);
            }


            return Json(events.ToArray());
        }
    }
}