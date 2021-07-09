using System;
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