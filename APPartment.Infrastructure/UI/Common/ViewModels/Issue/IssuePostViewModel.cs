using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using System.ComponentModel.DataAnnotations;
using APPIssue = APPartment.Data.Server.Models.Issue.Issue;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Issue
{
    [IMapFrom(typeof(APPIssue))]
    public class IssuePostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Closed")]
        public bool IsClosed { get; set; }
    }
}
