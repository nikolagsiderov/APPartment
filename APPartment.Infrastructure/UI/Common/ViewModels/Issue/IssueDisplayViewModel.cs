using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using APPIssue = APPartment.Data.Server.Models.Issue.Issue;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Issue
{
    [IMapFrom(typeof(APPIssue))]
    public class IssueDisplayViewModel : GridItemViewModelWithHome
    {
        [GridFieldDisplay]
        [Display(Name = "Closed")]
        public bool IsClosed { get; set; }
    }
}
