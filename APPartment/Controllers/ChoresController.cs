using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using APPartment.Utilities.Constants.Breadcrumbs;

namespace APPartment.Controllers
{
    public class ChoresController : BaseCRUDController<Chore>
    {
        private readonly DataAccessContext _context;

        public ChoresController(DataAccessContext context) : base(context)
        {
            _context = context;
        }

        #region Actions
        [Breadcrumb(ChoresBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Chores - All";
            ViewData["Module"] = "Chores";

            return base.Index();
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

            var modelObjects = await _context.Set<Chore>().Where(x => x.HouseId == currentHouseId && x.AssignedToId != currentUserId).ToListAsync();

            ViewData["GridTitle"] = "Chores - Others";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;
            ViewData["Statuses"] = baseService.GetStatuses(typeof(Chore));

            return View("_Grid", modelObjects);
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

            var modelObjects = await _context.Set<Chore>().Where(x => x.HouseId == currentHouseId && x.AssignedToId != currentUserId).ToListAsync();

            ViewData["GridTitle"] = "Chores - Mine";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;
            ViewData["Statuses"] = baseService.GetStatuses(typeof(Chore));

            return View("_Grid", modelObjects);
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
