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