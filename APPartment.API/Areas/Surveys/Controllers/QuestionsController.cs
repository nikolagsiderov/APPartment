using System;
using System.Linq;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.API.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class QuestionsController : BaseAPICRUDController<SurveyQuestionDisplayViewModel, SurveyQuestionPostViewModel>
    {
        public QuestionsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<SurveyQuestionDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentUserID;

        [HttpGet("survey/{surveyID:long}")]
        public virtual ActionResult GetCollection(long surveyID)
        {
            try
            {
                var models = BaseCRUDService.GetCollection<SurveyQuestionDisplayViewModel>(x => x.SurveyID == surveyID);

                foreach (var model in models)
                {
                    NormalizeDisplayModel(model);
                }

                if (models.Any())
                    return Ok(models);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        protected override void NormalizeDisplayModel(SurveyQuestionDisplayViewModel model)
        {
            if (model.TypeID > 0)
                model.TypeDisplayName = BaseCRUDService.GetLookupEntity<SurveyQuestionTypeLookupViewModel>(model.TypeID).Name;
            else
                model.TypeDisplayName = "Bizarre question without possible answers";
        }

        protected override void NormalizePostModel(SurveyQuestionPostViewModel model)
        {
        }
    }
}