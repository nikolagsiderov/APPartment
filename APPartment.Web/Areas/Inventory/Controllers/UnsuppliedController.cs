using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Inventory;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;

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
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override async Task PopulateViewData(InventoryPostViewModel model)
        {
        }
    }
}
