using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPSurveyParticipantAnswer = APPartment.Data.Server.Models.Survey.SurveyParticipantAnswer;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyParticipantAnswer))]
    public class SurveyParticipantAnswerPostViewModel : PostViewModel
    {
        public long SurveyParticipantID { get; set; }

        public long AnswerID { get; set; }

        public bool MarkedAsCorrect { get; set; }
    }
}
