using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.Constants;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.API.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class PendingController : BaseAPICRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public PendingController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentUserID;
            }
        }

        [HttpGet("takesurvey/{surveyID:long}")]
        public ActionResult TakeSurvey(long surveyID)
        {
            try
            {
                var model = new TakeSurveyPostViewModel() { SurveyID = surveyID };
                model.ParticipantID = BaseCRUDService.GetEntity<SurveyParticipantPostViewModel>(x => x.UserID == CurrentUserID).ID;
                var survey = BaseCRUDService.GetEntity<SurveyPostViewModel>(model.SurveyID);
                model.SurveyDisplayName = survey.Name;
                var questions = BaseCRUDService.GetCollection<SurveyQuestionPostViewModel>(x => x.SurveyID == survey.ID);

                foreach (var question in questions)
                {
                    var answers = BaseCRUDService.GetCollection<SurveyAnswerPostViewModel>(x => x.QuestionID == question.ID);

                    foreach (var answer in answers)
                    {
                        model.QuestionsAndAnswers.Add(new KeyValuePair<SurveyQuestionPostViewModel, SurveyAnswerPostViewModel>(question, answer));
                    }
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        protected override void NormalizeDisplayModel(SurveyDisplayViewModel model)
        {
            var currentSurveyParticipant = BaseCRUDService.GetEntity<SurveyParticipantPostViewModel>(y => y.UserID == CurrentUserID && y.SurveyID == model.ID);

            if (currentSurveyParticipant != null)
                if (currentSurveyParticipant.StatusID == (long)SurveyParticipantStatuses.Submitted && model.Active)
                    model.HideItem = true;
                else if (model.Active == false)
                    model.HideItem = true;
        }

        protected override void NormalizePostModel(SurveyPostViewModel model)
        {
        }
    }
}