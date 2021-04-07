using APPartment.Common;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using APPartment.Infrastructure.UI.Web.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace APPartment.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                return RedirectToAction("EnterCreateHomeOptions", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserPostViewModel user)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/{nameof(Register)}";

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

                HttpContext.Session.SetString("UserID", user.ID.ToString());
                HttpContext.Session.SetString("Username", user.Name.ToString());

                return RedirectToAction("EnterCreateHomeOptions", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                return RedirectToAction("EnterCreateHomeOptions", "Home");

            return View();
        }

        [HttpPost]
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
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/{nameof(Login)}";

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, user))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            user = JsonConvert.DeserializeObject<UserPostViewModel>(content);

                            if (user != null)
                            {
                                HttpContext.Session.SetString("UserID", user.ID.ToString());
                                HttpContext.Session.SetString("Username", user.Name.ToString());

                                return RedirectToAction("EnterCreateHomeOptions", "Home");
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
            HttpContext.Session.SetString("UserID", string.Empty);
            HttpContext.Session.SetString("HomeID", string.Empty);

            return RedirectToAction("Index", "Home");
        }
    }
}