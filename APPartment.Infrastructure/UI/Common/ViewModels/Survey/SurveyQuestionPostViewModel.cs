using APPartment.Infrastructure.Attributes;
using APPSurveyQuestion = APPartment.Data.Server.Models.Survey.SurveyQuestion;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;
using System.ComponentModel.DataAnnotations;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyQuestion))]
    public class SurveyQuestionPostViewModel : PostViewModel
    {
        [Display(Name = "Question type")]
        [APPUIHint(Templates.Dropdown, SelectList = "TypeIDsSelectList")]
        public long TypeID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long SurveyID { get; set; }
    }
}
