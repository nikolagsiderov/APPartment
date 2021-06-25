using System;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPartment.Infrastructure.UI.Common.ViewModels.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;

namespace APPartment.API.Areas.Inventory.Controllers
{
    [Area(APPAreas.Inventory)]
    public class SuppliedController : BaseAPICRUDController<InventoryDisplayViewModel, InventoryPostViewModel>
    {
        public SuppliedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<InventoryDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.IsSupplied == true;

        protected override void NormalizeDisplayModel(InventoryDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(InventoryPostViewModel model)
        {
        }
    }
}