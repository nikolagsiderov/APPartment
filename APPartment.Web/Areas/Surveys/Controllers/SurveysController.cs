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
using APPartment.Infrastructure.Tools;

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
        public async Task<IActionResult> Activate(long id)
        {
            var survey = await APPI.RequestEntity<SurveyPostViewModel>(id);
            survey.Active = true;

            await APPI.RequestPostEntity(survey);

            return RedirectToAction(nameof(base.Index), CurrentControllerName);
        }

        [HttpGet]
        public async Task<IActionResult> Inactivate(long id)
        {
            var survey = await APPI.RequestEntity<SurveyPostViewModel>(id);
            survey.Active = false;

            await APPI.RequestPostEntity(survey);

            return RedirectToAction(nameof(base.Index), CurrentControllerName);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStandardAnswer(string questionID, bool isCorrect, string answer)
        {
            var model = new SurveyAnswerPostViewModel() { QuestionID = long.Parse(questionID), Answer = answer, IsCorrect = isCorrect };
            await APPI.RequestPostEntity(model, CurrentAreaName, "Answers");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer(string questionID, string answer)
        {
            var model = new SurveyAnswerPostViewModel() { QuestionID = long.Parse(questionID), Answer = answer, IsCorrect = true };
            await APPI.RequestPostEntity(model, CurrentAreaName, "Answers");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> CreateQuestion(long id) // surveyID
        {
            ViewBag.SurveyID = id.ToString();

            var questionTypes = await APPI.RequestLookupEntities<SurveyQuestionTypeLookupViewModel>(CurrentAreaName, "QuestionTypes");

            // TODO: Ensure only one answer can be marked as correct on question type = Standard question with ONE correct answer
            // This is temporary, until we get stardard question with ONE correct answer working properly
            questionTypes.RemoveAt(0);

            var questionTypesSelectList = questionTypes.ToLookupSelectList();
            ViewData["TypeIDsSelectList"] = questionTypesSelectList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(string surveyID, string question, string typeID)
        {
            var model = new SurveyQuestionPostViewModel() { SurveyID = long.Parse(surveyID), Name = question, TypeID = long.Parse(typeID) };
            var savedQuestion = await APPI.RequestPostEntity(model, CurrentAreaName, nameof(this.Questions));

            return Ok(savedQuestion.ID);
        }

        [HttpGet]
        public async Task<IActionResult> Questions(long id) // surveyID
        {
            var questions = new List<SurveyQuestionDisplayViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{nameof(Questions)}/survey/{id}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        questions = JsonConvert.DeserializeObject<List<SurveyQuestionDisplayViewModel>>(content);
                }
            }

            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveQuestion(string ID)
        {
            await APPI.RequestDeleteEntity(long.Parse(ID), CurrentAreaName, nameof(this.Questions));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Assign(long id)
        {
            var model = await APPI.RequestEntity<SurveyPostViewModel>(id);

            var participantsSelectList = await GetAssignedUsersSelectList(model);
            ViewData[nameof(model.SurveyParticipantsIDs)] = participantsSelectList;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(string ID, string[] surveyParticipantsIDs)
        {
            var model = await APPI.RequestEntity<SurveyPostViewModel>(long.Parse(ID));
            var parsedSurveyParticipantsIDs = new List<long>();

            foreach (var IDstring in surveyParticipantsIDs)
            {
                parsedSurveyParticipantsIDs.Add(long.Parse(IDstring));
            }

            model.SurveyParticipantsIDs = parsedSurveyParticipantsIDs;
            await APPI.RequestPostEntity(model);

            return Ok();
        }

        public override async Task SetObjectActions(SurveyPostViewModel model)
        {
            await base.SetObjectActions(model);

            if (model.Active == false)
                model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Activate), model.ID, "btn-outline-warning", "fas fa-level-up-alt"));
            else
                model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Inactivate), model.ID, "btn-outline-warning", "fas fa-level-down-alt"));

            model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Assign), model.ID, "btn-outline-warning", "fas fa-tag", null, true));
            model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(CreateQuestion), model.ID, "btn-outline-warning", "fas fa-question", "Create a question", true));
            model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Questions), model.ID, "btn-outline-warning", "fas fa-question", "View questions", true));
        }

        public override async Task SetGridItemActions(SurveyDisplayViewModel model)
        {
            await base.SetGridItemActions(model);

            if (model.Active == false)
                model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Activate), model.ID, "btn-outline-warning", "fas fa-level-up-alt"));
            else
                model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Inactivate), model.ID, "btn-outline-warning", "fas fa-level-down-alt"));

            model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Assign), model.ID, "btn-outline-warning", "fas fa-tag", null, true));

            var dropdownActions = new List<string>();
            var dropdownButton = GridItemActionBuilder.BuildDropdownButton(model.ID, "Q&A", "Questions & answers", "btn-outline-warning");
            var createQuestion = GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(CreateQuestion), model.ID, "btn-warning", "fas fa-question", "Create a question", true, true);
            var questions = GridItemActionBuilder.BuildCustomAction(APPAreas.Surveys, CurrentControllerName, nameof(Questions), model.ID, "btn-warning", "fas fa-question", "View questions", true, true);
            dropdownActions.Add(createQuestion);
            dropdownActions.Add(questions);
            model.ActionsHtml.Add(GridItemActionBuilder.PopulateDropdownButton(model.ID, dropdownButton, dropdownActions));
        }

        protected override async Task PopulateViewData(SurveyPostViewModel model)
        {
            await base.PopulateViewData(model);

            var participantsSelectList = await GetAssignedUsersSelectList(model);
            ViewData[nameof(model.SurveyParticipantsIDs)] = participantsSelectList;

            foreach (var participant in participantsSelectList)
            {
                model.SurveyParticipantsIDs.Add(long.Parse(participant.Value));
            }

            var typesSelectList = await GetSurveyTypesSelectList(model);
            ViewData[nameof(model.SurveyTypeID)] = typesSelectList;
        }

        protected override void PopulateViewDataForIndex()
        {
            base.PopulateViewDataForIndex();

            ViewBag.TypesURL = Url.Action(nameof(Index), nameof(SurveyTypesController).Replace("Controller", ""));
            ViewBag.TypesFAIcon = "fas fa-poll";
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

        protected override void Normalize(SurveyPostViewModel model)
        {
        }
    }
}
