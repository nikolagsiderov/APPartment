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
    public class InventoryController : BaseCRUDController<Inventory>
    {
        private readonly DataAccessContext _context;

        public InventoryController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        [Breadcrumb("<i class='fas fa-tasks' style='font-size:20px'></i> Inventory")]
        public override Task<IActionResult> Index(string sortOrder)
        {
            ViewData["GridTitle"] = "Inventory";

            return base.Index(sortOrder);
        }

        public JsonResult GetInventoryCriticalCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var hygieneCriticalCount = _context.Set<Inventory>().ToList().Where(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Critical ||
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
