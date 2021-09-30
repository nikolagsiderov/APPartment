using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.Controllers.Web;
using APPartment.Infrastructure.UI.Web.Html;
using System.Net.Http;
using APPartment.Common;

namespace APPartment.MVC.Web.Areas.Surveys.Controllers
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
            var model = await APPI.RequestAsync<TakeSurveyPostViewModel>(new string[] { CurrentAreaName, CurrentControllerName, nameof(TakeSurvey), id.ToString() }, CurrentUserID, CurrentHomeID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FinishSurvey(string surveyID, string finishLater)
        {
            var responseIsSuccess = false;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{nameof(SurveysController).Replace("Controller", "")}/{nameof(FinishSurvey)}/{long.Parse(surveyID)}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, bool.Parse(finishLater)))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        responseIsSuccess = true;
                }
            }

            if (responseIsSuccess)
                return Ok();
            else
                return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCorrect(string answerID)
        {
            var responseIsSuccess = false;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{nameof(SurveysController).Replace("Controller", "")}/{nameof(MarkAsCorrect)}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, long.Parse(answerID)))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        responseIsSuccess = true;
                }
            }

            if (responseIsSuccess)
                return Ok();
            else
                return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> SetOpenEndedAnswer(string questionID, string answer)
        {
            var responseIsSuccess = false;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{nameof(SurveysController).Replace("Controller", "")}/{nameof(SetOpenEndedAnswer)}/{long.Parse(questionID)}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, answer))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        responseIsSuccess = true;
                }
            }

            if (responseIsSuccess)
                return Ok();
            else
                return Json(false);
        }

        public override async Task SetGridItemActions(SurveyDisplayViewModel model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, nameof(TakeSurvey), model.ID, "btn-outline-warning", "fas fa-sign-in-alt"));
        }

        protected override void Normalize(SurveyPostViewModel model)
        {
        }
    }
}
