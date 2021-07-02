using APPartment.Infrastructure.Attributes;
using APPSurveyQuestion = APPartment.Data.Server.Models.Survey.SurveyQuestion;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyQuestion))]
    public class SurveyQuestionPostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Hidden)]
        public long SurveyID { get; set; }
    }
}
