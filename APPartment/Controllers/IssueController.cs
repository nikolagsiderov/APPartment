using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using APPartment.Enums;
using APPartment.Utilities.Constants.Breadcrumbs;
using System;
using System.Linq.Expressions;

namespace APPartment.Controllers
{
    public class IssuesController : BaseCRUDController<Issue>
    {
        private readonly DataAccessContext _context;

        public IssuesController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        public override Expression<Func<Issue, bool>> ExecuteInContext { get; set; }

        public override Expression<Func<Issue, bool>> FuncToExpression(Func<Issue, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(IssuesBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Issues - All";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = true;

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId);

            return base.Index();
        }

        [Breadcrumb(IssuesBreadcrumbs.Closed_Breadcrumb)]
        public async Task<IActionResult> Closed()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ViewData["GridTitle"] = "Issues - Closed";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId && x.Marked == true);

            return await base.Index();
        }

        [Breadcrumb(IssuesBreadcrumbs.Open_Breadcrumb)]
        public async Task<IActionResult> Open()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ViewData["GridTitle"] = "Issues - Open";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId && x.Marked == false);

            return await base.Index();
        }

        public JsonResult GetIssuesCriticalCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var issuesCriticalCount = _context.Set<Issue>().ToList().Where(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Critical || 
                x.Status == (int)ObjectStatus.High) && x.Marked == false).Count();

                return Json(issuesCriticalCount);
            }

            return Json(0);
        }
        #endregion

        public override void PopulateViewData()
        {
        }
    }
}
