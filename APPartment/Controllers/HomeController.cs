﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities;
using APPartment.ORM.Framework.Core;
using APPartment.Data.Server.Models.Core;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Home;
using APPartment.Data.Server.Models.Base;
using APPartment.UI.ViewModels;
using APPartment.Data.Server.Models.Objects;

namespace APPartment.Controllers
{
    public class HomeController : BaseController
    {
        #region Context, Services and Utilities
        private HtmlRenderHelper htmlRenderHelper;
        private TimeConverter timeConverter = new TimeConverter();
        private DaoContext dao;
        //private HistoryHtmlBuilder historyHtmlBuilder;
        #endregion

        public HomeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            dao = new DaoContext();
            htmlRenderHelper = new HtmlRenderHelper();
            //historyHtmlBuilder = new HistoryHtmlBuilder();
        }

        #region Actions
        [DefaultBreadcrumb(HomeBreadcrumbs.Default_Breadcrumb)]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var searchedUser = new User() { Id = (long)CurrentUserId };
            var currentUser = dao.GetObject(searchedUser, x => x.Id == searchedUser.Id);

            ViewData["Username"] = currentUser.Name;

            var displayObjects = GetDisplayObject();

            var homePageDisplayModel = new HomePageDisplayModel()
            {
                Messages = GetMessages(),
                BaseObjects = displayObjects
            };

            var searchedHomeStatus = new HomeStatus() { HomeId = (long)CurrentHomeId };
            var searchedHomeSettings = new HomeSetting() { HomeId = (long)CurrentHomeId };

            if (dao.GetObjects(searchedHomeStatus, x => x.HomeId == searchedHomeStatus.HomeId).Any())
            {
                homePageDisplayModel.HomeStatus = dao.GetObject(searchedHomeStatus, x => x.HomeId == searchedHomeStatus.HomeId);
            }

            if (dao.GetObjects(searchedHomeSettings, x => x.HomeId == searchedHomeSettings.HomeId).Any())
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
        public IActionResult Register(Home home)
        {
            if (ModelState.IsValid)
            {
                var homeNameAlreadyExists = dao.GetObject(home, x => x.Name == home.Name);

                if (homeNameAlreadyExists != null)
                {
                    ModelState.AddModelError("Name", "This home name is already taken.");
                    return View(home);
                }

                dao.Create(home);
                home = dao.GetObject(home, x => x.Name == home.Name);

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
            var existingHome = dao.GetObject(home, x => x.Name == home.Name && x.Password == home.Password);

            if (existingHome != null)
            {
                HttpContext.Session.SetString("HomeId", existingHome.Id.ToString());
                HttpContext.Session.SetString("HomeName", existingHome.Name.ToString());

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

            var searchedHomeSettings = new HomeSetting() { HomeId = (long)CurrentHomeId };
            var existingHomeSettings = dao.GetObject(searchedHomeSettings, x => x.HomeId == searchedHomeSettings.HomeId);

            if (existingHomeSettings != null)
            {
                var searchedHome = new Home() { Id = (long)CurrentHomeId };
                var homeModel = dao.GetObject(searchedHome, (long)CurrentHomeId);
                existingHomeSettings.HomeName = homeModel.Name;
            }

            if (existingHomeSettings != null)
            {
                return View(existingHomeSettings);
            }

            ViewData["HomeName"] = CurrentHomeName;

            return View();
        }

        [HttpPost]
        public IActionResult Settings(HomeSetting settings)
        {
            var searchedHome = new Home() { Id = (long)CurrentHomeId };
            var homeModel = dao.GetObject(searchedHome, (long)CurrentHomeId);
            settings.HomeId = (long)CurrentHomeId;

            if (!string.IsNullOrEmpty(settings.HomeName) || settings.HomeName != homeModel.Name)
            {
                homeModel.Name = settings.HomeName;

                dao.Update(homeModel);
                HttpContext.Session.SetString("HomeName", homeModel.Name.ToString());
            }

            if (settings.Id == 0)
            {
                settings.HomeId = (long)CurrentHomeId;
                dao.Create(settings);
            }
            else
            {
                dao.Update(settings);
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
        public ActionResult CreateMessage(string username, string messageText)
        {
            var adjustedMessage = string.Join(" <br /> ", messageText.Split('\n').ToList());
            var message = new Message() { Details = adjustedMessage, CreatedById = (long)CurrentUserId, HomeId = (long)CurrentHomeId, CreatedDate = DateTime.Now };

            dao.Create(message);

            return Ok();
        }

        public JsonResult GetHomeStatus()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                var searchedHomeStatus = new HomeStatus() { HomeId = (long)CurrentHomeId };
                if (dao.GetObjects(searchedHomeStatus, x => x.HomeId == searchedHomeStatus.HomeId).Any())
                {
                    var currentHomeStatus = dao.GetObject(searchedHomeStatus, x => x.HomeId == searchedHomeStatus.HomeId);
                    var searchedUser = new User() { Id = currentHomeStatus.UserId };
                    var user = dao.GetObject(searchedUser, x => x.Id == searchedUser.Id);

                    var result = $"{currentHomeStatus.Status};{user.Name};{currentHomeStatus.Details}";

                    return Json(result);
                }
            }

            var elseResult = $"1;system_generated;No one has set a status yet!";

            return Json(elseResult);
        }

        public ActionResult SetHomeStatus(string homeStatusString, string homeStatusDetailsString)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                var searchedHome = new Home() { Id = (long)CurrentHomeId };
                var homeModel = dao.GetObject(searchedHome, (long)CurrentHomeId);

                var homeStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(homeStatusDetailsString))
                {
                    homeStatusDetails = homeStatusDetailsString;
                }

                var searchedHomeStatus = new HomeStatus() { HomeId = (long)CurrentHomeId };

                if (dao.GetObjects(searchedHomeStatus, x => x.HomeId == searchedHomeStatus.HomeId).Any())
                {
                    var currentHomeStatus = dao.GetObject(searchedHomeStatus, x => x.HomeId == searchedHomeStatus.HomeId);

                    currentHomeStatus.Status = int.Parse(homeStatusString);
                    currentHomeStatus.Details = homeStatusDetails;
                    currentHomeStatus.UserId = (long)CurrentUserId;

                    dao.Update(currentHomeStatus);
                }
                else
                {
                    var homeStatus = new HomeStatus()
                    {
                        Status = int.Parse(homeStatusString),
                        Details = homeStatusDetails,
                        UserId = (long)CurrentUserId,
                        HomeId = (long)CurrentHomeId
                    };

                    dao.Create(homeStatus);
                }
            }

            return Json("");
        }

        //[HttpGet]
        //[Breadcrumb(HomeBreadcrumbs.History_Breadcrumb)]
        //public IActionResult History()
        //{
        //    if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    var historyModel = new HomeHistoryDisplayView();
        //    var history = _context.Audits.Where(x => x.HomeId == CurrentHomeId).ToList();

        //    historyModel.History = historyHtmlBuilder.BuildHomeHistory(history);

        //    ViewData["HomeName"] = CurrentHomeName;

        //    return View(historyModel);
        //}
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Getters
        public List<IdentityBaseObject> GetDisplayObject()
        {
            var displayObjects = new List<IdentityBaseObject>();
            var lastInventoryModel = new Inventory();
            var lastHygieneModel = new Hygiene();
            var lastIssueModel = new Issue();

            var searchedInventory = new Inventory() { HomeId = (long)CurrentHomeId };
            var inventoryObject = dao.GetObject(searchedInventory, x => x.HomeId == searchedInventory.HomeId);

            if (inventoryObject != null)
            {
                inventoryObject.LastUpdated = inventoryObject.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)inventoryObject.ModifiedDate);
                //var searchedUser = new User() { Id = (long)inventoryObject.ModifiedById };
                //inventoryObject.LastUpdatedBy = dao.GetObject(searchedUser, x => x.Id == (long)inventoryObject.ModifiedById).Name;
                //lastInventoryModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastInventoryModel.ObjectId);
            }

            var searchedHygiene = new Hygiene() { HomeId = (long)CurrentHomeId };
            var hygieneObjects = dao.GetObject(searchedHygiene, x => x.HomeId == searchedHygiene.HomeId);

            if (hygieneObjects != null)
            {
                lastHygieneModel.LastUpdated = hygieneObjects.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)hygieneObjects.ModifiedDate);
                //var searchedUser = new User() { Id = (long)hygieneObjects.ModifiedById };
                //lastHygieneModel.LastUpdatedBy = dao.GetObject(searchedUser, x => x.Id == (long)hygieneObjects.ModifiedById).Name;
                //lastHygieneModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastHygieneModel.ObjectId);
            }

            var searchedIssue = new Issue() { HomeId = (long)CurrentHomeId };
            var issueObjects = dao.GetObject(searchedIssue, x => x.HomeId == searchedIssue.HomeId);

            if (issueObjects != null)
            {
                lastIssueModel.LastUpdated = issueObjects.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)issueObjects.ModifiedDate);
                //var searchedUser = new User() { Id = (long)issueObjects.ModifiedById };
                //lastIssueModel.LastUpdatedBy = dao.GetObject(searchedUser, x => x.Id == (long)issueObjects.ModifiedById).Name;
                //lastIssueModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastIssueModel.ObjectId); 
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
            var searchedHomeSetting = new HomeSetting() { HomeId = (long)CurrentHomeId };
            var rentDueDateDay = dao.GetObject(searchedHomeSetting, x => x.HomeId == searchedHomeSetting.HomeId).RentDueDateDay;

            if (rentDueDateDay.ToString() != "0")
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
            var messages = htmlRenderHelper.BuildMessagesForChat(dao.GetObjects<Message>(), (long)CurrentHomeId);

            return messages;
        }
        #endregion

        public void SetUserToCurrentHome(long homeId)
        {
            var homeUser = new HomeUser()
            {
                HomeId = homeId,
                UserId = (long)CurrentUserId
            };

            var userIsAlreadyApartOfCurrentHome = dao.GetObject(homeUser, x => x.UserId == homeUser.UserId && x.HomeId == homeUser.HomeId);

            if (userIsAlreadyApartOfCurrentHome == null)
            {
                dao.Create(homeUser);
            }
        }
    }
}
