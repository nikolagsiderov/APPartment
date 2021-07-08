using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyPostViewModel : PostViewModel
    {
        [Required]
        [APPUIHint(Templates.Date, Row = 1, Col = 3, Order = 2, Section = "General Information")]
        [Display(Name = "Expire date")]
        public DateTime ExpireDate { get; set; } = DateTime.Today;

        [APPUIHint(Templates.Multiselect, Row = 3, Col = 4, Order = 1, Section = "General Information", SelectList = nameof(SurveyParticipantsIDs))]
        [Display(Name = "Survey participants")]
        public List<long> SurveyParticipantsIDs { get; set; } = new List<long>();

        [Required]
        [APPUIHint(Templates.Dropdown, Row = 3, Col = 4, Order = 2, Section = "General Information", SelectList = nameof(SurveyTypeID))]
        [Display(Name = "Survey type")]
        public long SurveyTypeID { get; set; }
    }
}
