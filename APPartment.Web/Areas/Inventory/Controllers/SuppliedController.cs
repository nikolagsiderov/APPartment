using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Inventory;
using APPAreas = APPartment.UI.Utilities.Constants.Areas;

namespace APPartment.Web.Areas.Inventory.Controllers
{
    [Area(APPAreas.Inventory)]
    public class SuppliedController : BaseCRUDController<InventoryDisplayViewModel, InventoryPostViewModel>
    {
        public SuppliedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<InventoryDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == (long)CurrentHomeID && x.IsSupplied == true;
            }
        }

        [Breadcrumb(InventoryBreadcrumbs.Supplied_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["Module"] = APPAreas.Inventory;
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var count = BaseWebService.Count<InventoryPostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.IsSupplied == true);
            return Json(count);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
