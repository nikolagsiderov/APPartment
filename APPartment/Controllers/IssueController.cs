﻿using SmartBreadcrumbs.Attributes;
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

        public override Expression<Func<Issue, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeId == CurrentHomeId;
            }
        }

        #region Actions
        [Breadcrumb(IssuesBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Issues - All";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = true;

            return base.Index();
        }

        [Breadcrumb(IssuesBreadcrumbs.Closed_Breadcrumb)]
        public IActionResult Closed()
        {
            ViewData["GridTitle"] = "Issues - Closed";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            return base.Index();
        }

        [Breadcrumb(IssuesBreadcrumbs.Open_Breadcrumb)]
        public IActionResult Open()
        {
            ViewData["GridTitle"] = "Issues - Open";
            ViewData["Module"] = "Issues";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetIssuesCriticalCount()
        {
            var issuesCriticalCount = baseFacade.Count<Issue>(x => x.HomeId == (long)CurrentHomeId);
            return Json(issuesCriticalCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
