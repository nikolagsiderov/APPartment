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
    public class PendingController : BaseCRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public PendingController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => false;

        [Breadcrumb(SurveysBreadcrumbs.Pending_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        [HttpGet]
        public async Task<IActionResult> TakeSurvey(long id)
        {
            var model = await APPI.RequestEntity<TakeSurveyPostViewModel>(id, CurrentAreaName, CurrentControllerName, nameof(TakeSurvey));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FinishSurvey(string surveyID, string finishLater)
        {
            // TODO: Call APPI
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCorrect(string answerID)
        {
            // TODO: Call APPI
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetOpenEndedAnswer(string answerID, string answer)
        {
            // TODO: Call APPI
            return Ok();
        }

        public override async Task SetGridItemActions(SurveyDisplayViewModel model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(TakeSurvey), model.ID, "btn-outline-warning", "fas fa-sign-in-alt"));
        }

        protected override void Normalize(SurveyPostViewModel model)
        {
        }
    }
}
