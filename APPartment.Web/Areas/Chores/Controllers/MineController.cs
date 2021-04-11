﻿using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;

namespace APPartment.Web.Areas.Chores.Controllers
{
    [Area(APPAreas.Chores)]
    public class MineController : BaseCRUDController<ChoreDisplayViewModel, ChorePostViewModel>
    {
        public MineController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override Expression<Func<ChoreDisplayViewModel, bool>> FilterExpression => x => x.HomeID == CurrentHomeID && x.AssignedToUserID == (long)CurrentUserID;

        public override bool CanManage => false;

        [Breadcrumb(ChoresBreadcrumbs.Mine_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }
    }
}
