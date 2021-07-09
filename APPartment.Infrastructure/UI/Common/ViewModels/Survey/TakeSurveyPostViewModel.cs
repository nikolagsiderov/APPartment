using System.Collections.Generic;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    public class TakeSurveyPostViewModel
    {
        public string SurveyDisplayName { get; set; }

        public long SurveyID { get; set; }

        public long ParticipantID { get; set; }

        public List<KeyValuePair<SurveyQuestionPostViewModel, SurveyAnswerPostViewModel>> QuestionsAndAnswers { get; set; } = new List<KeyValuePair<SurveyQuestionPostViewModel, SurveyAnswerPostViewModel>>();
    }
}
