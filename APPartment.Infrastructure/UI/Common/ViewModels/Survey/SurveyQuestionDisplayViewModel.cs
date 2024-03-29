﻿using APPartment.Infrastructure.Attributes;
using APPSurveyQuestion = APPartment.Data.Server.Models.Survey.SurveyQuestion;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurveyQuestion))]
    public class SurveyQuestionDisplayViewModel : GridItemViewModel
    {
        public long TypeID { get; set; }

        public long SurveyID { get; set; }

        public string TypeDisplayName { get; set; }
    }
}
