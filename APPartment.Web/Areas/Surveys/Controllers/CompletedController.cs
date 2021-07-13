using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.Controllers.Web;
using APPartment.Infrastructure.UI.Web.Html;

namespace APPartment.Web.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class CompletedController : BaseCRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public CompletedController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => false;

        [Breadcrumb(SurveysBreadcrumbs.Completed_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        [HttpGet]
        public async Task<IActionResult> ViewSurvey(long id)
        {
            var model = await APPI.RequestEntity<TakeSurveyPostViewModel>(id, CurrentAreaName, CurrentControllerName, nameof(ViewSurvey));
            return View(model);
        }

        public override async Task SetGridItemActions(SurveyDisplayViewModel model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(ViewSurvey), model.ID, "btn-outline-warning", "fas fa-eye"));
        }

        protected override void Normalize(SurveyPostViewModel model)
        {
        }
    }
}
