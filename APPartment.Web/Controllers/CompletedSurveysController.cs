using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Survey;

namespace APPartment.Web.Controllers
{
    public class CompletedSurveysController : BaseCRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public CompletedSurveysController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID && x.IsCompleted == true; ;
            }
        }

        [Breadcrumb(SurveysBreadcrumbs.Completed_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Surveys - Completed";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var completedSurveysCount = BaseWebService.Count<SurveyPostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.IsCompleted == true);
            return Json(completedSurveysCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
