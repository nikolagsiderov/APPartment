using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartBreadcrumbs.Attributes;
using System.Linq;
using Microsoft.AspNetCore.Http;
using APPartment.Enums;

namespace APPartment.Controllers
{
    public class HygieneController : BaseCRUDController<Hygiene>
    {
        private const string All_Breadcrumb = "<i class='fas fa-recycle' style='font-size:20px'></i> Hygiene";
        private const string Cleaned_Breadcrumb = "<i class='fas fa-check' style='font-size:20px'></i> Cleaned";
        private const string Due_Cleaning_Breadcrumb = "<i class='fas fa-exclamation-triangle' style='font-size:20px'></i> Due Cleaning";

        private readonly DataAccessContext _context;

        public HygieneController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        [Breadcrumb(All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Hygiene - All";
            ViewData["Module"] = "Hygiene";

            return base.Index();
        }

        [Breadcrumb(Cleaned_Breadcrumb)]
        public override Task<IActionResult> IndexCompleted()
        {
            ViewData["GridTitle"] = "Hygiene - Cleaned";
            ViewData["Module"] = "Hygiene";

            return base.IndexCompleted();
        }

        [Breadcrumb(Due_Cleaning_Breadcrumb)]
        public override Task<IActionResult> IndexNotCompleted()
        {
            ViewData["GridTitle"] = "Hygiene - Due Cleaning";
            ViewData["Module"] = "Hygiene";

            return base.IndexNotCompleted();
        }

        public JsonResult GetHygieneCriticalCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var hygieneCriticalCount = _context.Set<Hygiene>().ToList().Where(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Critical ||
                x.Status == (int)ObjectStatus.High) && x.IsCompleted == false).Count();

                return Json(hygieneCriticalCount);
            }

            return Json(0);
        }

        public override void PopulateViewData()
        {
        }
    }
}
