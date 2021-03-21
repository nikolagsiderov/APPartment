using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using APPIssue = APPartment.Data.Server.Models.Issue.Issue;

namespace APPartment.UI.ViewModels.Issue
{
    [IMapFrom(typeof(APPIssue))]
    public class IssueDisplayViewModel : GridItemViewModelWithHome
    {
        public bool IsClose { get; set; }
    }
}
