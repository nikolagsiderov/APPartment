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
    }
}
