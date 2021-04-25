using APPartment.Infrastructure.Services.Attributes;
using APPSurveyType = APPartment.Data.Server.Models.Survey.SurveyType;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyType))]
    public class SurveyTypePostViewModel : PostViewModel
    {
    }
}
