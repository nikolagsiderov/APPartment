using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPSurveyAnswer = APPartment.Data.Server.Models.Survey.SurveyAnswer;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.UI.Web.Constants;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyAnswer))]
    public class SurveyAnswerPostViewModel : PostViewModel
    {
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        [Display(Name = "Correct")]
        [APPUIHint(Templates.Boolean)]
        public bool IsCorrect { get; set; }

        [Display(Name = "Choice cap")]
        [APPUIHint(Templates.NumberNullable)]
        public int? ChoiceCap { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long QuestionID { get; set; }

        public SurveyParticipantAnswerPostViewModel SurveyParticipantAnswer { get; set; } = new SurveyParticipantAnswerPostViewModel();
    }
}
