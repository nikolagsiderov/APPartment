﻿using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Linq.Expressions;
using System;
using APPartment.Infrastructure.Controllers.Api;

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

                foreach (var surveyParticipantID in surveyParticipantsIDs)
                {
                    if (!BaseCRUDService.Any<SurveyParticipantPostViewModel>(x => x.SurveyID == model.ID && x.UserID == surveyParticipantID))
                    {
                        var surveyParticipant = new SurveyParticipantPostViewModel() { SurveyID = model.ID, UserID = surveyParticipantID };
                        BaseCRUDService.Save(surveyParticipant);
                    }
                }

                model = BaseCRUDService.Save(model);
                NormalizePostModel(model);

                return Ok(model);
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
