using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.API.Areas.Chores.Controllers
{
    [Area(APPAreas.Chores)]
    [ApiController]
    [Route("api/[area]/[controller]")]
    public class MineController : BaseAPICRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public MineController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<ChoreDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.AssignedToUserID == CurrentUserID;

        protected override void NormalizeDisplayModel(ChoreDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(ChorePostViewModel model)
        {
        }
    }
}