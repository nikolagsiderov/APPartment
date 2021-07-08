using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPSurveyQuestionType = APPartment.Data.Server.Models.Survey.SurveyQuestionType;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyQuestionType))]
    public class SurveyQuestionTypeLookupViewModel : LookupPostViewModel
    {
    }
}
