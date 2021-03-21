using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPIssue = APPartment.Data.Server.Models.Issue.Issue;

namespace APPartment.UI.ViewModels.Issue
{
    [IMapFrom(typeof(APPIssue))]
    public class IssuePostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Boolean)]
        public bool IsClosed { get; set; }
    }
}
