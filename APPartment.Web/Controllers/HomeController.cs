using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Http;
using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels.Home;
using APPartment.UI.ViewModels;
using APPartment.UI.ViewModels.Inventory;
using APPartment.UI.ViewModels.Hygiene;
using APPartment.UI.ViewModels.Issue;
using APPartment.UI.ViewModels.Base;
using APPartment.UI.ViewModels.User;
using APPartment.UI.ViewModels.Chat;

namespace APPartment.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Services and Utilities
        private HtmlRenderHelper htmlRenderHelper;
        private TimeConverter timeConverter;
        #endregion

        public HomeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            htmlRenderHelper = new HtmlRenderHelper(CurrentUserId);
            timeConverter = new TimeConverter();
        }

        [DefaultBreadcrumb(HomeBreadcrumbs.Default_Breadcrumb)]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
                return RedirectToAction("Login", "Account");

            var currentUser = BaseWebService.GetEntity<UserPostViewModel>((long)CurrentUserId);

            ViewData["Username"] = currentUser.Name;

            var displayObjects = GetDisplayObject();

            var homePageDisplayModel = new HomePageDisplayModel()
            {
                Messages = GetMessages(),
                ViewModels = displayObjects
            };

            if (BaseWebService.Any<HomeStatusPostViewModel>(x => x.HomeId == (long)CurrentHomeId))
                homePageDisplayModel.HomeStatus = BaseWebService.GetEntity<HomeStatusPostViewModel>(x => x.HomeId == (long)CurrentHomeId);

            if (BaseWebService.Any<HomeSettingPostViewModel>(x => x.HomeId == (long)CurrentHomeId))
                homePageDisplayModel.RentDueDate = GetRentDueDate();

            return View(homePageDisplayModel);
        }

        public IActionResult EnterCreateHomeOptions()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Register(HomePostViewModel home)
        {
            if (ModelState.IsValid)
            {
                var homeExists = BaseWebService.Any<HomePostViewModel>(x => x.Name == home.Name);

                if (homeExists)
                {
                    ModelState.AddModelError("Name", "This home name is already taken.");
                    return View(home);
                }

                BaseWebService.Save(home);
                home = BaseWebService.GetEntity<HomePostViewModel>(x => x.Name == home.Name);

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
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Login(HomePostViewModel home)
        {
            var existingHome = BaseWebService.GetEntity<HomePostViewModel>(x => x.Name == home.Name && x.Password == home.Password);

            if (existingHome != null)
            {
                HttpContext.Session.SetString("HomeId", existingHome.Id.ToString());
                HttpContext.Session.SetString("HomeName", existingHome.Name.ToString());

                SetUserToCurrentHome(existingHome.Id);

                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Home name or password is wrong.");

            return View();
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.Settings_Breadcrumb)]
        public IActionResult Settings()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
                return RedirectToAction("Login", "Home");

            var existingHomeSettings = BaseWebService.GetEntity<HomeSettingPostViewModel>(x => x.HomeId == (long)CurrentHomeId);

            if (existingHomeSettings != null)
            {
                var homeModel = BaseWebService.GetEntity<HomePostViewModel>((long)CurrentHomeId);
                existingHomeSettings.HomeName = homeModel.Name;
            }

            if (existingHomeSettings != null)
                return View(existingHomeSettings);

            ViewData["HomeName"] = CurrentHomeName;

            return View();
        }

        [HttpPost]
        public IActionResult Settings(HomeSettingPostViewModel settings)
        {
            var home = BaseWebService.GetEntity<HomePostViewModel>((long)CurrentHomeId);
            settings.HomeId = (long)CurrentHomeId;

            if (!string.IsNullOrEmpty(settings.HomeName) || settings.HomeName != home.Name)
            {
                home.Name = settings.HomeName;
                BaseWebService.Save(home);

                HttpContext.Session.SetString("HomeName", home.Name.ToString());
            }

            if (settings.Id == 0)
            {
                settings.HomeId = (long)CurrentHomeId;
                BaseWebService.Save(settings);
            }
            else
                BaseWebService.Save(settings);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.About_Breadcrumb)]
        public IActionResult About()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public ActionResult CreateMessage(string username, string messageText)
        {
            var adjustedMessage = string.Join(" <br /> ", messageText.Split('\n').ToList());
            var message = new MessageDisplayViewModel() { Details = adjustedMessage, CreatedById = (long)CurrentUserId, HomeId = (long)CurrentHomeId, CreatedDate = DateTime.Now };

            BaseWebService.Save(message);

            return Ok();
        }

        public JsonResult GetHomeStatus()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeId")))
            {
                if (BaseWebService.Any<HomeStatusPostViewModel>(x => x.HomeId == (long)CurrentHomeId))
                {
                    var currentHomeStatus = BaseWebService.GetEntity<HomeStatusPostViewModel>(x => x.HomeId == (long)CurrentHomeId);
                    var user = BaseWebService.GetEntity<UserPostViewModel>(currentHomeStatus.UserId);

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
                var homeStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(homeStatusDetailsString))
                    homeStatusDetails = homeStatusDetailsString;

                if (BaseWebService.Any<HomeStatusPostViewModel>(x => x.HomeId == (long)CurrentHomeId))
                {
                    var currentHomeStatus = BaseWebService.GetEntity<HomeStatusPostViewModel>(x => x.HomeId == (long)CurrentHomeId);

                    currentHomeStatus.Status = int.Parse(homeStatusString);
                    currentHomeStatus.Details = homeStatusDetails;
                    currentHomeStatus.UserId = (long)CurrentUserId;

                    BaseWebService.Save(currentHomeStatus);
                }
                else
                {
                    var homeStatus = new HomeStatusPostViewModel()
                    {
                        Status = int.Parse(homeStatusString),
                        Details = homeStatusDetails,
                        UserId = (long)CurrentUserId,
                        HomeId = (long)CurrentHomeId
                    };

                    BaseWebService.Save(homeStatus);
                }
            }

            return Json("");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Getters
        public List<PostViewModel> GetDisplayObject()
        {
            var displayObjects = new List<PostViewModel>();
            var lastInventoryModel = new InventoryPostViewModel();
            var lastHygieneModel = new HygienePostViewModel();
            var lastIssueModel = new IssuePostViewModel();

            var inventoryObject = BaseWebService.GetEntity<InventoryPostViewModel>(x => x.HomeId == (long)CurrentHomeId);

            if (inventoryObject != null)
            {
                inventoryObject.LastUpdated = inventoryObject.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)inventoryObject.ModifiedDate);
                //var searchedUser = new User() { Id = (long)inventoryObject.ModifiedById };
                //inventoryObject.LastUpdatedBy = dao.GetObject(searchedUser, x => x.Id == (long)inventoryObject.ModifiedById).Name;
                //lastInventoryModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastInventoryModel.ObjectId);
            }

            var hygieneObjects = BaseWebService.GetEntity<HygienePostViewModel>(x => x.HomeId == (long)CurrentHomeId);

            if (hygieneObjects != null)
            {
                lastHygieneModel.LastUpdated = hygieneObjects.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)hygieneObjects.ModifiedDate);
                //var searchedUser = new User() { Id = (long)hygieneObjects.ModifiedById };
                //lastHygieneModel.LastUpdatedBy = dao.GetObject(searchedUser, x => x.Id == (long)hygieneObjects.ModifiedById).Name;
                //lastHygieneModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastHygieneModel.ObjectId);
            }

            var issueObjects = BaseWebService.GetEntity<IssuePostViewModel>(x => x.HomeId == (long)CurrentHomeId);

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
            var rentDueDateDay = BaseWebService.GetEntity<HomeSettingPostViewModel>(x => x.HomeId == (long)CurrentHomeId).RentDueDateDay;

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
            // TODO: x.CreatedById != 0 should be handled as case when user is deleted
            var messages = BaseWebService.GetCollection<MessageDisplayViewModel>(x => x.HomeId == (long)CurrentHomeId && x.CreatedById != 0);
            var messagesResult = htmlRenderHelper.BuildMessagesForChat(messages, (long)CurrentHomeId);

            return messagesResult;
        }
        #endregion

        public void SetUserToCurrentHome(long homeId)
        {
            var homeUser = new HomeUserPostViewModel()
            {
                HomeId = homeId,
                UserId = (long)CurrentUserId
            };

            var userIsAlreadyApartOfCurrentHome = BaseWebService.Any<HomeUserPostViewModel>(x => x.UserId == homeUser.UserId && x.HomeId == homeUser.HomeId);

            if (!userIsAlreadyApartOfCurrentHome)
                BaseWebService.Save(homeUser);
        }
    }
}
