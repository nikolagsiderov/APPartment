using APPartment.UI.Attributes;
using APPartment.UI.Constants;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPIssue = APPartment.Data.Server.Models.Issue.Issue;

namespace APPartment.UI.ViewModels.Issue
{
    [IMapFrom(typeof(APPIssue))]
    public class IssuePostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Closed")]
        public bool IsClosed { get; set; }
    }
}
