using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.API.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    [ApiController]
    [Route("api/[area]/[controller]")]
    public class CompletedController : BaseAPICRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public CompletedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentUserID && x.IsCompleted == true;

        protected override void NormalizeDisplayModel(SurveyDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(SurveyPostViewModel model)
        {
        }
    }
}