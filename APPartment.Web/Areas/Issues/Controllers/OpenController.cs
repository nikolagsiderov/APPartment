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
    public class OpenController : BaseCRUDController<IssueDisplayViewModel, IssuePostViewModel>
    {
        public OpenController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<IssueDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.IsClosed == false;

        public override bool CanManage => false;

        [Breadcrumb(IssuesBreadcrumbs.Open_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override async Task PopulateViewData(IssuePostViewModel model)
        {
        }
    }
}
