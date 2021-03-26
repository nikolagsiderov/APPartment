using System;
using System.Linq.Expressions;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels;
using APPartment.UI.ViewModels.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace APPartment.UI.Controllers.Base
{
    public abstract class BaseReadOnlyController<T, U> : BaseAuthorizeController
        where T : GridItemViewModelWithHome, new()
        where U : PostViewModelWithHome, new()
    {
        public BaseReadOnlyController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public abstract Expression<Func<T, bool>> FilterExpression { get; }

        [Breadcrumb("Base")]
        public virtual IActionResult Index()
        {
            var models = BaseWebService.GetCollection<T>(FilterExpression);
            return View("_GridReadOnly", models);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Details_Breadcrumb)]
        public IActionResult Details(long? ID)
        {
            if (ID == null)
                return new Error404NotFoundViewResult();

            var model = BaseWebService.GetEntity<U>((long)ID);

            if (model == null)
                return new Error404NotFoundViewResult();

            ViewData["ReadOnly"] = true;

            return View("_Details", model);
        }
    }
}