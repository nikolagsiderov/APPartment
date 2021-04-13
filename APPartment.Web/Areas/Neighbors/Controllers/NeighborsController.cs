using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Common.ViewModels.Home;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.Web.Areas.Neighbors.Controllers
{
    [Area(APPAreas.Neighbors)]
    public class Neighbors : BaseCRUDController<HomeDisplayViewModel, HomePostViewModel>
    {
        public Neighbors(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => false;

        [Breadcrumb(NeighborsBreadcrumbs.Index_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }
    }
}