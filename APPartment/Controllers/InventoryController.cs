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
    public class InventoryController : BaseCRUDController<Inventory>
    {
        private readonly DataAccessContext _context;

        public InventoryController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        #region Actions
        [Breadcrumb(InventoryBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Inventory - All";
            ViewData["Module"] = "Inventory";

            return base.Index();
        }

        [Breadcrumb(InventoryBreadcrumbs.Supplied_Breadcrumb)]
        public async Task<IActionResult> Supplied()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<Inventory>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == true).ToListAsync();

            ViewData["GridTitle"] = "Inventory - Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;
            ViewData["Statuses"] = baseService.GetStatuses(typeof(Inventory));

            return View("_Grid", modelObjects);
        }

        [Breadcrumb(InventoryBreadcrumbs.Not_Supplied_Breadcrumb)]
        public async Task<IActionResult> NotSupplied()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<Inventory>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == false).ToListAsync();

            ViewData["GridTitle"] = "Inventory - Not Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;
            ViewData["Statuses"] = baseService.GetStatuses(typeof(Inventory));

            return View("_Grid", modelObjects);
        }

        public JsonResult GetInventoryCriticalCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var inventoryCriticalCount = _context.Set<Inventory>().ToList().Where(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Critical ||
                x.Status == (int)ObjectStatus.High) && x.IsCompleted == false).Count();

                return Json(inventoryCriticalCount);
            }

            return Json(0);
        }
        #endregion

        public override void PopulateViewData()
        {
        }
    }
}
