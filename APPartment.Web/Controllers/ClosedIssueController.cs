using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Issue;

namespace APPartment.Web.Controllers
{
    public class ClosedIssuesController : BaseCRUDController<IssueDisplayViewModel, IssuePostViewModel>
    {
        public ClosedIssuesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<IssueDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID && x.IsClosed == true; 
            }
        }

        [Breadcrumb(IssuesBreadcrumbs.Closed_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Issues - Closed";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var closedIssuesCount = BaseWebService.Count<IssuePostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.IsClosed == true);
            return Json(closedIssuesCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
