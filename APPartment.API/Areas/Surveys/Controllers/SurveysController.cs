using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Linq.Expressions;
using System;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.Constants;

namespace APPartment.API.Controllers
{
    [Area(APPAreas.Surveys)]
    public class SurveysController : BaseAPICRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public SurveysController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentUserID;

        [HttpPost]
        public override ActionResult CreateOrEdit([FromBody] SurveyPostViewModel model)
        {
            try
            {
                var surveyParticipantsIDs = model.SurveyParticipantsIDs;
                model = BaseCRUDService.Save(model);
                var currentSurveyParticipants = BaseCRUDService.GetCollection<SurveyParticipantPostViewModel>(x => x.SurveyID == model.ID);

                if (currentSurveyParticipants != null && currentSurveyParticipants.Count > 0)
                {
                    foreach (var participant in currentSurveyParticipants)
                    {
                        var surveyParticipantAnswers = BaseCRUDService.GetCollection<SurveyParticipantAnswerPostViewModel>(x => x.SurveyParticipantID == participant.ID);
                        surveyParticipantAnswers.ForEach(participantAnswer => BaseCRUDService.Delete(participantAnswer));
                        BaseCRUDService.Delete(participant);
                    }
                }

                foreach (var surveyParticipantID in surveyParticipantsIDs)
                {
                    if (BaseCRUDService.Any<SurveyParticipantPostViewModel>(x => x.SurveyID == model.ID && x.UserID == surveyParticipantID) == false)
                    {
                        var surveyParticipant = new SurveyParticipantPostViewModel() { SurveyID = model.ID, UserID = surveyParticipantID };
                        BaseCRUDService.Save(surveyParticipant);
                    }
                }

                NormalizePostModel(model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost("finishsurvey/{surveyID:long}")]
        public ActionResult FinishSurvey(long surveyID, [FromBody] bool finishLater)
        {
            try
            {
                var currentSurveyParticipant = BaseCRUDService.GetEntity<SurveyParticipantPostViewModel>(x => x.UserID == CurrentUserID && x.SurveyID == surveyID);

                if (currentSurveyParticipant != null)
                {
                    if (finishLater)
                        currentSurveyParticipant.StatusID = (long)SurveyParticipantStatuses.StartedNotCompleted;
                    else
                        currentSurveyParticipant.StatusID = (long)SurveyParticipantStatuses.Submitted;

                    BaseCRUDService.Save(currentSurveyParticipant);

                    return Ok();
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }

        }

        [HttpPost("markascorrect")]
        public ActionResult MarkAsCorrect([FromBody] long answerID)
        {
            try
            {
                var answer = BaseCRUDService.GetEntity<SurveyAnswerPostViewModel>(answerID);
                var singleQuestion = BaseCRUDService.GetEntity<SurveyQuestionPostViewModel>(answer.QuestionID);
                var surveyID = singleQuestion.SurveyID;
                var currentSurveyParticipant = BaseCRUDService.GetEntity<SurveyParticipantPostViewModel>(x => x.UserID == CurrentUserID && x.SurveyID == surveyID);

                if (currentSurveyParticipant != null)
                {
                    var currentSurveyParticipantAnswer = BaseCRUDService.GetEntity<SurveyParticipantAnswerPostViewModel>
                        (x => x.AnswerID == answerID && x.SurveyParticipantID == currentSurveyParticipant.ID);

                    if (currentSurveyParticipantAnswer != null)
                    {
                        currentSurveyParticipantAnswer.AnswerID = answerID;
                        currentSurveyParticipantAnswer.MarkedAsCorrect = true;
                    }
                    else
                    {
                        currentSurveyParticipantAnswer = new SurveyParticipantAnswerPostViewModel()
                        {
                            AnswerID = answerID,
                            SurveyParticipantID = currentSurveyParticipant.ID,
                            MarkedAsCorrect = true
                        };
                    }

                    BaseCRUDService.Save(currentSurveyParticipantAnswer);

                    return Ok();
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost("setopenendedanswer/{questionID:long}")]
        public ActionResult SetOpenEndedAnswer(long questionID, [FromBody] string answer)
        {
            try
            {
                var theAnswer = BaseCRUDService.GetEntity<SurveyAnswerPostViewModel>(x => x.QuestionID == questionID);
                theAnswer.Answer = answer;
                var singleQuestion = BaseCRUDService.GetEntity<SurveyQuestionPostViewModel>(theAnswer.QuestionID);
                var surveyID = singleQuestion.SurveyID;
                var currentSurveyParticipant = BaseCRUDService.GetEntity<SurveyParticipantPostViewModel>(x => x.UserID == CurrentUserID && x.SurveyID == surveyID);

                if (currentSurveyParticipant != null)
                {
                    var currentSurveyParticipantAnswer = BaseCRUDService.GetEntity<SurveyParticipantAnswerPostViewModel>
                        (x => x.AnswerID == theAnswer.ID && x.SurveyParticipantID == currentSurveyParticipant.ID);

                    if (currentSurveyParticipantAnswer != null)
                    {
                        currentSurveyParticipantAnswer.AnswerID = theAnswer.ID;
                        currentSurveyParticipantAnswer.MarkedAsCorrect = true;
                    }
                    else
                    {
                        currentSurveyParticipantAnswer = new SurveyParticipantAnswerPostViewModel()
                        {
                            AnswerID = theAnswer.ID,
                            SurveyParticipantID = currentSurveyParticipant.ID,
                            MarkedAsCorrect = true
                        };
                    }

                    BaseCRUDService.Save(currentSurveyParticipantAnswer);
                    BaseCRUDService.Save(theAnswer);

                    return Ok();
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        protected override void NormalizeDisplayModel(SurveyDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(SurveyPostViewModel model)
        {
            var surveyParticipants = BaseCRUDService.GetCollection<SurveyParticipantPostViewModel>(x => x.SurveyID == model.ID);

            foreach (var surveyParticipant in surveyParticipants)
            {
                model.SurveyParticipantsIDs.Add(surveyParticipant.UserID);
            }
        }
    }
}
