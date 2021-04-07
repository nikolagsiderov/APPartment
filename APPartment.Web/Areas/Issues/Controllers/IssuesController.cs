using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Issue;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;

namespace APPartment.Web.Areas.Issues.Controllers
{
    [Area(APPAreas.Issues)]
    public class IssuesController : BaseCRUDController<IssueDisplayViewModel, IssuePostViewModel>
    {
        public IssuesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<IssueDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID;

        public override bool CanManage => true;

        [Breadcrumb(IssuesBreadcrumbs.Manage_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override async Task PopulateViewData(IssuePostViewModel model)
        {
        }
    }
}
