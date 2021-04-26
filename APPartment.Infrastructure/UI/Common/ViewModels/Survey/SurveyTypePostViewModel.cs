using APPSurveyType = APPartment.Data.Server.Models.Survey.SurveyType;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.Attributes;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyType))]
    public class SurveyTypePostViewModel : PostViewModel
    {
    }
}
