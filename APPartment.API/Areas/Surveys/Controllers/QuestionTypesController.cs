using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;

namespace APPartment.API.Areas.Surveys.Controllers
{
    [APPArea(APPAreas.Surveys)]
    public class QuestionTypesController : BaseAPILookupController<SurveyQuestionTypeLookupViewModel>
    {
        public QuestionTypesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<SurveyQuestionTypeLookupViewModel, bool>> FilterExpression => null;

        protected override void NormalizeModel(SurveyQuestionTypeLookupViewModel model)
        {
        }
    }
}
