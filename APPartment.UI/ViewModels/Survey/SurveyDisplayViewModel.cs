﻿using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.UI.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyDisplayViewModel : GridItemViewModelWithHome
    {
        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }
    }
}
