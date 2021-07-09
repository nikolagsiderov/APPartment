using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPSurveyAnswer = APPartment.Data.Server.Models.Survey.SurveyAnswer;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyAnswer))]
    public class SurveyAnswerDisplayViewModel : GridItemViewModel
    {
        public string Answer { get; set; }

        public bool IsCorrect { get; set; }

        public int? ChoiceCap { get; set; }

        public long QuestionID { get; set; }
    }
}
