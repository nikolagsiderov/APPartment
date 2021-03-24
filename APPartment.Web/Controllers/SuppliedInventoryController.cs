using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Inventory;

namespace APPartment.Web.Controllers
{
    public class SuppliedInventoryController : BaseCRUDController<InventoryDisplayViewModel, InventoryPostViewModel>
    {
        public SuppliedInventoryController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
            ViewData["GridTitle"] = "Inventory - Supplied";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var suppliedInventoryCount = BaseWebService.Count<InventoryPostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.IsSupplied == true);
            return Json(suppliedInventoryCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
