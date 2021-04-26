using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyDisplayViewModel : GridItemViewModel
    {
        [Display(Name = "Completed")]
        [GridFieldDisplay]
        public bool IsCompleted { get; set; }
    }
}
