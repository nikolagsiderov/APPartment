using APPartment.Common;
using APPartment.Infrastructure.Controllers.Web;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace APPartment.MVC.Web.Areas.Home.Controllers
{
    [Area(APPAreas.Home)]
    public class NotificationsController : BaseController
    {
        public NotificationsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public async Task<JsonResult> GetContents()
        {
            var result = string.Empty;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/contents";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        result = content;
                }
            }

            return Json(result);
        }

        public async Task<JsonResult> GetCount()
        {
            var result = 0;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/count";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        result = int.Parse(content);
                }
            }

            return Json(result);
        }
    }
}