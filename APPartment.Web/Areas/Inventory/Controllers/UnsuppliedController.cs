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
    public class UnsuppliedController : BaseCRUDController<InventoryDisplayViewModel, InventoryPostViewModel>
    {
        public UnsuppliedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<InventoryDisplayViewModel, bool>> FilterExpression => x => x.HomeID == (long)CurrentHomeID && x.IsSupplied == false;

        public override bool CanManage => false;

        [Breadcrumb(InventoryBreadcrumbs.Unsupplied_Breadcrumb)]
        public override IActionResult Index()
        {
            return base.Index();
        }

        public JsonResult GetCount()
        {
            var notSuppliedInventoryCount = BaseWebService.Count<InventoryPostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.IsSupplied == false);
            return Json(notSuppliedInventoryCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
