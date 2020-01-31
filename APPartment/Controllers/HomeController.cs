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
using System.Threading.Tasks;

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

            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentUser = _context.User.Find(currentUserId);

            ViewData["Username"] = currentUser.Username;

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var messages = _context.Message.ToList();

            ViewData["Messages"] = messages;

            var displayObjects = GetDisplayObject(currentHouseId);

            if (_context.HouseSettings.Any(x => x.HouseId == long.Parse(HttpContext.Session.GetString("HouseId"))))
            {
                ViewData["RentDueDate"] = _context.HouseSettings.Find(long.Parse(HttpContext.Session.GetString("HouseId"))).RentDueDate.Date.ToLongDateString();
            }

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

        [HttpGet]
        public IActionResult Settings()
        {
            var houseSettingsArePresent = _context.HouseSettings.Any(x => x.Id == long.Parse(HttpContext.Session.GetString("HouseId")));
            HouseSettings houseSettings = null;

            if (houseSettingsArePresent)
            {
                houseSettings = _context.HouseSettings.Find(long.Parse(HttpContext.Session.GetString("HouseId")));
            }
            
            if (houseSettings != null)
            {
                return View(houseSettings);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Settings(HouseSettings settings)
        {
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            settings.HouseId = currentHouseId;

            if (settings.Id == 0)
            {
                _context.HouseSettings.Add(settings);
            }
            else
            {
                _context.Update(settings);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(string username, string messageText)
        {
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

            var message = new Message() { Username = username, Text = messageText, UserId = currentUserId, CreatedDate = DateTime.Now };

            await _context.Message.AddAsync(message);
            await _context.SaveChangesAsync();

            return Ok();
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
