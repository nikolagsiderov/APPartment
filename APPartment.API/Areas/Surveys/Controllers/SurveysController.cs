using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Linq.Expressions;
using System;
using APPartment.Infrastructure.Controllers.Api;

namespace APPartment.API.Controllers
{
    [Area(APPAreas.Surveys)]
    [ApiController]
    [Route("api/[area]/[controller]")]
    public class SurveysController : BaseAPICRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public SurveysController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentUserID;

        protected override void NormalizeDisplayModel(SurveyDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(SurveyPostViewModel model)
        {
        }
    }
}
