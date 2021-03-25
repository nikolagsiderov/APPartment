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
    [Area("Hygiene")]
    public class InProgressController : BaseCRUDController<HygieneDisplayViewModel, HygienePostViewModel>
    {
        public InProgressController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<HygieneDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID && x.IsDone == false;
            }
        }

        [Breadcrumb(HygieneBreadcrumbs.InProgress_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["Module"] = APPAreas.Hygiene;
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var count = BaseWebService.Count<HygienePostViewModel>(x => x.HomeID == (long)CurrentUserID && x.IsDone == false);
            return Json(count);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
