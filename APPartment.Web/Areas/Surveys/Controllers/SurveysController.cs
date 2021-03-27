using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Survey;
using APPAreas = APPartment.UI.Utilities.Constants.Areas;

namespace APPartment.Web.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class SurveysController : BaseCRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public SurveysController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID;

        public override bool CanManage => true;

        [Breadcrumb(SurveysBreadcrumbs.Manage_Breadcrumb)]
        public override IActionResult Index()
        {
            return base.Index();
        }

        public JsonResult GetCount()
        {
            var count = BaseWebService.Count<SurveyPostViewModel>(x => x.HomeID == (long)CurrentHomeID);
            return Json(count);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
