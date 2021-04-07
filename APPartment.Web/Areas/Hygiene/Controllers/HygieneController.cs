using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Hygiene;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;

namespace APPartment.Web.Areas.Hygiene.Controllers
{
    [Area(APPAreas.Hygiene)]
    public class HygieneController : BaseCRUDController<HygieneDisplayViewModel, HygienePostViewModel>
    {
        public HygieneController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<HygieneDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID;

        public override bool CanManage => true;

        [Breadcrumb(HygieneBreadcrumbs.Manage_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override async Task PopulateViewData(HygienePostViewModel model)
        {
        }
    }
}
