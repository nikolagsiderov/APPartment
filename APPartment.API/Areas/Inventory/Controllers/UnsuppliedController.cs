using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.API.Areas.Inventory.Controllers
{
    [Area(APPAreas.Inventory)]
    public class UnsuppliedController : BaseAPICRUDController<InventoryDisplayViewModel, InventoryPostViewModel>
    {
        public UnsuppliedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<InventoryDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.IsSupplied == false;

        protected override void NormalizeDisplayModel(InventoryDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(InventoryPostViewModel model)
        {
        }
    }
}