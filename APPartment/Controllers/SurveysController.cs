﻿using Microsoft.AspNetCore.Http;
using System.Linq;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.Data.Server.Models.Objects;
using APPartment.UI.Utilities.Constants.Breadcrumbs;

namespace APPartment.Controllers
{
    public class SurveysController : BaseCRUDController<Survey>
    {
        public SurveysController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<Survey, bool>> FilterExpression { get; set; }

        public override Expression<Func<Survey, bool>> FuncToExpression(Func<Survey, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(SurveysBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Surveys - All";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = true;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return base.Index();
        }

        [Breadcrumb(SurveysBreadcrumbs.Completed_Breadcrumb)]
        public IActionResult Completed()
        {
            ViewData["GridTitle"] = "Surveys - Completed";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.IsCompleted == true);

            return base.Index();
        }

        [Breadcrumb(SurveysBreadcrumbs.Pending_Breadcrumb)]
        public IActionResult Pending()
        {
            ViewData["GridTitle"] = "Surveys - Pending";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.IsCompleted == false);

            return base.Index();
        }

        public JsonResult GetPendingSurveysCount()
        {
            var pendingSurveysCount = baseFacade.GetObjects<Survey>(x => x.HomeId == (long)CurrentHomeId).Count();
            return Json(pendingSurveysCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
