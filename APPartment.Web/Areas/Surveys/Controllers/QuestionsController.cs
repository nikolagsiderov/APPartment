using System.Threading.Tasks;
using APPartment.Infrastructure.Controllers.Web;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.Web.Areas.Surveys.Controllers
{
    [Area(APPAreas.Surveys)]
    public class QuestionsController : BaseCRUDController<SurveyQuestionDisplayViewModel, SurveyQuestionPostViewModel>
    {
        private long currentSurveyID;

        public QuestionsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            GetCurrentSurveyIDFromQuery();
        }

        private void GetCurrentSurveyIDFromQuery()
        {
            var valueString = Request.RouteValues["id"].ToString();

            if (string.IsNullOrEmpty(valueString))
                this.currentSurveyID = long.Parse(valueString);
        }

        public override bool CanManage => true;

        [Breadcrumb(SurveysBreadcrumbs.Questions_Breadcrumb)]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

        protected override void Normalize(SurveyQuestionPostViewModel model)
        {
            if (model.IsNew)
                model.SurveyID = currentSurveyID;
        }
    }
}