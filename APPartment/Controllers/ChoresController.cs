using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels;
using APPartment.UI.ViewModels.Chore;
using APPartment.UI.ViewModels.User;

namespace APPartment.Controllers
{
    public class ChoresController : BaseCRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public ChoresController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<ChoreDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeId == CurrentHomeId;
            }
        }

        #region Actions
        [Breadcrumb(ChoresBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Chores - All";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = true;

            return base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Others_Breadcrumb)]
        public IActionResult Others()
        {
            ViewData["GridTitle"] = "Chores - Others";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            return base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public IActionResult Mine()
        {
            ViewData["GridTitle"] = "Chores - Mine";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public IActionResult Assign(string username, long choreId)
        {
            var model = BaseWebService.GetEntity<ChorePostViewModel>(choreId);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            var userToAssign = BaseWebService.GetEntity<UserPostViewModel>(x => x.Name == username);
            var userToAssignUserId = userToAssign.Id;

            model.AssignedToUserId = userToAssignUserId;
            BaseWebService.Save(model);

            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetMyChoresCount()
        {
            var myChoresCount = BaseWebService.Count<ChorePostViewModel>(x => x.HomeId == (long)CurrentHomeId && x.AssignedToUserId == (long)CurrentUserId);
            return Json(myChoresCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
