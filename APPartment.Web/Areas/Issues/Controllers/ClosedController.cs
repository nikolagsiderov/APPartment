﻿using SmartBreadcrumbs.Attributes;
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
    public class ClosedController : BaseCRUDController<IssueDisplayViewModel, IssuePostViewModel>
    {
        public ClosedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<IssueDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.IsClosed == true;

        public override bool CanManage => false;

        [Breadcrumb(IssuesBreadcrumbs.Closed_Breadcrumb)]
        public override IActionResult Index()
        {
            return base.Index();
        }

        public JsonResult GetCount()
        {
            var count = BaseWebService.Count<IssuePostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.IsClosed == true);
            return Json(count);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
