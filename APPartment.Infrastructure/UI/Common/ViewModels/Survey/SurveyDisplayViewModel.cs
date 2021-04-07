using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyDisplayViewModel : GridItemViewModelWithHome
    {
        [Display(Name = "Completed")]
        [GridFieldDisplay]
        public bool IsCompleted { get; set; }
    }
}
