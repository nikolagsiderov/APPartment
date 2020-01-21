using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APPartment.Models;
using SmartBreadcrumbs.Attributes;
using APPartment.Data;
using Microsoft.AspNetCore.Http;
using APPartment.Models.Base;

namespace APPartment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataAccessContext _context;

        public HomeController(ILogger<HomeController> logger, DataAccessContext context)
        {
            _logger = logger;
            _context = context;
        }

        [DefaultBreadcrumb("<i class='fas fa-home' style='font-size:20px'></i> My Home")]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var displayObjects = GetDisplayObject(currentHouseId);

            return View(displayObjects);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(House house)
        {
            if (ModelState.IsValid)
            {
                house.CreatedBy = _context.User.Find(long.Parse(HttpContext.Session.GetString("UserId"))).Username;
                house.CreatedDate = DateTime.Now;
                house.ModifiedBy = house.CreatedBy;
                house.ModifiedDate = house.CreatedDate;

                _context.Add(house);
                _context.SaveChanges();

                ModelState.Clear();

                HttpContext.Session.SetString("HouseId", house.Id.ToString());
                HttpContext.Session.SetString("HouseName", house.Name.ToString());

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(House house)
        {
            var houseIsContainedInDb = _context.House.Any(x => x.Name == house.Name && x.Password == house.Password);

            if (houseIsContainedInDb)
            {
                var home = _context.House.Single(h => h.Name == house.Name && h.Password == house.Password);

                HttpContext.Session.SetString("HouseId", home.Id.ToString());
                HttpContext.Session.SetString("HouseName", home.Name.ToString());

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "House name or password is wrong.");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<BaseObject> GetDisplayObject(long? currentHouseId)
        {
            var displayObjects = new List<BaseObject>();
            var lastInventoryObject = new Inventory();
            var lastHygieneObject = new Hygiene();
            var lastIssueObject = new Issue();

            var inventoryObjects = _context.Set<Inventory>().Where(x => x.HouseId == currentHouseId);

            if (inventoryObjects.Count() > 0)
            {
                lastInventoryObject = inventoryObjects.OrderByDescending(x => x.Id).First();
            }

            var hygieneObjects = _context.Set<Hygiene>().Where(x => x.HouseId == currentHouseId);

            if (hygieneObjects.Count() > 0)
            {
                lastHygieneObject = hygieneObjects.OrderByDescending(x => x.Id).First();
            }

            var issueObjects = _context.Set<Issue>().Where(x => x.HouseId == currentHouseId);

            if (issueObjects.Count() > 0)
            {
                lastIssueObject = issueObjects.OrderByDescending(x => x.Id).First();
            }

            displayObjects.Add(lastInventoryObject);
            displayObjects.Add(lastHygieneObject);
            displayObjects.Add(lastIssueObject);

            return displayObjects;
        }
    }
}
