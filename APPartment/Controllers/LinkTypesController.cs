using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APPartment.Data.Core;
using APPartment.Data.Server.Models.Core;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace APPartment.Controllers
{
    public class LinkTypesController : BaseCRUDController<LinkType>
    {
        private readonly DataAccessContext _context;

        public LinkTypesController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
        {
            _context = context;
        }

        public override Expression<Func<LinkType, bool>> FilterExpression { get; set; }

        public override Expression<Func<LinkType, bool>> FuncToExpression(Func<LinkType, bool> f)
        {
            return x => f(x);
        }

        [Breadcrumb(SurveysBreadcrumbs.All_Breadcrumb)]
        public override Task<IActionResult> Index()
        {
            ViewData["GridTitle"] = "Link Types";
            ViewData["Module"] = "Core";
            ViewData["Manage"] = true;

            FilterExpression = FuncToExpression(x => x.HomeId == CurrentHomeId);

            return base.Index();
        }

        protected override void PopulateViewData()
        {
        }
    }
}