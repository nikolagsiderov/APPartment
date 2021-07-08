using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.API.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
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
