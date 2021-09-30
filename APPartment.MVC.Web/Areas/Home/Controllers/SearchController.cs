using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using APPartment.Common;
using APPartment.Infrastructure.Controllers.Web;
using APPartment.Infrastructure.UI.Common.ViewModels;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APPartment.MVC.Web.Areas.Home.Controllers
{
    [Area(APPAreas.Home)]
    public class SearchController : BaseAuthorizeController
    {
        public SearchController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public async Task<IActionResult> Search(string keyWords)
        {
            var watch = new Stopwatch();
            watch.Start();

            var objects = new List<BusinessObjectDisplayViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{APPAreas.Home}/home/objects/search/{keyWords}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        objects = JsonConvert.DeserializeObject<List<BusinessObjectDisplayViewModel>>(content);
                }
            }

            var model = new GeneralSearchViewModel(objects, keyWords);
            watch.Stop();
            model.ElapsedTime = (double)watch.ElapsedMilliseconds / 1000;

            return View("_Search", model);
        }
    }
}