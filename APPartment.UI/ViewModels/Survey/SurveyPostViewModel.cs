using APPartment.UI.Attributes;
using APPartment.UI.Constants;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.UI.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyPostViewModel : PostViewModelWithHome
    {
        [Display(Name = "Completed")]
        [APPUIHint(Templates.Boolean)]
        public bool IsCompleted { get; set; } = false;
    }
}
