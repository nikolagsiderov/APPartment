﻿using System;
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
using APPartment.DisplayModels.Home;
using APPartment.Utilities;
using APPartment.Enums;

namespace APPartment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataAccessContext _context;
        private HtmlRenderHelper htmlRenderHelper = new HtmlRenderHelper();

        public HomeController(ILogger<HomeController> logger, DataAccessContext context)
        {
            _logger = logger;
            _context = context;
        }

        [DefaultBreadcrumb("<i class='fas fa-home' style='font-size:20px'></i> Home")]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentUser = _context.Users.Find(currentUserId);

            ViewData["Username"] = currentUser.Username;

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var displayObjects = GetDisplayObject(currentHouseId);

            if (_context.HouseSettings.Any(x => x.HouseId == long.Parse(HttpContext.Session.GetString("HouseId"))))
            {
                ViewData["RentDueDateDay"] = _context.HouseSettings.Find(long.Parse(HttpContext.Session.GetString("HouseId"))).RentDueDateDay;
            }

            var homeDisplayModel = new HomeDisplayModel()
            {
                Messages = GetMessages(currentHouseId),
                BaseObjects = displayObjects
            };

            return View(homeDisplayModel);
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
                house.CreatedBy = _context.Users.Find(long.Parse(HttpContext.Session.GetString("UserId"))).Username;
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
            var houseIsContainedInDb = _context.Houses.Any(x => x.Name == house.Name && x.Password == house.Password);

            if (houseIsContainedInDb)
            {
                var home = _context.Houses.Single(h => h.Name == house.Name && h.Password == house.Password);

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
        [Breadcrumb("<i class='fas fa-cogs' style='font-size:20px'></i> Settings")]
        public IActionResult Settings()
        {
            var houseSettingsArePresent = _context.HouseSettings.Any(x => x.Id == long.Parse(HttpContext.Session.GetString("HouseId")));
            HouseSettings houseSettings = null;

            if (houseSettingsArePresent)
            {
                houseSettings = _context.HouseSettings.Find(long.Parse(HttpContext.Session.GetString("HouseId")));

                var houseModel = _context.Houses.Find(long.Parse(HttpContext.Session.GetString("HouseId")));
                houseSettings.HouseName = houseModel.Name;
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

            if (!string.IsNullOrEmpty(settings.HouseName))
            {
                var houseModel = _context.Find<House>(currentHouseId);
                houseModel.Name = settings.HouseName;
                HttpContext.Session.SetString("HouseName", houseModel.Name.ToString());
                _context.SaveChanges();
            }

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

        [HttpGet]
        [Breadcrumb("<i class='fas fa-info-circle' style='font-size:20px'></i> About")]
        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(string username, string messageText)
        {
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var message = new Message() { Username = username, Text = messageText, UserId = currentUserId, HouseId = currentHouseId, CreatedDate = DateTime.Now };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public JsonResult GetHomeStatus()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                if (_context.HouseStatuses.Any(x => x.HouseId == currentHouseId))
                {
                    var currentHouseStatusUserId = _context.HouseStatuses.OrderByDescending(x => x.Id).Where(x => x.HouseId == currentHouseId).FirstOrDefault().UserId;
                    var currentHouseStatus = _context.HouseStatuses.OrderByDescending(x => x.Id).Where(x => x.HouseId == currentHouseId).FirstOrDefault().Status;
                    var username = _context.Users.Where(x => x.UserId == currentHouseStatusUserId).FirstOrDefault().Username;
                    var currentHouseStatusDetails = _context.HouseStatuses.OrderByDescending(x => x.Id).Where(x => x.HouseId == currentHouseId).FirstOrDefault().Details;

                    var result = $"{currentHouseStatus};{username};{currentHouseStatusDetails}";

                    return Json(result);
                }
            }

            var elseResult = $"1;system_generated;No one has set a status yet!";

            return Json(elseResult);
        }

        public ActionResult SetHomeStatus(string houseStatusString, string houseStatusDetailsString)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));
                long? currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

                var houseStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(houseStatusDetailsString))
                {
                    houseStatusDetails = houseStatusDetailsString;
                }

                if (_context.HouseStatuses.Any(x => x.HouseId == currentHouseId))
                {
                    var currentHouseStatus = _context.HouseStatuses.OrderByDescending(x => x.Id).Where(x => x.HouseId == currentHouseId).FirstOrDefault();

                    currentHouseStatus.Status = int.Parse(houseStatusString);
                    currentHouseStatus.Details = houseStatusDetails;
                    currentHouseStatus.UserId = (long)currentUserId;

                    _context.Update(currentHouseStatus);
                    _context.SaveChanges();
                }
                else
                {
                    var houseStatus = new HouseStatus()
                    {
                        Status = int.Parse(houseStatusString),
                        Details = houseStatusDetails,
                        UserId = (long)currentUserId,
                        HouseId = currentHouseId
                    };

                    _context.Add(houseStatus);
                    _context.SaveChanges();
                }
            }

            return Json("");
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
                lastInventoryObject = inventoryObjects.OrderByDescending(x => x.ModifiedDate).First();
            }

            var hygieneObjects = _context.Set<Hygiene>().Where(x => x.HouseId == currentHouseId);

            if (hygieneObjects.Count() > 0)
            {
                lastHygieneObject = hygieneObjects.OrderByDescending(x => x.ModifiedDate).First();
            }

            var issueObjects = _context.Set<Issue>().Where(x => x.HouseId == currentHouseId);

            if (issueObjects.Count() > 0)
            {
                lastIssueObject = issueObjects.OrderByDescending(x => x.ModifiedDate).First();
            }

            displayObjects.Add(lastInventoryObject);
            displayObjects.Add(lastHygieneObject);
            displayObjects.Add(lastIssueObject);

            return displayObjects;
        }

        private List<string> GetMessages(long currentHouseId)
        {
            var messages = htmlRenderHelper.BuildMessagesForChat(_context.Messages.ToList(), currentHouseId);

            return messages;
        }
    }
}
