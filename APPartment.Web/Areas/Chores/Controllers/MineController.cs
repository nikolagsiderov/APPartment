using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Chore;
using APPAreas = APPartment.UI.Utilities.Constants.Areas;

namespace APPartment.Web.Areas.Chores.Controllers
{
    [Area(APPAreas.Chores)]
    public class MineController : BaseCRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public MineController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<ChoreDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID && x.AssignedToUserID == (long)CurrentUserID;
            }
        }

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["Module"] = APPAreas.Chores;
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var count = BaseWebService.Count<ChorePostViewModel>(x => x.HomeID == (long)CurrentHomeID && x.AssignedToUserID == (long)CurrentUserID);
            return Json(count);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
