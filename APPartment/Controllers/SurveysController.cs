using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using Microsoft.AspNetCore.Http;
using System.Linq;
using SmartBreadcrumbs.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APPartment.Utilities.Constants.Breadcrumbs;
using System;
using System.Linq.Expressions;

namespace APPartment.Controllers
{
    public class SurveysController : BaseCRUDController<Survey>
    {
        private readonly DataAccessContext _context;

        public SurveysController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        public override Expression<Func<Survey, bool>> ExecuteInContext { get; set; }

        public override Expression<Func<Survey, bool>> FuncToExpression(Func<Survey, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(SurveysBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Surveys - All";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = true;

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId);

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

            ViewData["GridTitle"] = "Surveys - Completed";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId && x.Marked == true);

            return await base.Index();
        }

        [Breadcrumb(SurveysBreadcrumbs.Pending_Breadcrumb)]
        public async Task<IActionResult> Pending()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ViewData["GridTitle"] = "Surveys - Pending";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId && x.Marked == false);

            return await base.Index();
        }

        public JsonResult GetPendingSurveysCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var pendingSurveysCount = _context.Set<Survey>().ToList().Where(x => x.HouseId == currentHouseId && x.Marked == false).Count();

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
