using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.Controllers.Web;

namespace APPartment.MVC.Web.Areas.Chores.Controllers
{
    [Area(APPAreas.Chores)]
    public class MineController : BaseCRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public MineController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => false;

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override void Normalize(ChorePostViewModel model)
        {
        }
    }
}
