using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.UI.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyDisplayViewModel : GridItemViewModelWithHome
    {
        public bool IsCompleted { get; set; }
    }
}
