using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.Constants;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;

namespace APPartment.API.Areas.Surveys.Controllers
{
    [APPArea(APPAreas.Surveys)]
    public class CompletedController : BaseAPICRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public CompletedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
            {
                if (currentSurveyParticipant.StatusID != (long)SurveyParticipantStatuses.Submitted && model.Active)
                    model.HideItem = true;
            }
            else
                model.HideItem = true;
        }

        protected override void NormalizePostModel(SurveyPostViewModel model)
        {
        }
    }
}