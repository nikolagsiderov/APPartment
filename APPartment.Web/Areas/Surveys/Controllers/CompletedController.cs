using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Survey;
using APPAreas = APPartment.UI.Constants.Areas;
using System.Threading.Tasks;

namespace APPartment.Web.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class CompletedController : BaseCRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public CompletedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<SurveyDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.IsCompleted == true;

        public override bool CanManage => false;

        [Breadcrumb(SurveysBreadcrumbs.Completed_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override void PopulateViewData()
        {
        }
    }
}
