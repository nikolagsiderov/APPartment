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
    public class NotCompletedSurveysController : BaseCRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public NotCompletedSurveysController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID && x.IsCompleted == false; ;
            }
        }

        [Breadcrumb(SurveysBreadcrumbs.Pending_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Surveys - Not Completed";
            ViewData["Module"] = "Surveys";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var notCompletedSurveysCount = BaseWebService.Count<SurveyPostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.IsCompleted == false);
            return Json(notCompletedSurveysCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
