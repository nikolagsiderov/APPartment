﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using APPartment.UI.Controllers.Base;
using APPartment.Data.Core;
using APPartment.UI.Utilities;
using APPartment.ORM.Framework.Core;
using APPartment.Data.Server.Models.Core;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Home;
using APPartment.Data.Server.Models.Base;
using APPartment.UI.ViewModels;
using APPartment.Data.Server.Models.Objects;
using APPObject = APPartment.Data.Server.Models.Core.Object;

namespace APPartment.Controllers
{
    public class HomeController : BaseController
    {
        #region Context, Services and Utilities
        private readonly DataAccessContext _context;
        private HtmlRenderHelper htmlRenderHelper;
        private TimeConverter timeConverter = new TimeConverter();
        private DataContext dataContext;
        private HistoryHtmlBuilder historyHtmlBuilder;
        #endregion

        public HomeController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
        {
            _context = context;
            htmlRenderHelper = new HtmlRenderHelper(_context);
            dataContext = new DataContext(_context);
            historyHtmlBuilder = new HistoryHtmlBuilder(_context);
        }

        #region Actions
        [DefaultBreadcrumb(HomeBreadcrumbs.Default_Breadcrumb)]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = _context.Users.Find(CurrentUserId);

            ViewData["Username"] = currentUser.Username;

            var displayObjects = GetDisplayObject();

            var homePageDisplayModel = new HomePageDisplayModel()
            {
                Messages = GetMessages(),
                BaseObjects = displayObjects
            };

            if (_context.HomeStatuses.Where(x => x.HomeId == CurrentHomeId).Any())
            {
                homePageDisplayModel.HomeStatus = _context.HomeStatuses.Where(x => x.HomeId == CurrentHomeId).OrderByDescending(x => x.Id).FirstOrDefault();
            }

            if (_context.HomeSettings.Any(x => x.HomeId == CurrentHomeId))
            {
                homePageDisplayModel.RentDueDate = GetRentDueDate();
            }

            return View(homePageDisplayModel);
        }

        public IActionResult EnterCreateHomeOptions()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Home home)
        {
            if (ModelState.IsValid)
            {
                var homeNameAlreadyExists = _context.Homes.Any(x => x.Name == home.Name);
                if (homeNameAlreadyExists)
                {
                    ModelState.AddModelError("Name", "This home name is already taken.");
                    return View(home);
                }

                await dataContext.SaveAsync<Home>(home, CurrentUserId, 0, null);

                ModelState.Clear();

                HttpContext.Session.SetString("HomeId", home.Id.ToString());
                HttpContext.Session.SetString("HomeName", home.Name.ToString());

                SetUserToCurrentHome(home.Id);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(Home home)
        {
            var homeIsContainedInDb = _context.Homes.Any(x => x.Name == home.Name && x.Password == home.Password);

            if (homeIsContainedInDb)
            {
                var thisHome = _context.Homes.Single(h => h.Name == home.Name && h.Password == home.Password);

                HttpContext.Session.SetString("HomeId", thisHome.Id.ToString());
                HttpContext.Session.SetString("HomeName", thisHome.Name.ToString());

                SetUserToCurrentHome(home.Id);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Home name or password is wrong.");
            }

            return View();
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.Settings_Breadcrumb)]
        public IActionResult Settings()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Login", "Home");
            }

            var homeSettingsArePresent = _context.HomeSettings.Any(x => x.HomeId == CurrentHomeId);
            HomeSettings homeSettings = null;

            if (homeSettingsArePresent)
            {
                homeSettings = _context.HomeSettings.Where(x => x.HomeId == CurrentHomeId).First();

                var homeModel = _context.Homes.Find(CurrentHomeId);
                homeSettings.HomeName = homeModel.Name;
            }

            if (homeSettings != null)
            {
                return View(homeSettings);
            }

            ViewData["HomeName"] = CurrentHomeName;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Settings(HomeSettings settings)
        {
            var homeModel = _context.Find<Home>(CurrentHomeId);
            settings.HomeId = CurrentHomeId;

            if (!string.IsNullOrEmpty(settings.HomeName) || settings.HomeName != homeModel.Name)
            {
                homeModel.Name = settings.HomeName;

                await dataContext.UpdateAsync(homeModel, CurrentUserId, CurrentHomeId, null);
                HttpContext.Session.SetString("HomeName", homeModel.Name.ToString());
            }

            if (settings.Id == 0)
            {
                settings.HomeId = CurrentHomeId;
                await dataContext.SaveAsync(settings, CurrentUserId, CurrentHomeId, null);
            }
            else
            {
                await dataContext.UpdateAsync(settings, CurrentUserId, CurrentHomeId, null);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.About_Breadcrumb)]
        public IActionResult About()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(string username, string messageText)
        {
            var adjustedMessage = string.Join(" <br /> ", messageText.Split('\n').ToList());
            var message = new Message() { Username = username, Text = adjustedMessage, UserId = (long)CurrentUserId, HomeId = (long)CurrentHomeId, CreatedDate = DateTime.Now };

            await dataContext.SaveAsync(message, CurrentUserId, CurrentHomeId, null);

            return Ok();
        }

        public JsonResult GetHomeStatus()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                if (_context.HomeStatuses.Any(x => x.HomeId == CurrentHomeId))
                {
                    var currentHomeStatusUserId = _context.HomeStatuses.OrderByDescending(x => x.Id).Where(x => x.HomeId == CurrentHomeId).FirstOrDefault().UserId;
                    var currentHomeStatus = _context.HomeStatuses.OrderByDescending(x => x.Id).Where(x => x.HomeId == CurrentHomeId).FirstOrDefault().Status;
                    var username = _context.Users.Where(x => x.UserId == currentHomeStatusUserId).FirstOrDefault().Username;
                    var currentHomeStatusDetails = _context.HomeStatuses.OrderByDescending(x => x.Id).Where(x => x.HomeId == CurrentHomeId).FirstOrDefault().Details;

                    var result = $"{currentHomeStatus};{username};{currentHomeStatusDetails}";

                    return Json(result);
                }
            }

            var elseResult = $"1;system_generated;No one has set a status yet!";

            return Json(elseResult);
        }

        public async Task<ActionResult> SetHomeStatus(string homeStatusString, string homeStatusDetailsString)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                var homeModel = _context.Find<Home>(CurrentHomeId);

                var homeStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(homeStatusDetailsString))
                {
                    homeStatusDetails = homeStatusDetailsString;
                }

                if (_context.HomeStatuses.Any(x => x.HomeId == CurrentHomeId))
                {
                    var currentHomeStatus = _context.HomeStatuses.OrderByDescending(x => x.Id).Where(x => x.HomeId == CurrentHomeId).FirstOrDefault();

                    currentHomeStatus.Status = int.Parse(homeStatusString);
                    currentHomeStatus.Details = homeStatusDetails;
                    currentHomeStatus.UserId = (long)CurrentUserId;

                    await dataContext.UpdateAsync(currentHomeStatus, (long)CurrentUserId, CurrentHomeId, null);
                }
                else
                {
                    var homeStatus = new HomeStatus()
                    {
                        Status = int.Parse(homeStatusString),
                        Details = homeStatusDetails,
                        UserId = (long)CurrentUserId,
                        HomeId = CurrentHomeId
                    };

                    await dataContext.SaveAsync(homeStatus, CurrentUserId, CurrentHomeId, null);
                }
            }

            return Json("");
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.History_Breadcrumb)]
        public IActionResult History()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Login", "Home");
            }

            var historyModel = new HomeHistoryDisplayView();
            var history = _context.Audits.Where(x => x.HomeId == CurrentHomeId).ToList();

            historyModel.History = historyHtmlBuilder.BuildHomeHistory(history);

            ViewData["HomeName"] = CurrentHomeName;

            return View(historyModel);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Getters
        public List<BaseObject> GetDisplayObject()
        {
            var displayObjects = new List<BaseObject>();
            var lastInventoryModel = new Inventory();
            var lastHygieneModel = new Hygiene();
            var lastIssueModel = new Issue();

            var inventoryObjects = _context.Set<Inventory>().Where(x => x.HomeId == CurrentHomeId);

            if (inventoryObjects.Count() > 0)
            {
                var inventoryObject = new APPObject();
                var objects = new List<APPObject>();

                foreach (var inventory in inventoryObjects)
                {
                    objects.Add(_context.Set<APPObject>().Where(x => x.ObjectId == inventory.ObjectId).FirstOrDefault());
                }

                inventoryObject = objects.OrderByDescending(x => x.ModifiedDate).FirstOrDefault();
                lastInventoryModel = _context.Set<Inventory>().Where(x => x.ObjectId == inventoryObject.ObjectId).FirstOrDefault();

                var when = _context.Audits.Where(x => x.ObjectId == lastInventoryModel.ObjectId || x.TargetObjectId == lastInventoryModel.ObjectId)
                    .OrderByDescending(x => x.Id).FirstOrDefault().When;

                lastInventoryModel.LastUpdated = when == null ? string.Empty : timeConverter.CalculateRelativeTime(when);
                lastInventoryModel.LastUpdatedBy = _context.Users.Where(x => x.UserId == inventoryObject.ModifiedById).FirstOrDefault().Username;
                lastInventoryModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastInventoryModel.ObjectId);
            }

            var hygieneObjects = _context.Set<Hygiene>().Where(x => x.HomeId == CurrentHomeId);

            if (hygieneObjects.Count() > 0)
            {
                var hygieneObject = new APPObject();
                var objects = new List<APPObject>();

                foreach (var hygiene in hygieneObjects)
                {
                    objects.Add(_context.Set<APPObject>().Where(x => x.ObjectId == hygiene.ObjectId).FirstOrDefault());
                }

                hygieneObject = objects.OrderByDescending(x => x.ModifiedDate).FirstOrDefault();
                lastHygieneModel = _context.Set<Hygiene>().Where(x => x.ObjectId == hygieneObject.ObjectId).FirstOrDefault();

                var when = _context.Audits.Where(x => x.ObjectId == lastHygieneModel.ObjectId || x.TargetObjectId == lastHygieneModel.ObjectId)
                    .OrderByDescending(x => x.Id).FirstOrDefault().When;

                lastHygieneModel.LastUpdated = when == null ? string.Empty : timeConverter.CalculateRelativeTime(when);
                lastHygieneModel.LastUpdatedBy = _context.Users.Where(x => x.UserId == hygieneObject.ModifiedById).FirstOrDefault().Username;
                lastHygieneModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastHygieneModel.ObjectId);
            }

            var issueObjects = _context.Set<Issue>().Where(x => x.HomeId == CurrentHomeId);

            if (issueObjects.Count() > 0)
            {
                var issueObject = new APPObject();
                var objects = new List<APPObject>();

                foreach (var issue in issueObjects)
                {
                    objects.Add(_context.Set<APPObject>().Where(x => x.ObjectId == issue.ObjectId).FirstOrDefault());
                }

                issueObject = objects.OrderByDescending(x => x.ModifiedDate).FirstOrDefault();
                lastIssueModel = _context.Set<Issue>().Where(x => x.ObjectId == issueObject.ObjectId).FirstOrDefault();

                var when = _context.Audits.Where(x => x.ObjectId == lastIssueModel.ObjectId || x.TargetObjectId == lastIssueModel.ObjectId)
                    .OrderByDescending(x => x.Id).FirstOrDefault().When;

                lastIssueModel.LastUpdated = when == null ? string.Empty : timeConverter.CalculateRelativeTime(when);
                lastIssueModel.LastUpdatedBy = _context.Users.Where(x => x.UserId == issueObject.ModifiedById).FirstOrDefault().Username;
                lastIssueModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastIssueModel.ObjectId); 
            }

            displayObjects.Add(lastInventoryModel);
            displayObjects.Add(lastHygieneModel);
            displayObjects.Add(lastIssueModel);

            return displayObjects;
        }

        public string GetRentDueDate()
        {
            var nextMonth = DateTime.Now.AddMonths(1).Month.ToString();
            var thisMonth = DateTime.Now.Month.ToString();
            var rentDueDate = string.Empty;
            var rentDueDateDay = _context.HomeSettings.Find(CurrentHomeId).RentDueDateDay;

            if (rentDueDateDay != null && rentDueDateDay.ToString() != "0")
            {
                var dateString = $"{rentDueDateDay}/{nextMonth}/{DateTime.Now.Year.ToString()}";

                if (DateTime.Parse(dateString).AddMonths(-1).Date > DateTime.Now.Date)
                {
                    dateString = $"{rentDueDateDay}/{thisMonth}/{DateTime.Now.Year.ToString()}";
                }

                rentDueDate = DateTime.Parse(dateString).ToLongDateString();
            }

            return rentDueDate;
        }

        private List<string> GetMessages()
        {
            var messages = htmlRenderHelper.BuildMessagesForChat(_context.Messages.ToList(), (long)CurrentHomeId);

            return messages;
        }
        #endregion

        public void SetUserToCurrentHome(long homeId)
        {
            var userIsAlreadyApartOfCurrentHome = _context.HomeUsers.Any(x => x.UserId == CurrentUserId && x.HomeId == homeId);

            if (!userIsAlreadyApartOfCurrentHome)
            {
                var homeUser = new HomeUser()
                {
                    HomeId = (long)homeId,
                    UserId = (long)CurrentUserId
                };

                _context.HomeUsers.Add(homeUser);
                _context.SaveChanges();
            }
        }
    }
}
