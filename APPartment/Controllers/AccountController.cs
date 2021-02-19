using APPartment.Data.Server.Models.Core;
using APPartment.ORM.Framework.Core;
using APPartment.UI.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.Controllers
{
    public class AccountController : BaseController
    {
        #region Context, Services and Utilities
        private DaoContext dao;
        #endregion

        public AccountController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            dao = new DaoContext();
        }

        #region Actions
        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("EnterCreateHomeOptions", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {            
            if (ModelState.IsValid)
            {
                var alreadyExistingUser = dao.GetObject(user, x => x.Name == user.Name);
                if (alreadyExistingUser != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(user); 
                }

                dao.Create(user);
                user = dao.GetObject(user, x => x.Name == user.Name);

                ModelState.Clear();

                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Name.ToString());

                return RedirectToAction("EnterCreateHomeOptions", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("EnterCreateHomeOptions", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var existingUserWithPassedCredentials = dao.GetObject(user, x => x.Name == user.Name && x.Password == user.Password);
            
            if (existingUserWithPassedCredentials != null)
            {
                HttpContext.Session.SetString("UserId", existingUserWithPassedCredentials.Id.ToString());
                HttpContext.Session.SetString("Username", existingUserWithPassedCredentials.Name.ToString());

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
            HttpContext.Session.SetString("UserId", string.Empty);
            HttpContext.Session.SetString("HomeId", string.Empty);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult LoggedIn()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        #endregion
    }
}