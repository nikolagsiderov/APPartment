using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using APPartment.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.UI.Common.ViewModels.GeneralCalendar;
using APPartment.Infrastructure.Controllers.Web;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;

namespace APPartment.MVC.Web.Areas.Home.Controllers
{
    [Area(APPAreas.Home)]
    public class CalendarController : BaseAuthorizeController
    {
        public CalendarController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet]
        [Breadcrumb(CalendarBreadcrumbs.Index)]
        public ActionResult Index()
        {
            return View(CurrentControllerName, new EventViewModel());
        }

        [HttpGet]
        public async Task<JsonResult> GetEvents()
        {
            var result = new List<EventViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/events";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        result = JsonConvert.DeserializeObject<List<EventViewModel>>(content);
                }
            }

            return Json(result.ToArray());
        }
    }
}