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
using System.Linq.Expressions;
using System;

namespace APPartment.Controllers
{
    public class InventoryController : BaseCRUDController<Inventory>
    {
        private readonly DataAccessContext _context;

        public InventoryController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        public override Expression<Func<Inventory, bool>> ExecuteInContext { get; set; }

        public override Expression<Func<Inventory, bool>> FuncToExpression(Func<Inventory, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(InventoryBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Inventory - All";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = true;

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId);

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

            ViewData["GridTitle"] = "Inventory - Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Trivial || x.Status == (int)ObjectStatus.Medium));

            return await base.Index();
        }

        [Breadcrumb(InventoryBreadcrumbs.Not_Supplied_Breadcrumb)]
        public async Task<IActionResult> NotSupplied()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            ViewData["GridTitle"] = "Inventory - Not Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;

            ExecuteInContext = FuncToExpression(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.High || x.Status == (int)ObjectStatus.Critical));

            return await base.Index();
        }

        public JsonResult GetInventoryCriticalCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                var inventoryCriticalCount = _context.Set<Inventory>().ToList().Where(x => x.HouseId == currentHouseId && (x.Status == (int)ObjectStatus.Critical ||
                x.Status == (int)ObjectStatus.High)).Count();

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
