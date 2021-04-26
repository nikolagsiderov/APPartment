using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;

namespace APPartment.API.Controllers
{
    [Area(APPAreas.Chores)]
    [ApiController]
    [Route("api/[area]/[controller]")]
    public class ChoresController : BaseAPICRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public ChoresController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<ChoreDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID;

        protected override void NormalizeDisplayModel(ChoreDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(ChorePostViewModel model)
        {
        }
    }
}
