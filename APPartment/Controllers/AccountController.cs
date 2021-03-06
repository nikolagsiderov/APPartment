﻿using APPartment.Data.Core;
using APPartment.Data.Server.Models.Core;
using APPartment.UI.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.Controllers
{
    public class AccountController : BaseController
    {
        #region Context, Services and Utilities
        private readonly BaseFacade baseFacade;
        #endregion

        public AccountController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            baseFacade = new BaseFacade();
        }

        #region Actions
        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                return RedirectToAction("EnterCreateHomeOptions", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {            
            if (ModelState.IsValid)
            {
                var userExists = baseFacade.Any<User>(x => x.Name == user.Name);
                if (userExists)
                {
                    ModelState.AddModelError("Name", "This username is already taken.");
                    return View(user); 
                }

                baseFacade.Create(user, 0);
                user = baseFacade.GetObject<User>(x => x.Name == user.Name);

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
                return RedirectToAction("EnterCreateHomeOptions", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var userWithPassedCredsExists = baseFacade.GetObject<User>(x => x.Name == user.Name && x.Password == user.Password);
            
            if (userWithPassedCredsExists != null)
            {
                HttpContext.Session.SetString("UserId", userWithPassedCredsExists.Id.ToString());
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
            HttpContext.Session.SetString("UserId", string.Empty);
            HttpContext.Session.SetString("HomeId", string.Empty);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult LoggedIn()
        {
            if (HttpContext.Session.GetString("UserId") != null)
                return View();
            else
                return RedirectToAction("Login");
        }
        #endregion
    }
}