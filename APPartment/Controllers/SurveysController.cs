using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using Microsoft.AspNetCore.Http;
using System.Linq;
using SmartBreadcrumbs.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.Controllers
{
    public class SurveysController : BaseCRUDController<Survey>
    {
        private const string All_Breadcrumb = "<i class='fas fa-tasks' style='font-size:20px'></i> All";
        private const string Pending_Breadcrumb = "<i class='fas fa-pen' style='font-size:20px'></i> Pending";
        private const string Completed_Breadcrumb = "<i class='fas fa-check' style='font-size:20px'></i> Completed";

        private readonly DataAccessContext _context;

        public SurveysController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        [Breadcrumb(All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Surveys - All";
            ViewData["Module"] = "Surveys";

            return base.Index();
        }

        [Breadcrumb(Completed_Breadcrumb)]
        public override Task<IActionResult> IndexCompleted()
        {
            ViewData["GridTitle"] = "Surveys - Completed";
            ViewData["Module"] = "Surveys";

            return base.IndexCompleted();
        }

        [Breadcrumb(Pending_Breadcrumb)]
        public override Task<IActionResult> IndexNotCompleted()
        {
            ViewData["GridTitle"] = "Surveys - Pending";
            ViewData["Module"] = "Surveys";

            return base.IndexNotCompleted();
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

        public override void PopulateViewData()
        {
        }
    }
}
