﻿using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using APPartment.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using APPartment.Infrastructure.UI.Web.Html;

namespace APPartment.Web.Areas.Chores.Controllers
{
    [Area(APPAreas.Chores)]
    public class ChoresController : BaseCRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public ChoresController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<ChoreDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID;

        public override bool CanManage => true;

        [Breadcrumb(ChoresBreadcrumbs.Manage_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        public async Task<IActionResult> Assign(long modelID, long userID)
        {
            return Ok();
        }

        public override async Task SetObjectActions(ChorePostViewModel model)
        {
            var users = new List<UserPostViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/users/home/{CurrentHomeID}";

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        users = JsonConvert.DeserializeObject<List<UserPostViewModel>>(content);
                }
            }

            var options = users.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();
            options.Insert(0, new SelectListItem() { Text = "Please select a user...", Value = null });

            model.ActionsHtml.Add(ObjectActionBuilder.BuildCustomActionWithDropdownWithModal(APPAreas.Chores, CurrentControllerName, nameof(Assign), model.ID, "Assign", "Assign to user", "btn-outline-warning", "fas fa-tag", options));

            await base.SetObjectActions(model);
        }

        public override async Task SetGridItemActions(ChoreDisplayViewModel model)
        {
            var users = new List<UserPostViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/users/home/{CurrentHomeID}";

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        users = JsonConvert.DeserializeObject<List<UserPostViewModel>>(content);
                }
            }

            var options = users.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();
            options.Insert(0, new SelectListItem() { Text = "Please select a user...", Value = null });

            model.ActionsHtml.Add(GridItemActionBuilder.BuildCustomActionWithDropdownWithModal(APPAreas.Chores, CurrentControllerName, nameof(Assign), model.ID, "Assign", "Assign to user", "btn-outline-warning", "fas fa-tag", options));

            await base.SetGridItemActions(model);
        }

        protected override async Task PopulateViewData(ChorePostViewModel model)
        {
            var users = new List<UserPostViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/users/home/{CurrentHomeID}";

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        users = JsonConvert.DeserializeObject<List<UserPostViewModel>>(content);
                }
            }

            var selectList = users.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();
            selectList.Insert(0, new SelectListItem() { Text = "Please select a user...", Value = null });

            if (model.ID > 0)
            {
                foreach (var item in selectList)
                {
                    if (item.Value == model.AssignedToUserID.ToString())
                    {
                        item.Selected = true;
                    }
                }
            }

            ViewData["AssignedToUserID"] = selectList;
        }
    }
}
