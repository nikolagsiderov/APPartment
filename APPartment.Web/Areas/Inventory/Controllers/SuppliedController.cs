using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Inventory;
using APPAreas = APPartment.UI.Constants.Areas;
using System.Threading.Tasks;

namespace APPartment.Web.Areas.Inventory.Controllers
{
    [Area(APPAreas.Inventory)]
    public class SuppliedController : BaseCRUDController<InventoryDisplayViewModel, InventoryPostViewModel>
    {
        public SuppliedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<InventoryDisplayViewModel, bool>> FilterExpression => x => x.HomeID == (long)CurrentHomeID && x.IsSupplied == true;

        public override bool CanManage => false;

        [Breadcrumb(InventoryBreadcrumbs.Supplied_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override void PopulateViewData(InventoryPostViewModel model)
        {
        }
    }
}
