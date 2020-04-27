using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartBreadcrumbs.Attributes;
using System.Linq;
using Microsoft.AspNetCore.Http;
using APPartment.Enums;
using APPartment.Utilities.Constants.Breadcrumbs;
using System;
using System.Linq.Expressions;

namespace APPartment.Controllers
{
    public class HygieneController : BaseCRUDController<Hygiene>
    {
        private readonly DataAccessContext _context;

        public HygieneController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        public override Expression<Func<Hygiene, bool>> FilterExpression { get; set; }

        public override Expression<Func<Hygiene, bool>> FuncToExpression(Func<Hygiene, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(HygieneBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Hygiene - All";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = true;

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            FilterExpression = FuncToExpression(x => x.HouseId == currentHouseId);

            return base.Index();
        }

        [Breadcrumb(HygieneBreadcrumbs.Cleaned_Breadcrumb)]
        public async Task<IActionResult> Cleaned()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ViewData["GridTitle"] = "Hygiene - Cleaned";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Trivial || x.Status == (int)ObjectStatus.Medium));

            return await base.Index();
        }

        [Breadcrumb(HygieneBreadcrumbs.Due_Cleaning_Breadcrumb)]
        public async Task<IActionResult> DueCleaning()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ViewData["GridTitle"] = "Hygiene - Due Cleaning";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.High || x.Status == (int)ObjectStatus.Critical));

            return await base.Index();
        }
        
        public JsonResult GetHygieneCriticalCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var hygieneCriticalCount = _context.Set<Hygiene>().ToList().Where(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Critical ||
                x.Status == (int)ObjectStatus.High)).Count();

                return Json(hygieneCriticalCount);
            }

            return Json(0);
        }
        #endregion

        public override void PopulateViewData()
        {
        }
    }
}
