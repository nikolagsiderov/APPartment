using System.Linq;
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
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Chores - All";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = true;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Others_Breadcrumb)]
        public IActionResult Others()
        {
            ViewData["GridTitle"] = "Chores - Others";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.AssignedToUserId != CurrentUserId);

            return base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public IActionResult Mine()
        {
            ViewData["GridTitle"] = "Chores - Mine";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.AssignedToUserId == CurrentUserId);

            return base.Index();
        }

        public IActionResult Assign(string username, long choreId)
        {
            var model = baseFacade.GetObject<Chore>(choreId);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            var userToAssign = baseFacade.GetObject<User>(x => x.Name == username);
            var userToAssignUserId = userToAssign.Id;

            model.AssignedToUserId = userToAssignUserId;
            baseFacade.Update(model);

            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetMyChoresCount()
        {
            var myChoresCount = baseFacade.GetObjects<Chore>(x => x.HomeId == (long)CurrentHomeId && x.AssignedToUserId == (long)CurrentUserId).Count();
            return Json(myChoresCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
