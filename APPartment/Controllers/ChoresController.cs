using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using APPartment.Utilities.Constants.Breadcrumbs;
using System;
using System.Linq.Expressions;

namespace APPartment.Controllers
{
    public class ChoresController : BaseCRUDController<Chore>
    {
        private readonly DataAccessContext _context;

        public ChoresController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        public override Expression<Func<Chore, bool>> FilterExpression { get; set; }

        public override Expression<Func<Chore, bool>> FuncToExpression(Func<Chore, bool> f)
        {
            return x => f(x);
        }

        #region Actions
        [Breadcrumb(ChoresBreadcrumbs.All_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["GridTitle"] = "Chores - All";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = true;

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            FilterExpression = FuncToExpression(x => x.HouseId == currentHouseId);

            return await base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Others_Breadcrumb)]
        public async Task<IActionResult> Others()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

            ViewData["GridTitle"] = "Chores - Others";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HouseId == currentHouseId && x.AssignedToId != currentUserId);

            return await base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public async Task<IActionResult> Mine()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

            ViewData["GridTitle"] = "Chores - Mine";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HouseId == currentHouseId && x.AssignedToId == currentUserId);

            return await base.Index();
        }

        public JsonResult GetMyChoresCount()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));
                var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

                var myChoresCount = _context.Set<Chore>().ToList().Where(x => x.HouseId == currentHouseId && x.AssignedToId == currentUserId).Count();

                return Json(myChoresCount);
            }

            return Json(0);
        }
        #endregion

        public override void PopulateViewData()
        {
        }
    }
}
