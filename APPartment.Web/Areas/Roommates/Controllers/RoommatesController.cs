using System.Threading.Tasks;
using APPartment.Infrastructure.Controllers.Web;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;


namespace APPartment.Web.Areas.Roommates.Controllers
{
    [Area(APPAreas.Roommates)]
    public class RoommatesController : BaseCRUDController<UserDisplayViewModel, UserPostViewModel>
    {
        public RoommatesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => false;

        [Breadcrumb(RoommatesBreadcrumbs.Index_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override void Normalize(UserPostViewModel model)
        {
        }
    }
}