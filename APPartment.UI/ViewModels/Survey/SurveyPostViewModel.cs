using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.UI.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyPostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Boolean)]
        public bool IsCompleted { get; set; } = false;
    }
}
