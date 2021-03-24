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
using System.Security.Cryptography.X509Certificates;

namespace APPartment.Web.Controllers
{
    public class DoneChoresController : BaseCRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public DoneChoresController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<ChoreDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID && x.IsDone == true;
            }
        }

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Chores - Done";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var doneChoresCount = BaseWebService.Count<ChorePostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.AssignedToUserID == (long)CurrentUserID && x.IsDone == true);
            return Json(doneChoresCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
