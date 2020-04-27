using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using APPartment.Enums;
using APPartment.Utilities.Constants.Breadcrumbs;
using System.Linq.Expressions;
using System;
using Microsoft.AspNetCore.Http;

namespace APPartment.Controllers
{
    public class InventoryController : BaseCRUDController<Inventory>
    {
        private readonly DataAccessContext _context;

        public InventoryController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
        {
            _context = context;
        }

        public override Expression<Func<Inventory, bool>> FilterExpression { get; set; }

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

            FilterExpression = FuncToExpression(x => x.HouseId == CurrentHouseId);

            return base.Index();
        }

        [Breadcrumb(InventoryBreadcrumbs.Supplied_Breadcrumb)]
        public async Task<IActionResult> Supplied()
        {
            ViewData["GridTitle"] = "Inventory - Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HouseId == CurrentHouseId && (x.Status == (int)ObjectStatus.Trivial || x.Status == (int)ObjectStatus.Medium));

            return await base.Index();
        }

        [Breadcrumb(InventoryBreadcrumbs.Not_Supplied_Breadcrumb)]
        public async Task<IActionResult> NotSupplied()
        {
            ViewData["GridTitle"] = "Inventory - Not Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HouseId == CurrentHouseId && (x.Status == (int)ObjectStatus.High || x.Status == (int)ObjectStatus.Critical));

            return await base.Index();
        }

        public JsonResult GetInventoryCriticalCount()
        {
            var inventoryCriticalCount = _context.Set<Inventory>().ToList().Where(x => x.HouseId == CurrentHouseId && (x.Status == (int)ObjectStatus.Critical ||
            x.Status == (int)ObjectStatus.High)).Count();
            return Json(inventoryCriticalCount);
        }
        #endregion

        public override void PopulateViewData()
        {
        }
    }
}
