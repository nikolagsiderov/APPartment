using APPartment.UI.Controllers.Base;
using APPartment.UI.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Register(UserPostViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userExists = BaseWebService.Any<UserPostViewModel>(x => x.Name == user.Name);

                if (userExists)
                {
                    ModelState.AddModelError("Name", "This username is already taken.");
                    return View(user);
                }

                BaseWebService.Save(user);
                user = BaseWebService.GetEntity<UserPostViewModel>(x => x.Name == user.Name);

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
        public IActionResult Login(UserPostViewModel user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError("", "Username or password cannot be empty.");
                return View();
            }

            var userWithPassedCredsExists = BaseWebService.GetEntity<UserPostViewModel>(x => x.Name == user.Name && x.Password == user.Password);

            if (userWithPassedCredsExists != null)
            {
                HttpContext.Session.SetString("UserID", userWithPassedCredsExists.ID.ToString());
                HttpContext.Session.SetString("Username", userWithPassedCredsExists.Name.ToString());

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