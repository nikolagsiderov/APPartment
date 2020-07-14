using System.Linq;
using APPartment.Controllers.Base;
using APPartment.Core;
using APPartment.Data;
using APPartment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace APPartment.Controllers
{
    public class AccountController : BaseController
    {
        #region Context, Services and Utilities
        private readonly DataAccessContext _context;
        private DataContext<User> dataContext;
        #endregion

        public AccountController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
        {
            _context = context;
            dataContext = new DataContext<User>(_context);
        }

        #region Actions
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Register(User user)
        {            
            if (ModelState.IsValid)
            {
                var usernameAlreadyExists = _context.Users.Any(x => x.Username == user.Username);
                if (usernameAlreadyExists)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(user); 
                }

                dataContext.Save(user, 0, 0, null);

                ModelState.Clear();

                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Username", user.Username.ToString());

                return RedirectToAction("EnterCreateHomeOptions", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var userIsContainedInDb = _context.Users.Any(x => x.Username == user.Username && x.Password == user.Password);
            
            if (userIsContainedInDb)
            {
                var usr = _context.Users.Single(u => u.Username == user.Username && u.Password == user.Password);

                HttpContext.Session.SetString("UserId", usr.UserId.ToString());
                HttpContext.Session.SetString("Username", usr.Username.ToString());

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