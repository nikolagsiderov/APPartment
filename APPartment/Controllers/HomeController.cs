using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities;
using APPartment.Data.Server.Models.Core;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Home;
using APPartment.Data.Server.Models.Base;
using APPartment.UI.ViewModels;
using APPartment.Data.Server.Models.Objects;
using APPartment.Data.Core;

namespace APPartment.Controllers
{
    public class HomeController : BaseController
    {
        #region Context, Services and Utilities
        private HtmlRenderHelper htmlRenderHelper;
        private TimeConverter timeConverter = new TimeConverter();
        private readonly BaseFacade baseFacade;
        #endregion

        public HomeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            baseFacade = new BaseFacade();
            htmlRenderHelper = new HtmlRenderHelper();
        }

        #region Actions
        [DefaultBreadcrumb(HomeBreadcrumbs.Default_Breadcrumb)]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = baseFacade.GetObject<User>((long)CurrentUserId);

            ViewData["Username"] = currentUser.Name;

            var displayObjects = GetDisplayObject();

            var homePageDisplayModel = new HomePageDisplayModel()
            {
                Messages = GetMessages(),
                BaseObjects = displayObjects
            };

            if (baseFacade.GetObjects<HomeStatus>(x => x.HomeId == (long)CurrentHomeId).Any())
            {
                homePageDisplayModel.HomeStatus = baseFacade.GetObject<HomeStatus>(x => x.HomeId == (long)CurrentHomeId);
            }

            if (baseFacade.GetObjects<HomeSetting>(x => x.HomeId == (long)CurrentHomeId).Any())
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
                var homeNameAlreadyExists = baseFacade.GetObject<Home>(x => x.Name == home.Name);

                if (homeNameAlreadyExists != null)
                {
                    ModelState.AddModelError("Name", "This home name is already taken.");
                    return View(home);
                }

                baseFacade.Create(home);
                home = baseFacade.GetObject<Home>(x => x.Name == home.Name);

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
            var existingHome = baseFacade.GetObject<Home>(x => x.Name == home.Name && x.Password == home.Password);

            if (existingHome != null)
            {
                HttpContext.Session.SetString("HomeId", existingHome.Id.ToString());
                HttpContext.Session.SetString("HomeName", existingHome.Name.ToString());

                SetUserToCurrentHome(existingHome.Id);

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

            var existingHomeSettings = baseFacade.GetObject<HomeSetting>(x => x.HomeId == (long)CurrentHomeId);

            if (existingHomeSettings != null)
            {
                var homeModel = baseFacade.GetObject<Home>((long)CurrentHomeId);
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
            var homeModel = baseFacade.GetObject<Home>((long)CurrentHomeId);
            settings.HomeId = (long)CurrentHomeId;

            if (!string.IsNullOrEmpty(settings.HomeName) || settings.HomeName != homeModel.Name)
            {
                homeModel.Name = settings.HomeName;

                baseFacade.Update(homeModel);
                HttpContext.Session.SetString("HomeName", homeModel.Name.ToString());
            }

            if (settings.Id == 0)
            {
                settings.HomeId = (long)CurrentHomeId;
                baseFacade.Create(settings);
            }
            else
            {
                baseFacade.Update(settings);
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

            baseFacade.Create(message);

            return Ok();
        }

        public JsonResult GetHomeStatus()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                if (baseFacade.GetObjects<HomeStatus>(x => x.HomeId == (long)CurrentHomeId).Any())
                {
                    var currentHomeStatus = baseFacade.GetObject<HomeStatus>(x => x.HomeId == (long)CurrentHomeId);
                    var user = baseFacade.GetObject<User>(currentHomeStatus.UserId);

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
                var homeModel = baseFacade.GetObject<Home>((long)CurrentHomeId);

                var homeStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(homeStatusDetailsString))
                {
                    homeStatusDetails = homeStatusDetailsString;
                }

                if (baseFacade.GetObjects<HomeStatus>(x => x.HomeId == (long)CurrentHomeId).Any())
                {
                    var currentHomeStatus = baseFacade.GetObject<HomeStatus>(x => x.HomeId == (long)CurrentHomeId);

                    currentHomeStatus.Status = int.Parse(homeStatusString);
                    currentHomeStatus.Details = homeStatusDetails;
                    currentHomeStatus.UserId = (long)CurrentUserId;

                    baseFacade.Update(currentHomeStatus);
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

                    baseFacade.Create(homeStatus);
                }
            }

            return Json("");
        }
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

            var inventoryObject = baseFacade.GetObject<Inventory>(x => x.HomeId == (long)CurrentHomeId);

            if (inventoryObject != null)
            {
                inventoryObject.LastUpdated = inventoryObject.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)inventoryObject.ModifiedDate);
                //var searchedUser = new User() { Id = (long)inventoryObject.ModifiedById };
                //inventoryObject.LastUpdatedBy = dao.GetObject(searchedUser, x => x.Id == (long)inventoryObject.ModifiedById).Name;
                //lastInventoryModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastInventoryModel.ObjectId);
            }

            var hygieneObjects = baseFacade.GetObject<Hygiene>(x => x.HomeId == (long)CurrentHomeId);

            if (hygieneObjects != null)
            {
                lastHygieneModel.LastUpdated = hygieneObjects.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)hygieneObjects.ModifiedDate);
                //var searchedUser = new User() { Id = (long)hygieneObjects.ModifiedById };
                //lastHygieneModel.LastUpdatedBy = dao.GetObject(searchedUser, x => x.Id == (long)hygieneObjects.ModifiedById).Name;
                //lastHygieneModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastHygieneModel.ObjectId);
            }

            var issueObjects = baseFacade.GetObject<Issue>(x => x.HomeId == (long)CurrentHomeId);

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
            var rentDueDateDay = baseFacade.GetObject<HomeSetting>(x => x.HomeId == (long)CurrentHomeId).RentDueDateDay;

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
            var messages = htmlRenderHelper.BuildMessagesForChat(baseFacade.GetObjects<Message>(), (long)CurrentHomeId);

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

            var userIsAlreadyApartOfCurrentHome = baseFacade.GetObject<HomeUser>(x => x.UserId == homeUser.UserId && x.HomeId == homeUser.HomeId);

            if (userIsAlreadyApartOfCurrentHome == null)
            {
                baseFacade.Create(homeUser);
            }
        }
    }
}
