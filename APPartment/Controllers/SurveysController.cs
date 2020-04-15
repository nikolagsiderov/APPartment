﻿using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using Microsoft.AspNetCore.Http;
using System.Linq;
using SmartBreadcrumbs.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APPartment.Utilities.Constants.Breadcrumbs;

namespace APPartment.Controllers
{
    public class SurveysController : BaseCRUDController<Survey>
    {
        private readonly DataAccessContext _context;

        public SurveysController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        #region Actions
        [Breadcrumb(SurveysBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Surveys - All";
            ViewData["Module"] = "Surveys";

            return base.Index();
        }

        [Breadcrumb(SurveysBreadcrumbs.Completed_Breadcrumb)]
        public async Task<IActionResult> Completed()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<Survey>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == true).ToListAsync();

            ViewData["GridTitle"] = "Surveys - Completed";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;
            ViewData["Statuses"] = baseService.GetStatuses(typeof(Survey));

            return View("_Grid", modelObjects);
        }

        [Breadcrumb(SurveysBreadcrumbs.Pending_Breadcrumb)]
        public async Task<IActionResult> Pending()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<Survey>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == false).ToListAsync();

            ViewData["GridTitle"] = "Surveys - Pending";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;
            ViewData["Statuses"] = baseService.GetStatuses(typeof(Survey));

            return View("_Grid", modelObjects);
        }

        public JsonResult GetPendingSurveysCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var pendingSurveysCount = _context.Set<Survey>().ToList().Where(x => x.HouseId == currentHouseId && x.IsCompleted == false).Count();

                return Json(pendingSurveysCount);
            }

            return Json(0);
        }
        #endregion

        public override void PopulateViewData()
        {
        }
    }
}
