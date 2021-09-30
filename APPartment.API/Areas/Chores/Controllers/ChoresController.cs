using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using APPArea = APPartment.Infrastructure.Attributes.AreaAttribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using APPartment.Infrastructure.Controllers.Api;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Web.Html;

namespace APPartment.API.Controllers
{
    [APPArea(APPAreas.Chores)]
    public class ChoresController : BaseAPICRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public ChoresController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected override Expression<Func<ChoreDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID;

        // TODO: Convert to proper endpoints
        //[HttpGet]
        //public async Task<IActionResult> Assign(long id)
        //{
        //    var model = BaseCRUDService.GetEntity<ChorePostViewModel>(id);

        //    //var users = await GetAssignedUsersSelectList(model);
        //    //ViewData[nameof(model.AssignedToUserID)] = users;

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Assign(string ID, string assignedUserID)
        //{
        //    var model = new ChorePostViewModel();

        //    using (var httpClient = new HttpClient())
        //    {
        //        var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/{long.Parse(ID)}";
        //        httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
        //        httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

        //        using (var response = await httpClient.GetAsync(requestUri))
        //        {
        //            string content = await response.Content.ReadAsStringAsync();

        //            if (response.IsSuccessStatusCode)
        //                model = JsonConvert.DeserializeObject<ChorePostViewModel>(content);
        //        }
        //    }

        //    model.AssignedToUserID = long.Parse(assignedUserID);

        //    return await base.Edit(model.ID, model);
        //}

        protected override void NormalizeDisplayModel(ChoreDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(ChorePostViewModel model)
        {
        }

        public override async Task SetObjectActions(ChorePostViewModel model)
        {
            model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomAction(CurrentArea, "Assign", model.ID, "btn-outline-warning", "fas fa-tag"));
            await base.SetObjectActions(model);
        }

        public override async Task SetGridItemActions(ChoreDisplayViewModel model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomAction(CurrentArea, "Assign", model.ID, "btn-outline-warning", "fas fa-tag"));
            await base.SetGridItemActions(model);
        }
    }
}
