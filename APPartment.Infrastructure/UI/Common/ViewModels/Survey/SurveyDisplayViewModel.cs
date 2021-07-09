using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using System;
using System.ComponentModel.DataAnnotations;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyDisplayViewModel : GridItemViewModel
    {
        [Display(Name = "Expire date")]
        [GridFieldDisplay]
        public DateTime ExpireDate { get; set; }

        [GridFieldDisplay]
        public bool Active { get; set; }
    }
}
