using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System.Linq;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.Data.Server.Models.Objects;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.Enums;

namespace APPartment.Controllers
{
    public class HygieneController : BaseCRUDController<Hygiene>
    {
        public HygieneController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<Hygiene, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeId == CurrentHomeId;
            }
        }

        #region Actions
        [Breadcrumb(HygieneBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Hygiene - All";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = true;

            return base.Index();
        }

        [Breadcrumb(HygieneBreadcrumbs.Cleaned_Breadcrumb)]
        public IActionResult Cleaned()
        {
            ViewData["GridTitle"] = "Hygiene - Cleaned";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            return base.Index();
        }

        [Breadcrumb(HygieneBreadcrumbs.Due_Cleaning_Breadcrumb)]
        public IActionResult DueCleaning()
        {
            ViewData["GridTitle"] = "Hygiene - Due Cleaning";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            return base.Index();
        }

        public JsonResult GetHygieneCriticalCount()
        {
            var hygieneCriticalCount = baseFacade.Count<Hygiene>(x => x.HomeId == (long)CurrentUserId);

            return Json(hygieneCriticalCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
