using System.Threading.Tasks;
using APPartment.Infrastructure.Controllers.Web;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.Web.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class QuestionsController : BaseCRUDController<SurveyQuestionDisplayViewModel, SurveyQuestionPostViewModel>
    {
        public QuestionsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override bool CanManage => true;

        [Breadcrumb(SurveysBreadcrumbs.Questions_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }
    }
}