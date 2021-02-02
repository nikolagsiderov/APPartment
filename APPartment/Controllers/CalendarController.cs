using System;
using System.Collections.Generic;
using APPartment.Data.Core;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.GeneralCalendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace APPartment.Controllers
{
    public class CalendarController : BaseAuthorizeController
    {
        public CalendarController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
        {
        }

        [HttpGet]
        [Breadcrumb(CalendarBreadcrumbs.Index)]
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