using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;

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
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override async Task PopulateViewData(SurveyPostViewModel model)
        {
        }
    }
}
