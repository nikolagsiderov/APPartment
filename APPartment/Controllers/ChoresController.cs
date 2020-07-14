using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using APPartment.Utilities.Constants.Breadcrumbs;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace APPartment.Controllers
{
    public class ChoresController : BaseCRUDController<Chore>
    {
        private readonly DataAccessContext _context;

        public ChoresController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
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
            ViewData["GridTitle"] = "Chores - All";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = true;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return await base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Others_Breadcrumb)]
        public async Task<IActionResult> Others()
        {
            ViewData["GridTitle"] = "Chores - Others";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.AssignedToId != CurrentUserId);

            return await base.Index();
        }

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public async Task<IActionResult> Mine()
        {
            ViewData["GridTitle"] = "Chores - Mine";
            ViewData["Module"] = "Chores";
            ViewData["Manage"] = false;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId && x.AssignedToId == CurrentUserId);

            return await base.Index();
        }

        public JsonResult GetMyChoresCount()
        {
            var myChoresCount = _context.Set<Chore>().ToList().Where(x => x.HomeId == CurrentHomeId && x.AssignedToId == CurrentUserId).Count();
            return Json(myChoresCount);
        }
        #endregion

        public override void PopulateViewData()
        {
        }
    }
}
