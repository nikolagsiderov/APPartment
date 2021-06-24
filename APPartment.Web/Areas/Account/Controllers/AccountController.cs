using APPartment.Common;
using APPartment.Infrastructure.Controllers.Web;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;
using System.Net.Http;
using System.Threading.Tasks;

namespace APPartment.Web.Areas.Account.Controllers
{
    [Area(APPAreas.Account)]
    public class AccountController : BaseCRUDController<UserDisplayViewModel, UserPostViewModel>
    {
        public override bool CanManage => true;

        public AccountController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [Breadcrumb("Users")]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentUserID")))
                return RedirectToAction("EnterCreateHomeOptions", "Home");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserPostViewModel user)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/{nameof(Register)}";

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, user))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            user = JsonConvert.DeserializeObject<UserPostViewModel>(content);
                        else
                        {
                            ModelState.AddModelError("Name", "This username is already taken.");
                            return View(user);
                        }
                    }
                }

                ModelState.Clear();

                HttpContext.Session.SetString("CurrentUserID", user.ID.ToString());
                HttpContext.Session.SetString("CurrentUsername", user.Name.ToString());

                return RedirectToAction("EnterCreateHomeOptions", "Home", new { area = APPAreas.Home });
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentUserID")))
                return RedirectToAction("EnterCreateHomeOptions", "Home");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserPostViewModel user)
        {
            user.ConfirmPassword = user.Password;
            ModelState.Clear();

            if (string.IsNullOrEmpty(user.Name))
                ModelState.AddModelError("Name", "Username field is required.");
            else if (string.IsNullOrEmpty(user.Password))
                ModelState.AddModelError("Password", "Password field is required.");
            else
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentAreaName}/{CurrentControllerName}/{nameof(Login)}";

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, user))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            user = JsonConvert.DeserializeObject<UserPostViewModel>(content);

                            if (user != null)
                            {
                                HttpContext.Session.SetString("CurrentUserID", user.ID.ToString());
                                HttpContext.Session.SetString("CurrentUsername", user.Name.ToString());

                                return RedirectToAction("EnterCreateHomeOptions", "Home", new { area = APPAreas.Home });
                            }
                            else
                            {
                                ModelState.AddModelError("", "Username or password is wrong.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Username or password is wrong.");
                        }
                    }
                }
            }
            
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("CurrentUserID", string.Empty);
            HttpContext.Session.SetString("CurrentHomeID", string.Empty);

            return RedirectToAction("Index", "Home", new { area = APPAreas.Home });
        }
    }
}