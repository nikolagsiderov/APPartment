using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Hygiene;

namespace APPartment.Web.Controllers
{
    public class NotDoneHygieneController : BaseCRUDController<HygieneDisplayViewModel, HygienePostViewModel>
    {
        public NotDoneHygieneController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<HygieneDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == CurrentHomeID && x.IsDone == false; ;
            }
        }

        [Breadcrumb(HygieneBreadcrumbs.Due_Cleaning_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Hygiene - Not Done";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var notDoneHygieneCount = BaseWebService.Count<HygienePostViewModel>(x => x.HomeID == (long)CurrentUserID && x.IsDone == false);
            return Json(notDoneHygieneCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
