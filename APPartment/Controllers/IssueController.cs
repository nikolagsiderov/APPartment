using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using APPartment.Enums;

namespace APPartment.Controllers
{
    public class IssuesController : BaseCRUDController<Issue>
    {
        private const string All_Breadcrumb = "<i class='fas fa-exclamation-circle' style='font-size:20px'></i> Issues";
        private const string Closed_Breadcrumb = "<i class='fas fa-check' style='font-size:20px'></i> Closed";
        private const string Open_Breadcrumb = "<i class='fas fa-exclamation-triangle' style='font-size:20px'></i> Open";

        private readonly DataAccessContext _context;

        public IssuesController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        [Breadcrumb(All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Issues - All";
            ViewData["Module"] = "Issues";

            return base.Index();
        }

        [Breadcrumb(Closed_Breadcrumb)]
        public override Task<IActionResult> IndexCompleted()
        {
            ViewData["GridTitle"] = "Issues - Closed";
            ViewData["Module"] = "Issues";

            return base.IndexCompleted();
        }

        [Breadcrumb(Open_Breadcrumb)]
        public override Task<IActionResult> IndexNotCompleted()
        {
            ViewData["GridTitle"] = "Issues - Open";
            ViewData["Module"] = "Issues";

            return base.IndexNotCompleted();
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

        public override void PopulateViewData()
        {
        }
    }
}
