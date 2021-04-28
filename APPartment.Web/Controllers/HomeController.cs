using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using APPartment.Common;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.UI.Common.ViewModels.Home;
using APPartment.Infrastructure.UI.Common.ViewModels;
using APPartment.Infrastructure.Controllers.Web;

namespace APPartment.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [DefaultBreadcrumb(HomeBreadcrumbs.Default_Breadcrumb)]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentUserID")))
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentHomeID")))
                return RedirectToAction("Login");

            var model = new HomePageDisplayModel();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/page";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<HomePageDisplayModel>(content);
                }
            }

            ViewData["CurrentUsername"] = CurrentUserName;

            return View(model);
        }

        public IActionResult EnterCreateHomeOptions()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentHomeID")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentHomeID")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(HomePostViewModel home)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/{nameof(Register)}";
                    httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, home))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            home = JsonConvert.DeserializeObject<HomePostViewModel>(content);
                        else
                        {
                            ModelState.AddModelError("Name", "This home name is already taken.");
                            return View(home);
                        }
                    }
                }

                ModelState.Clear();

                HttpContext.Session.SetString("CurrentHomeID", home.ID.ToString());
                HttpContext.Session.SetString("CurrentHomeName", home.Name.ToString());

                await SetUserToCurrentHome(home.ID);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentHomeID")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(HomePostViewModel home)
        {
            home.ConfirmPassword = home.Password;
            ModelState.Clear();

            if (string.IsNullOrEmpty(home.Name))
                ModelState.AddModelError("Name", "Home name field is required.");
            else if (string.IsNullOrEmpty(home.Password))
                ModelState.AddModelError("Password", "Password field is required.");
            else
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/{nameof(Login)}";
                    httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, home))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            home = JsonConvert.DeserializeObject<HomePostViewModel>(content);

                            if (home != null)
                            {
                                HttpContext.Session.SetString("CurrentHomeID", home.ID.ToString());
                                HttpContext.Session.SetString("CurrentHomeName", home.Name.ToString());

                                await SetUserToCurrentHome(home.ID);

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Home name or password is wrong.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Home name or password is wrong.");
                        }
                    }
                }
            }

            return View();
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.About_Breadcrumb)]
        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(string username, string messageText)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/chat?username={username}&messageText={messageText}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, new { }))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok();
                    }
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestID = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task SetUserToCurrentHome(long homeID)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/homeuser";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", homeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, new { }))
                {
                }
            }
        }
    }
}
