using APPartment.Infrastructure.Services.Attributes;
using APPSurveyParticipant = APPartment.Data.Server.Models.Survey.SurveyParticipant;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyParticipant))]
    public class SurveyParticipantPostViewModel : PostViewModel
    {
        public long SurveyID { get; set; }

        public long UserID { get; set; }
    }
}
