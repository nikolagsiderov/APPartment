using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Issue;
using APPAreas = APPartment.UI.Utilities.Constants.Areas;

namespace APPartment.Web.Areas.Issues.Controllers
{
    [Area(APPAreas.Issues)]
    public class IssuesController : BaseCRUDController<IssueDisplayViewModel, IssuePostViewModel>
    {
        public IssuesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<IssueDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID;
            }
        }

        [Breadcrumb(IssuesBreadcrumbs.Manage_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["Module"] = APPAreas.Issues;
            ViewData["Manage"] = true;

            return base.Index();
        }
    
        public JsonResult GetCount()
        {
            var count = BaseWebService.Count<IssuePostViewModel>(x => x.HomeID == (long)CurrentHomeID);
            return Json(count);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
