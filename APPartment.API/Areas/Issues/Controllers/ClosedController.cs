using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.UI.Common.ViewModels.Issue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.API.Areas.Issues.Controllers
{
    [Area(APPAreas.Issues)]
    public class ClosedController : BaseAPICRUDController<IssueDisplayViewModel, IssuePostViewModel>
    {
        public ClosedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<IssueDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.IsClosed == true;

        protected override void NormalizeDisplayModel(IssueDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(IssuePostViewModel model)
        {
        }
    }
}