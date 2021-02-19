using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        public override Expression<Func<Hygiene, bool>> FilterExpression { get; set; }

        public override Expression<Func<Hygiene, bool>> FuncToExpression(Func<Hygiene, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(HygieneBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Hygiene - All";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = true;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return base.Index();
        }

        [Breadcrumb(HygieneBreadcrumbs.Cleaned_Breadcrumb)]
        public IActionResult Cleaned()
        {
            ViewData["GridTitle"] = "Hygiene - Cleaned";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.Trivial || x.Status == (int)ObjectStatus.Medium));

            return base.Index();
        }

        [Breadcrumb(HygieneBreadcrumbs.Due_Cleaning_Breadcrumb)]
        public IActionResult DueCleaning()
        {
            ViewData["GridTitle"] = "Hygiene - Due Cleaning";
            ViewData["Module"] = "Hygiene";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && (x.Status == (int)ObjectStatus.High || x.Status == (int)ObjectStatus.Critical));

            return base.Index();
        }

        public JsonResult GetHygieneCriticalCount()
        {
            var hygieneCriticalCount = dao.GetObjects<Hygiene>(x => x.HomeId == (long)CurrentUserId).Count();

            return Json(hygieneCriticalCount);
        }
        #endregion

        protected override void PopulateViewData()
        {
        }
    }
}
