using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPIssue = APPartment.Data.Server.Models.Issue.Issue;

namespace APPartment.UI.ViewModels.Issue
{
    [IMapFrom(typeof(APPIssue))]
    public class IssueDisplayViewModel : GridItemViewModelWithHome
    {
        [GridFieldDisplay]
        [Display(Name = "Closed")]
        public bool IsClosed { get; set; }
    }
}
