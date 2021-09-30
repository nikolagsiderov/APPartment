using APPartment.Infrastructure.UI.Common.ViewModels.Issue;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;
using System.Linq.Expressions;
using System;
using APPartment.Infrastructure.Controllers.Api;

namespace APPartment.API.Controllers
{
    [APPArea(APPAreas.Issues)]
    public class IssuesController : BaseAPICRUDController<IssueDisplayViewModel, IssuePostViewModel>
    {
        public IssuesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<IssueDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID;

        protected override void NormalizeDisplayModel(IssueDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(IssuePostViewModel model)
        {
        }
    }
}
