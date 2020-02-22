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
        private readonly DataAccessContext _context;

        public HygieneController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        [Breadcrumb("<i class='fas fa-recycle' style='font-size:20px'></i> Hygiene")]
        public override Task<IActionResult> Index(string sortOrder)
        {
            return base.Index(sortOrder);
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

        public override void PopulateViewData()
        {
        }
    }
}
