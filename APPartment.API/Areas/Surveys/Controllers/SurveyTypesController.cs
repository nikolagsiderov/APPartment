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
    public class SurveyTypesController : BaseAPICRUDController<SurveyTypeDisplayViewModel, SurveyTypePostViewModel>
    {
        public SurveyTypesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<SurveyTypeDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentUserID;

        protected override void NormalizeDisplayModel(SurveyTypeDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(SurveyTypePostViewModel model)
        {
        }
    }
}