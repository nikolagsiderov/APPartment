using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using APPartment.Common;
using Newtonsoft.Json;
using APPartment.Infrastructure.UI.Web.Html;
using APPartment.Infrastructure.Controllers.Web;

namespace APPartment.Web.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class SurveysController : BaseCRUDController<SurveyDisplayViewModel, SurveyPostViewModel>
    {
        public SurveysController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => true;

        [Breadcrumb(SurveysBreadcrumbs.Manage_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        [HttpGet]
        public async Task<IActionResult> Assign(long id)
        {
            var model = new SurveyPostViewModel();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/{id}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<SurveyPostViewModel>(content);
                }
            }

            var participantsSelectList = await GetAssignedUsersSelectList(model);
            ViewData["SurveyParticipantsIDs"] = participantsSelectList;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(string ID, string[] surveyParticipantsIDs)
        {
            var model = new SurveyPostViewModel();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/{long.Parse(ID)}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<SurveyPostViewModel>(content);
                }
            }

            var parsedSurveyParticipantsIDs = new List<long>();

            foreach (var IDstring in surveyParticipantsIDs)
            {
                parsedSurveyParticipantsIDs.Add(long.Parse(IDstring));
            }

            model.SurveyParticipantsIDs = parsedSurveyParticipantsIDs;

            return await base.Edit(model.ID, model);
        }

        public override async Task SetObjectActions(SurveyPostViewModel model)
        {
            model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Assign), model.ID, "btn-outline-warning", "fas fa-tag"));
            await base.SetObjectActions(model);
        }

        public override async Task SetGridItemActions(SurveyDisplayViewModel model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Assign), model.ID, "btn-outline-warning", "fas fa-tag"));
            await base.SetGridItemActions(model);
        }

        protected override async Task PopulateViewData(SurveyPostViewModel model)
        {
            await base.PopulateViewData(model);

            var participantsSelectList = await GetAssignedUsersSelectList(model);
            ViewData["SurveyParticipantsIDs"] = participantsSelectList;

            foreach (var participant in participantsSelectList)
            {
                model.SurveyParticipantsIDs.Add(long.Parse(participant.Value));
            }

            var typesSelectList = await GetSurveyTypesSelectList(model);
            ViewData["SurveyTypeID"] = typesSelectList;
        }

        protected override void PopulateViewDataForIndex()
        {
            base.PopulateViewDataForIndex();

            ViewData["TypesLink"] = Url.Action(nameof(Index), nameof(SurveyTypesController).Replace("Controller", ""));
            ViewData["TypesLinkFAIcon"] = "fas fa-poll";
        }

        private async Task<List<SelectListItem>> GetAssignedUsersSelectList(SurveyPostViewModel model)
        {
            var users = await GetUsersInCurrentHome();

            var usersSelectList = users.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();

            if (model.ID > 0)
            {
                foreach (var item in usersSelectList)
                {
                    if (string.IsNullOrEmpty(item.Value))
                        continue;

                    if (model.SurveyParticipantsIDs.Contains(long.Parse(item.Value)))
                    {
                        item.Selected = true;
                    }
                }
            }

            return usersSelectList;
        }

        private async Task<List<SelectListItem>> GetSurveyTypesSelectList(SurveyPostViewModel model)
        {
            var types = new List<SurveyTypeDisplayViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{nameof(SurveyTypesController).Replace("Controller", "")}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        types = JsonConvert.DeserializeObject<List<SurveyTypeDisplayViewModel>>(content);
                }
            }

            var typesSelectList = types.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();

            if (model.ID > 0)
            {
                foreach (var item in typesSelectList)
                {
                    if (string.IsNullOrEmpty(item.Value))
                        continue;

                    if (model.SurveyTypeID.Equals(long.Parse(item.Value)))
                    {
                        item.Selected = true;
                    }
                }
            }

            return typesSelectList;
        }
    }
}
