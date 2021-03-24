﻿using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Inventory;

namespace APPartment.Web.Controllers
{
    public class InventoryController : BaseCRUDController<InventoryDisplayViewModel, InventoryPostViewModel>
    {
        public InventoryController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<InventoryDisplayViewModel, bool>> FilterExpression
        {
            get
            {
                return x => x.HomeID == (long)CurrentHomeID;
            }
        }

        [Breadcrumb(InventoryBreadcrumbs.All_Breadcrumb)]
        public override IActionResult Index()
        {
            ViewData["GridTitle"] = "Inventory - All";
            ViewData["Module"] = "Inventory";
            ViewData["Manage"] = true;

            return base.Index();
        }

        public JsonResult GetCount()
        {
            var inventoryCount = BaseWebService.Count<InventoryPostViewModel>(x => x.HomeID == (long)CurrentHomeID);
            return Json(inventoryCount);
        }

        protected override void PopulateViewData()
        {
        }
    }
}
