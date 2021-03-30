using APPartment.UI.Controllers.Base;
using APPartment.UI.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
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
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44310/api/{CurrentControllerName}/{nameof(this.Register)}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (apiResponse.Contains("\"status\":400"))
                    {
                        if (apiResponse.Contains("This username is already taken."))
                        {
                            ModelState.AddModelError("Name", "This username is already taken.");
                            return View(user);
                        }
                        else
                            return View();
                    }
                    else
                    {
                        user = JsonConvert.DeserializeObject<UserPostViewModel>(apiResponse);
                    }
                }
            }

            ModelState.Clear();

            HttpContext.Session.SetString("UserID", user.ID.ToString());
            HttpContext.Session.SetString("Username", user.Name.ToString());

            return RedirectToAction("EnterCreateHomeOptions", "Home");
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
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError("", "Username or password cannot be empty.");
            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://localhost:44310/api/{CurrentControllerName}/{nameof(Login)}?username={user.Name}&password={user.Password}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        try
                        {
                            user = JsonConvert.DeserializeObject<UserPostViewModel>(apiResponse);
                        }
                        catch (System.Exception ex)
                        {
                            ModelState.AddModelError("", "Username or password is wrong.");
                            return View();
                        }
                    }
                }
            }
            

            if (user != null && user.ID > 0)
            {
                HttpContext.Session.SetString("UserID", user.ID.ToString());
                HttpContext.Session.SetString("Username", user.Name.ToString());

                return RedirectToAction("EnterCreateHomeOptions", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username or password is wrong.");
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