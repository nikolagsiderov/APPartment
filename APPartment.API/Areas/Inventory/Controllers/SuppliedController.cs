using System;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;
using APPartment.Infrastructure.UI.Common.ViewModels.Inventory;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;

namespace APPartment.API.Areas.Inventory.Controllers
{
    [APPArea(APPAreas.Inventory)]
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