using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPSurveyParticipantStatus = APPartment.Data.Server.Models.Survey.SurveyParticipantStatus;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyParticipantStatus))]
    public class SurveyParticipantStatusLookupViewModel : LookupPostViewModel
    {
    }
}
