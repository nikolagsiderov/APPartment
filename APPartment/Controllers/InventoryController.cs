using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.Data.Server.Models.Objects;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.Enums;

namespace APPartment.Controllers
{
    public class InventoryController : BaseCRUDController<Inventory>
    {
        public InventoryController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
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

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return base.Index();
        }

        [Breadcrumb(InventoryBreadcrumbs.Supplied_Breadcrumb)]
        public async Task<IActionResult> Supplied()
        {
            ViewData["GridTitle"] = "Inventory - Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.Trivial || x.Status == (int)ObjectStatus.Medium));

            return await base.Index();
        }

        [Breadcrumb(InventoryBreadcrumbs.Not_Supplied_Breadcrumb)]
        public async Task<IActionResult> NotSupplied()
        {
            ViewData["GridTitle"] = "Inventory - Not Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.High || x.Status == (int)ObjectStatus.Critical));

            return await base.Index();
        }

        public JsonResult GetInventoryCriticalCount()
        {
            var seachedInventory = new Inventory() { HomeId = (long)CurrentHomeId };
            var inventoryCriticalCount = dao.GetObjects(seachedInventory, x => x.HomeId == seachedInventory.HomeId).Count();
            return Json(inventoryCriticalCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
