using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Hygiene;
using APPAreas = APPartment.UI.Utilities.Constants.Areas;

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
        public override IActionResult Index()
        {
            return base.Index();
        }

        public JsonResult GetCount()
        {
            var count = BaseWebService.Count<HygienePostViewModel>(x => x.HomeID == (long)CurrentUserID && x.IsDone == true);
            return Json(count);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
