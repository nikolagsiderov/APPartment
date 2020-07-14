using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartBreadcrumbs.Attributes;
using System.Linq;
using APPartment.Enums;
using APPartment.Utilities.Constants.Breadcrumbs;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace APPartment.Controllers
{
    public class HygieneController : BaseCRUDController<Hygiene>
    {
        private readonly DataAccessContext _context;

        public HygieneController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
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

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return base.Index();
        }

        [Breadcrumb(HygieneBreadcrumbs.Cleaned_Breadcrumb)]
        public async Task<IActionResult> Cleaned()
        {
            ViewData["GridTitle"] = "Hygiene - Cleaned";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.Trivial || x.Status == (int)ObjectStatus.Medium));

            return await base.Index();
        }

        [Breadcrumb(HygieneBreadcrumbs.Due_Cleaning_Breadcrumb)]
        public async Task<IActionResult> DueCleaning()
        {
            ViewData["GridTitle"] = "Hygiene - Due Cleaning";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.High || x.Status == (int)ObjectStatus.Critical));

            return await base.Index();
        }

        public JsonResult GetHygieneCriticalCount()
        {
            var hygieneCriticalCount = _context.Set<Hygiene>().ToList().Where(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.Critical ||
            x.Status == (int)ObjectStatus.High)).Count();

            return Json(hygieneCriticalCount);
        }
        #endregion

        public override void PopulateViewData()
        {
        }
    }
}
