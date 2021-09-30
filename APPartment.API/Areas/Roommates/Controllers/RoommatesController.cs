using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;

namespace APPartment.API.Controllers
{
    [APPArea(APPAreas.Roommates)]
    public class RoommatesController : BaseAPICRUDController<UserDisplayViewModel, UserPostViewModel>
    {
        public RoommatesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<UserDisplayViewModel, bool>> FilterExpression => x => x.ID != CurrentUserID;

        protected override void NormalizeDisplayModel(UserDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(UserPostViewModel model)
        {
        }
    }
}
