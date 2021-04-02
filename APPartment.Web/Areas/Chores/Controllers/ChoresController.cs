using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Chore;
using APPAreas = APPartment.UI.Constants.Areas;
using System.Threading.Tasks;
using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.User;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        protected override void PopulateViewData(ChorePostViewModel model)
        {
            var users = new BaseWebService(0).GetCollection<UserPostViewModel>();
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
