using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.Data.Server.Models.Objects;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.Enums;

namespace APPartment.Controllers
{
    public class IssuesController : BaseCRUDController<Issue>
    {
        public IssuesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<Issue, bool>> FilterExpression { get; set; }

        public override Expression<Func<Issue, bool>> FuncToExpression(Func<Issue, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(IssuesBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Issues - All";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = true;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return base.Index();
        }

        [Breadcrumb(IssuesBreadcrumbs.Closed_Breadcrumb)]
        public IActionResult Closed()
        {
            ViewData["GridTitle"] = "Issues - Closed";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.Status == (int)ObjectStatus.Trivial);

            return base.Index();
        }

        [Breadcrumb(IssuesBreadcrumbs.Open_Breadcrumb)]
        public IActionResult Open()
        {
            ViewData["GridTitle"] = "Issues - Open";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.Medium || x.Status == (int)ObjectStatus.High || x.Status == (int)ObjectStatus.Critical));

            return base.Index();
        }

        public JsonResult GetIssuesCriticalCount()
        {
            var issuesCriticalCount = baseFacade.GetObjects<Issue>(x => x.HomeId == (long)CurrentHomeId).Count();
            return Json(issuesCriticalCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
