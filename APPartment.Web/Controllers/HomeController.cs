﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using APPartment.Common;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.UI.Common.ViewModels.Home;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
                return RedirectToAction("Login");

            var model = new HomePageDisplayModel();
            var status = new HomeStatusPostViewModel();

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

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/status";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        status = JsonConvert.DeserializeObject<HomeStatusPostViewModel>(content);
                }
            }

            model.HomeStatus = status;
            ViewData["Username"] = CurrentUserName;

            return View(model);
        }

        public IActionResult EnterCreateHomeOptions()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
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

                HttpContext.Session.SetString("HomeID", home.ID.ToString());
                HttpContext.Session.SetString("HomeName", home.Name.ToString());

                await SetUserToCurrentHome(home.ID);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
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
                                HttpContext.Session.SetString("HomeID", home.ID.ToString());
                                HttpContext.Session.SetString("HomeName", home.Name.ToString());

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
        [Breadcrumb(HomeBreadcrumbs.Settings_Breadcrumb)]
        public async Task<IActionResult> Settings()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
                return RedirectToAction("Login", "Home");

            var settings = new HomeSettingPostViewModel();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/settings";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        settings = JsonConvert.DeserializeObject<HomeSettingPostViewModel>(content);
                }
            }

            if (settings != null)
                return View(settings);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Settings(HomeSettingPostViewModel settings)
        {
            if (settings.RentDueDateDay <= 0 || settings.RentDueDateDay >= 28)
            {
                ModelState.AddModelError("RentDueDateDay", "Rent due date day value can only be between 1 and 27, included.");
                return View(settings);
            }

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/settings";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, settings))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        settings = JsonConvert.DeserializeObject<HomeSettingPostViewModel>(content);

                        if (settings.ChangeHttpSession)
                            HttpContext.Session.SetString("HomeName", settings.HomeName);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
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

        public async Task<JsonResult> GetHomeStatus()
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
            {
                var status = new HomeStatusPostViewModel();
                var user = new UserPostViewModel();

                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/status";
                    httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                    httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                    using (var response = await httpClient.GetAsync(requestUri))
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            status = JsonConvert.DeserializeObject<HomeStatusPostViewModel>(content);
                    }
                }

                if (status.ID > 0)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var requestUri = $"{Configuration.DefaultAPI}/users/{status.UserID}";
                        httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                        httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                        using (var response = await httpClient.GetAsync(requestUri))
                        {
                            string content = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                                user = JsonConvert.DeserializeObject<UserPostViewModel>(content);
                        }
                    }

                    result = $"{status.Status};{user.Name};{status.Details}";
                    return Json(result);
                }
            }

            result = $"1;system_generated;No one has set a status yet!";
            return Json(result);
        }

        public async Task<ActionResult> SetHomeStatus(string homeStatusString, string homeStatusDetailsString)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/status?homeStatusString={homeStatusString}&homeStatusDetailsString={homeStatusDetailsString}";
                    httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                    httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, new { }))
                    {
                    }
                }
            }

            return Ok();
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
