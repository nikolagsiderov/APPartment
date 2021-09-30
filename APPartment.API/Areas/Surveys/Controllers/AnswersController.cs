using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;

namespace APPartment.API.Areas.Surveys.Controllers
{
    [APPArea(APPAreas.Surveys)]
    public class AnswersController : BaseAPICRUDController<SurveyAnswerDisplayViewModel, SurveyAnswerPostViewModel>
    {
        public AnswersController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<SurveyAnswerDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentUserID;

        protected override void NormalizeDisplayModel(SurveyAnswerDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(SurveyAnswerPostViewModel model)
        {
        }
    }
}