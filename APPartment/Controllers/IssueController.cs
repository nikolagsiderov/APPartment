using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using APPartment.Enums;
using Microsoft.EntityFrameworkCore;
using APPartment.Utilities.Constants.Breadcrumbs;

namespace APPartment.Controllers
{
    public class IssuesController : BaseCRUDController<Issue>
    {
        private readonly DataAccessContext _context;

        public IssuesController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        #region Actions
        [Breadcrumb(IssuesBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Issues - All";
            ViewData["Module"] = "Issues";

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

            var modelObjects = await _context.Set<Issue>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == true).ToListAsync();

            ViewData["GridTitle"] = "Issues - Closed";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;
            ViewData["Statuses"] = baseService.GetStatuses(typeof(Issue));

            return View("_Grid", modelObjects);
        }

        [Breadcrumb(IssuesBreadcrumbs.Open_Breadcrumb)]
        public async Task<IActionResult> Open()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<Issue>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == false).ToListAsync();

            ViewData["GridTitle"] = "Issues - Open";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;
            ViewData["Statuses"] = baseService.GetStatuses(typeof(Issue));

            return View("_Grid", modelObjects);
        }

        public JsonResult GetIssuesCriticalCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var issuesCriticalCount = _context.Set<Issue>().ToList().Where(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Critical || 
                x.Status == (int)ObjectStatus.High) && x.IsCompleted == false).Count();

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
