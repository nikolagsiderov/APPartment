using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.Web.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class SurveyTypesController : BaseCRUDController<SurveyTypeDisplayViewModel, SurveyTypePostViewModel>
    {
        public SurveyTypesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => true;

        [Breadcrumb(SurveysBreadcrumbs.Types_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }
    }
}