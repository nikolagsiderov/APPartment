using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.API.Controllers
{
    [Area(APPAreas.Neighbors)]
    public class NeighborsController : BaseAPICRUDController<HomeDisplayViewModel, HomePostViewModel>
    {
        public NeighborsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<HomeDisplayViewModel, bool>> FilterExpression => x => x.HomeID != CurrentHomeID;

        protected override void NormalizeDisplayModel(HomeDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(HomePostViewModel model)
        {
        }
    }
}
