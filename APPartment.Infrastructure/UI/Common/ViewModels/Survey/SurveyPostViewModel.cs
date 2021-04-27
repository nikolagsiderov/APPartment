using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APPSurvey = APPartment.Data.Server.Models.Survey.Survey;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Survey
{
    [IMapFrom(typeof(APPSurvey))]
    public class SurveyPostViewModel : PostViewModel
    {
        [Required]
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; } = false;

        [APPUIHint(Templates.Multiselect, Row = 3, Col = 4, Order = 1, Section = "General Information", SelectList = nameof(SurveyParticipantsIDs))]
        [Display(Name = "Survey participants")]
        public List<long> SurveyParticipantsIDs { get; set; } = new List<long>();

        [Required]
        [APPUIHint(Templates.Dropdown, Row = 3, Col = 4, Order = 2, Section = "General Information", SelectList = nameof(SurveyTypeID))]
        [Display(Name = "Survey type")]
        public long SurveyTypeID { get; set; }
    }
}
