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
using System.Threading.Tasks;
using System.Net.Http;
using APPartment.ORM.Framework;
using Newtonsoft.Json;

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
            htmlRenderHelper = new HtmlRenderHelper(CurrentUserID);
            timeConverter = new TimeConverter();
        }

        [DefaultBreadcrumb(HomeBreadcrumbs.Default_Breadcrumb)]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
                return RedirectToAction("Login", "Account");

            var currentUser = BaseWebService.GetEntity<UserPostViewModel>((long)CurrentUserID);

            ViewData["Username"] = currentUser.Name;

            var displayObjects = GetDisplayObject();

            var homePageDisplayModel = new HomePageDisplayModel()
            {
                Messages = GetMessages(),
                ViewModels = displayObjects
            };

            if (BaseWebService.Any<HomeStatusPostViewModel>(x => x.HomeID == (long)CurrentHomeID))
                homePageDisplayModel.HomeStatus = BaseWebService.GetEntity<HomeStatusPostViewModel>(x => x.HomeID == (long)CurrentHomeID);

            if (BaseWebService.Any<HomeSettingPostViewModel>(x => x.HomeID == (long)CurrentHomeID))
                homePageDisplayModel.RentDueDate = GetRentDueDate();

            return View(homePageDisplayModel);
        }

        public IActionResult EnterCreateHomeOptions()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(HomePostViewModel home)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/{nameof(Register)}";

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, home))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            home = JsonConvert.DeserializeObject<HomePostViewModel>(content);
                        else
                        {
                            ModelState.AddModelError("Name", "This home name is already taken.");
                            return View(home);
                        }
                    }
                }

                ModelState.Clear();

                HttpContext.Session.SetString("HomeID", home.ID.ToString());
                HttpContext.Session.SetString("HomeName", home.Name.ToString());

                SetUserToCurrentHome(home.ID);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(HomePostViewModel home)
        {
            home.ConfirmPassword = home.Password;
            ModelState.Clear();

            if (string.IsNullOrEmpty(home.Name))
                ModelState.AddModelError("Name", "Home name field is required.");
            else if (string.IsNullOrEmpty(home.Password))
                ModelState.AddModelError("Password", "Password field is required.");
            else
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/{CurrentControllerName}/{nameof(Login)}";

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, home))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            home = JsonConvert.DeserializeObject<HomePostViewModel>(content);

                            if (home != null)
                            {
                                HttpContext.Session.SetString("HomeID", home.ID.ToString());
                                HttpContext.Session.SetString("HomeName", home.Name.ToString());

                                SetUserToCurrentHome(home.ID);

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Home name or password is wrong.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Home name or password is wrong.");
                        }
                    }
                }
            }

            return View();
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.Settings_Breadcrumb)]
        public IActionResult Settings()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
                return RedirectToAction("Login", "Home");

            var existingHomeSettings = BaseWebService.GetEntity<HomeSettingPostViewModel>(x => x.HomeID == (long)CurrentHomeID);

            if (existingHomeSettings != null)
            {
                var homeModel = BaseWebService.GetEntity<HomePostViewModel>((long)CurrentHomeID);
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
            var home = BaseWebService.GetEntity<HomePostViewModel>((long)CurrentHomeID);
            settings.HomeID = (long)CurrentHomeID;

            if (!string.IsNullOrEmpty(settings.HomeName) || settings.HomeName != home.Name)
            {
                home.Name = settings.HomeName;
                BaseWebService.Save(home);

                HttpContext.Session.SetString("HomeName", home.Name.ToString());
            }

            if (settings.ID == 0)
            {
                settings.HomeID = (long)CurrentHomeID;
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
            return View();
        }

        [HttpPost]
        public ActionResult CreateMessage(string username, string messageText)
        {
            var adjustedMessage = string.Join(" <br /> ", messageText.Split('\n').ToList());
            var message = new MessageDisplayViewModel() { Details = adjustedMessage, CreatedByID = (long)CurrentUserID, HomeID = (long)CurrentHomeID, CreatedDate = DateTime.Now };

            BaseWebService.Save(message);

            return Ok();
        }

        public JsonResult GetHomeStatus()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
            {
                if (BaseWebService.Any<HomeStatusPostViewModel>(x => x.HomeID == (long)CurrentHomeID))
                {
                    var currentHomeStatus = BaseWebService.GetEntity<HomeStatusPostViewModel>(x => x.HomeID == (long)CurrentHomeID);
                    var user = BaseWebService.GetEntity<UserPostViewModel>(currentHomeStatus.UserID);

                    var result = $"{currentHomeStatus.Status};{user.Name};{currentHomeStatus.Details}";

                    return Json(result);
                }
            }

            var elseResult = $"1;system_generated;No one has set a status yet!";

            return Json(elseResult);
        }

        public ActionResult SetHomeStatus(string homeStatusString, string homeStatusDetailsString)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HomeID")))
            {
                var homeStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(homeStatusDetailsString))
                    homeStatusDetails = homeStatusDetailsString;

                if (BaseWebService.Any<HomeStatusPostViewModel>(x => x.HomeID == (long)CurrentHomeID))
                {
                    var currentHomeStatus = BaseWebService.GetEntity<HomeStatusPostViewModel>(x => x.HomeID == (long)CurrentHomeID);

                    currentHomeStatus.Status = int.Parse(homeStatusString);
                    currentHomeStatus.Details = homeStatusDetails;
                    currentHomeStatus.UserID = (long)CurrentUserID;

                    BaseWebService.Save(currentHomeStatus);
                }
                else
                {
                    var homeStatus = new HomeStatusPostViewModel()
                    {
                        Status = int.Parse(homeStatusString),
                        Details = homeStatusDetails,
                        UserID = (long)CurrentUserID,
                        HomeID = (long)CurrentHomeID
                    };

                    BaseWebService.Save(homeStatus);
                }
            }

            return Json("");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestID = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Getters
        public List<PostViewModel> GetDisplayObject()
        {
            var displayObjects = new List<PostViewModel>();
            var lastInventoryModel = new InventoryPostViewModel();
            var lastHygieneModel = new HygienePostViewModel();
            var lastIssueModel = new IssuePostViewModel();

            var inventoryObject = BaseWebService.GetEntity<InventoryPostViewModel>(x => x.HomeID == (long)CurrentHomeID);

            if (inventoryObject != null)
            {
                //inventoryObject.LastUpdated = inventoryObject.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)inventoryObject.ModifiedDate);
                //var searchedUser = new User() { ID = (long)inventoryObject.ModifiedByID };
                //inventoryObject.LastUpdatedBy = dao.GetObject(searchedUser, x => x.ID == (long)inventoryObject.ModifiedByID).Name;
                //lastInventoryModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastInventoryModel.ObjectID);
            }

            var hygieneObjects = BaseWebService.GetEntity<HygienePostViewModel>(x => x.HomeID == (long)CurrentHomeID);

            if (hygieneObjects != null)
            {
                //lastHygieneModel.LastUpdated = hygieneObjects.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)hygieneObjects.ModifiedDate);
                //var searchedUser = new User() { ID = (long)hygieneObjects.ModifiedByID };
                //lastHygieneModel.LastUpdatedBy = dao.GetObject(searchedUser, x => x.ID == (long)hygieneObjects.ModifiedByID).Name;
                //lastHygieneModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastHygieneModel.ObjectID);
            }

            var issueObjects = BaseWebService.GetEntity<IssuePostViewModel>(x => x.HomeID == (long)CurrentHomeID);

            if (issueObjects != null)
            {
                //lastIssueModel.LastUpdated = issueObjects.ModifiedDate == null ? string.Empty : timeConverter.CalculateRelativeTime((DateTime)issueObjects.ModifiedDate);
                //var searchedUser = new User() { ID = (long)issueObjects.ModifiedByID };
                //lastIssueModel.LastUpdatedBy = dao.GetObject(searchedUser, x => x.ID == (long)issueObjects.ModifiedByID).Name;
                //lastIssueModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastIssueModel.ObjectID); 
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
            var rentDueDateDay = BaseWebService.GetEntity<HomeSettingPostViewModel>(x => x.HomeID == (long)CurrentHomeID).RentDueDateDay;

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
            // TODO: x.CreatedByID != 0 should be handled as case when user is deleted
            var messages = BaseWebService.GetCollection<MessageDisplayViewModel>(x => x.HomeID == (long)CurrentHomeID && x.CreatedByID != 0);
            var messagesResult = htmlRenderHelper.BuildMessagesForChat(messages);

            return messagesResult;
        }
        #endregion

        public void SetUserToCurrentHome(long homeID)
        {
            var homeUser = new HomeUserPostViewModel()
            {
                HomeID = homeID,
                UserID = (long)CurrentUserID
            };

            var userIsAlreadyApartOfCurrentHome = BaseWebService.Any<HomeUserPostViewModel>(x => x.UserID == homeUser.UserID && x.HomeID == homeUser.HomeID);

            if (!userIsAlreadyApartOfCurrentHome)
                BaseWebService.Save(homeUser);
        }
    }
}
