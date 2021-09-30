using APPartment.Infrastructure.UI.Common.ViewModels.Inventory;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;

namespace APPartment.API.Controllers
{
    [APPArea(APPAreas.Inventory)]
    public class InventoryController : BaseAPICRUDController<InventoryDisplayViewModel, InventoryPostViewModel>
    {
        public InventoryController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<InventoryDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID;

        protected override void NormalizeDisplayModel(InventoryDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(InventoryPostViewModel model)
        {
        }
    }
}
