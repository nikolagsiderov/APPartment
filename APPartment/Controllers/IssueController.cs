using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Issues - All";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = true;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return base.Index();
        }

        [Breadcrumb(IssuesBreadcrumbs.Closed_Breadcrumb)]
        public async Task<IActionResult> Closed()
        {
            ViewData["GridTitle"] = "Issues - Closed";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.Status == (int)ObjectStatus.Trivial);

            return await base.Index();
        }

        [Breadcrumb(IssuesBreadcrumbs.Open_Breadcrumb)]
        public async Task<IActionResult> Open()
        {
            ViewData["GridTitle"] = "Issues - Open";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.Medium || x.Status == (int)ObjectStatus.High || x.Status == (int)ObjectStatus.Critical));

            return await base.Index();
        }

        public JsonResult GetIssuesCriticalCount()
        {
            var searchedIssue = new Issue() { HomeId = (long)CurrentHomeId };
            var issuesCriticalCount = dao.GetObjects(searchedIssue, x => x.HomeId == searchedIssue.HomeId).Count();
            return Json(issuesCriticalCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
