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
    public class NotClosedIssuesController : BaseCRUDController<IssueDisplayViewModel, IssuePostViewModel>
    {
        public NotClosedIssuesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<IssueDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID && x.IsClosed == false;
            }
        }

        [Breadcrumb(IssuesBreadcrumbs.Open_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Issues - Not Closed";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var notClosedIssuesCount = BaseWebService.Count<IssuePostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.IsClosed == false);
            return Json(notClosedIssuesCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
