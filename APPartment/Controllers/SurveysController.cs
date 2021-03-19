using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Survey;

namespace APPartment.Controllers
{
    public class SurveysController : BaseCRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public SurveysController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeId == CurrentHomeId;
            }
        }

        #region Actions
        [Breadcrumb(SurveysBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Surveys - All";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = true;

            return base.Index();
        }

        [Breadcrumb(SurveysBreadcrumbs.Completed_Breadcrumb)]
        public IActionResult Completed()
        {
            ViewData["GridTitle"] = "Surveys - Completed";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;

            return base.Index();
        }

        [Breadcrumb(SurveysBreadcrumbs.Pending_Breadcrumb)]
        public IActionResult Pending()
        {
            ViewData["GridTitle"] = "Surveys - Pending";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetPendingSurveysCount()
        {
            var pendingSurveysCount = BaseWebService.Count<SurveyPostViewModel>(x => x.HomeId == (long)CurrentHomeId);
            return Json(pendingSurveysCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
