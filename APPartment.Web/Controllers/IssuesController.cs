﻿using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Issue;

namespace APPartment.Web.Controllers
{
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

        [Breadcrumb(IssuesBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Issues - All";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = true;

            return base.Index();
        }
    
        public JsonResult GetCount()
        {
            var issuesCount = BaseWebService.Count<IssuePostViewModel>(x => x.HomeID == (long)CurrentHomeID);
            return Json(issuesCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
