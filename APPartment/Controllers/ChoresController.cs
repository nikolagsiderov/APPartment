using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.Data.Server.Models.Objects;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels;
using APPartment.Data.Server.Models.Core;

namespace APPartment.Controllers
{
    public class ChoresController : BaseCRUDController<Chore>
    {
        public ChoresController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<Chore, bool>> FilterExpression { get; set; }

        public override Expression<Func<Chore, bool>> FuncToExpression(Func<Chore, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(ChoresBreadcrumbs.All_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Chores - All";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = true;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return await base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Others_Breadcrumb)]
        public async Task<IActionResult> Others()
        {
            ViewData["GridTitle"] = "Chores - Others";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.AssignedToUserId != CurrentUserId);

            return await base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public async Task<IActionResult> Mine()
        {
            ViewData["GridTitle"] = "Chores - Mine";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.AssignedToUserId == CurrentUserId);

            return await base.Index();
        }

        public async Task<IActionResult> Assign(string username, long choreId)
        {
            var searchedChore = new Chore() { Id = choreId };
            var searchedUser = new User() { Name = username };
            var model = dao.GetObject(searchedChore, x => x.Id == searchedChore.Id);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            var userToAssign = dao.GetObject(searchedUser, x => x.Name == searchedUser.Name);
            var userToAssignUserId = userToAssign.Id;

            model.AssignedToUserId = userToAssignUserId;
            dao.Update(model);

            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetMyChoresCount()
        {
            var searchedChore = new Chore() { HomeId = (long)CurrentHomeId, AssignedToUserId = CurrentUserId };
            var myChoresCount = dao.GetObjects(searchedChore, x => x.HomeId == searchedChore.HomeId && x.AssignedToUserId == searchedChore.AssignedToUserId).Count();
            return Json(myChoresCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
