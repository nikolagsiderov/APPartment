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
    public class DoneController : BaseCRUDController<HygieneDisplayViewModel, HygienePostViewModel>
    {
        public DoneController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<HygieneDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.IsDone == true;

        public override bool CanManage => false;

        [Breadcrumb(HygieneBreadcrumbs.Done_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override async Task PopulateViewData(HygienePostViewModel model)
        {
        }
    }
}
