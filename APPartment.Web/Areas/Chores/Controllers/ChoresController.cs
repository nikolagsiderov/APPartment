using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using APPartment.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.UI.Web.Html;
using APPartment.Infrastructure.Controllers.Web;

namespace APPartment.Web.Areas.Chores.Controllers
{
    [Area(APPAreas.Chores)]
    public class ChoresController : BaseCRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public ChoresController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => true;

        [Breadcrumb(ChoresBreadcrumbs.Manage_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        [HttpGet]
        public async Task<IActionResult> Assign(long id)
        {
            var model = new ChorePostViewModel();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/{id}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<ChorePostViewModel>(content);
                }
            }

            var users = await GetAssignedUsersSelectList(model);
            ViewData["AssignedToUserID"] = users;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(string ID, string assignedUserID)
        {
            var model = new ChorePostViewModel();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/{long.Parse(ID)}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<ChorePostViewModel>(content);
                }
            }

            model.AssignedToUserID = long.Parse(assignedUserID);

            return await base.Edit(model.ID, model);
        }

        public override async Task SetObjectActions(ChorePostViewModel model)
        {
            model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomAction(APPAreas.Chores, CurrentControllerName, nameof(Assign), model.ID, "btn-outline-warning", "fas fa-tag"));
            await base.SetObjectActions(model);
        }

        public override async Task SetGridItemActions(ChoreDisplayViewModel model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(APPAreas.Chores, CurrentControllerName, nameof(Assign), model.ID, "btn-outline-warning", "fas fa-tag"));
            await base.SetGridItemActions(model);
        }

        protected override async Task PopulateViewData(ChorePostViewModel model)
        {
            await base.PopulateViewData(model);

            var users = await GetAssignedUsersSelectList(model);
            ViewData["AssignedToUserID"] = users;
        }

        private async Task<List<SelectListItem>> GetAssignedUsersSelectList(ChorePostViewModel model)
        {
            var users = await GetUsersInCurrentHome();

            var usersSelectList = users.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();
            usersSelectList.Insert(0, new SelectListItem() { Text = "Please select a user...", Value = null });

            if (model.ID > 0)
            {
                foreach (var item in usersSelectList)
                {
                    if (item.Value == model.AssignedToUserID.ToString())
                    {
                        item.Selected = true;
                    }
                }
            }

            return usersSelectList;
        }
    }
}
