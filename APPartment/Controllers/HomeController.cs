using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using APPartment.Models;
using SmartBreadcrumbs.Attributes;
using APPartment.Data;
using Microsoft.AspNetCore.Http;
using APPartment.Models.Base;
using System.Threading.Tasks;
using APPartment.DisplayModels.Home;
using APPartment.Utilities;
using APPartment.Enums;
using APPartment.Core;
using APPartment.Utilities.Constants.Breadcrumbs;
using APPartment.Controllers.Base;

namespace APPartment.Controllers
{
    public class HomeController : BaseController
    {
        #region Context, Services and Utilities
        private readonly DataAccessContext _context;
        private HtmlRenderHelper htmlRenderHelper;
        private TimeConverter timeConverter = new TimeConverter();
        private DataContext<House> dataContext;
        private DataContext<HouseSettings> houseSettingsDataContext;
        private DataContext<Message> messageDataContext;
        private DataContext<HouseStatus> houseStatusDataContext;
        private HistoryHtmlBuilder historyHtmlBuilder;
        #endregion

        public HomeController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
        {
            _context = context;
            htmlRenderHelper = new HtmlRenderHelper(_context);
            dataContext = new DataContext<House>(_context);
            houseSettingsDataContext = new DataContext<HouseSettings>(_context);
            messageDataContext = new DataContext<Message>(_context);
            houseStatusDataContext = new DataContext<HouseStatus>(_context);
            historyHtmlBuilder = new HistoryHtmlBuilder(_context);
        }

        #region Actions
        [DefaultBreadcrumb(HomeBreadcrumbs.Default_Breadcrumb)]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = _context.Users.Find(CurrentUserId);

            ViewData["Username"] = currentUser.Username;

            var displayObjects = GetDisplayObject();

            var homeDisplayModel = new HomeDisplayModel()
            {
                Messages = GetMessages(),
                BaseObjects = displayObjects
            };

            if (_context.HouseStatuses.Where(x => x.HouseId == CurrentHouseId).Any())
            {
                homeDisplayModel.HouseStatus = _context.HouseStatuses.Where(x => x.HouseId == CurrentHouseId).OrderByDescending(x => x.Id).FirstOrDefault();
            }

            if (_context.HouseSettings.Any(x => x.HouseId == CurrentHouseId))
            {
                homeDisplayModel.RentDueDate = GetRentDueDate();
            }

            return View(homeDisplayModel);
        }

        public IActionResult EnterCreateHomeOptions()
        {
            return View();
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
                dataContext.Save(house, CurrentUserId, 0, null);

                ModelState.Clear();

                HttpContext.Session.SetString("HouseId", house.Id.ToString());
                HttpContext.Session.SetString("HouseName", house.Name.ToString());

                SetUserToCurrentHouse();

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

                SetUserToCurrentHouse();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "House name or password is wrong.");
            }

            return View();
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.Settings_Breadcrumb)]
        public IActionResult Settings()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Home");
            }

            var houseSettingsArePresent = _context.HouseSettings.Any(x => x.HouseId == CurrentHouseId);
            HouseSettings houseSettings = null;

            if (houseSettingsArePresent)
            {
                houseSettings = _context.HouseSettings.Where(x => x.HouseId == CurrentHouseId).First();

                var houseModel = _context.Houses.Find(CurrentHouseId);
                houseSettings.HouseName = houseModel.Name;
            }

            if (houseSettings != null)
            {
                return View(houseSettings);
            }

            ViewData["HouseName"] = CurrentHouseName;

            return View();
        }

        [HttpPost]
        public IActionResult Settings(HouseSettings settings)
        {
            var houseModel = _context.Find<House>(CurrentHouseId);
            settings.HouseId = CurrentHouseId;

            if (!string.IsNullOrEmpty(settings.HouseName) || settings.HouseName != houseModel.Name)
            {
                houseModel.Name = settings.HouseName;

                dataContext.Update(houseModel, CurrentUserId, CurrentHouseId, null);
                HttpContext.Session.SetString("HouseName", houseModel.Name.ToString());
            }

            if (settings.Id == 0)
            {
                settings.HouseId = CurrentHouseId;
                houseSettingsDataContext.Save(settings, CurrentUserId, CurrentHouseId, null);
            }
            else
            {
                houseSettingsDataContext.Update(settings, CurrentUserId, CurrentHouseId, null);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.About_Breadcrumb)]
        public IActionResult About()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(string username, string messageText)
        {
            var houseModel = _context.Find<House>(CurrentHouseId);

            var message = new Message() { Username = username, Text = messageText, UserId = (long)CurrentUserId, HouseId = (long)CurrentHouseId, CreatedDate = DateTime.Now };

            await messageDataContext.SaveAsync(message, CurrentUserId, CurrentHouseId, null);

            return Ok();
        }

        public JsonResult GetHomeStatus()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                if (_context.HouseStatuses.Any(x => x.HouseId == CurrentHouseId))
                {
                    var currentHouseStatusUserId = _context.HouseStatuses.OrderByDescending(x => x.Id).Where(x => x.HouseId == CurrentHouseId).FirstOrDefault().UserId;
                    var currentHouseStatus = _context.HouseStatuses.OrderByDescending(x => x.Id).Where(x => x.HouseId == CurrentHouseId).FirstOrDefault().Status;
                    var username = _context.Users.Where(x => x.UserId == currentHouseStatusUserId).FirstOrDefault().Username;
                    var currentHouseStatusDetails = _context.HouseStatuses.OrderByDescending(x => x.Id).Where(x => x.HouseId == CurrentHouseId).FirstOrDefault().Details;

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
                var houseModel = _context.Find<House>(CurrentHouseId);

                var houseStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(houseStatusDetailsString))
                {
                    houseStatusDetails = houseStatusDetailsString;
                }

                if (_context.HouseStatuses.Any(x => x.HouseId == CurrentHouseId))
                {
                    var currentHouseStatus = _context.HouseStatuses.OrderByDescending(x => x.Id).Where(x => x.HouseId == CurrentHouseId).FirstOrDefault();

                    currentHouseStatus.Status = int.Parse(houseStatusString);
                    currentHouseStatus.Details = houseStatusDetails;
                    currentHouseStatus.UserId = (long)CurrentUserId;

                    houseStatusDataContext.Update(currentHouseStatus, (long)CurrentUserId, CurrentHouseId, null);
                }
                else
                {
                    var houseStatus = new HouseStatus()
                    {
                        Status = int.Parse(houseStatusString),
                        Details = houseStatusDetails,
                        UserId = (long)CurrentUserId,
                        HouseId = CurrentHouseId
                    };

                    houseStatusDataContext.Save(houseStatus, CurrentUserId, CurrentHouseId, null);
                }
            }

            return Json("");
        }

        [HttpGet]
        [Breadcrumb(HomeBreadcrumbs.History_Breadcrumb)]
        public IActionResult History()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Home");
            }

            var historyModel = new HomeHistoryDisplayView();
            var history = _context.Audits.Where(x => x.HouseId == CurrentHouseId).ToList();

            historyModel.History = historyHtmlBuilder.BuildHomeHistory(history);

            ViewData["HouseName"] = CurrentHouseName;

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
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var displayObjects = new List<BaseObject>();
            var lastInventoryModel = new Inventory();
            var lastHygieneModel = new Hygiene();
            var lastIssueModel = new Issue();

            var inventoryObjects = _context.Set<Inventory>().Where(x => x.HouseId == currentHouseId);

            if (inventoryObjects.Count() > 0)
            {
                var inventoryObject = new Models.Object();
                var objects = new List<Models.Object>();

                foreach (var inventory in inventoryObjects)
                {
                    objects.Add(_context.Set<Models.Object>().Where(x => x.ObjectId == inventory.ObjectId).FirstOrDefault());
                }

                inventoryObject = objects.OrderByDescending(x => x.ModifiedDate).FirstOrDefault();
                lastInventoryModel = _context.Set<Inventory>().Where(x => x.ObjectId == inventoryObject.ObjectId).FirstOrDefault();

                var when = _context.Audits.Where(x => x.ObjectId == lastInventoryModel.ObjectId || x.TargetObjectId == lastInventoryModel.ObjectId)
                    .OrderByDescending(x => x.Id).FirstOrDefault().When;

                lastInventoryModel.LastUpdated = when == null ? string.Empty : timeConverter.CalculateRelativeTime(when);
                lastInventoryModel.LastUpdatedBy = _context.Users.Where(x => x.UserId == inventoryObject.ModifiedById).FirstOrDefault().Username;
                lastInventoryModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastInventoryModel.ObjectId);
            }

            var hygieneObjects = _context.Set<Hygiene>().Where(x => x.HouseId == currentHouseId);

            if (hygieneObjects.Count() > 0)
            {
                var hygieneObject = new Models.Object();
                var objects = new List<Models.Object>();

                foreach (var hygiene in hygieneObjects)
                {
                    objects.Add(_context.Set<Models.Object>().Where(x => x.ObjectId == hygiene.ObjectId).FirstOrDefault());
                }

                hygieneObject = objects.OrderByDescending(x => x.ModifiedDate).FirstOrDefault();
                lastHygieneModel = _context.Set<Hygiene>().Where(x => x.ObjectId == hygieneObject.ObjectId).FirstOrDefault();

                var when = _context.Audits.Where(x => x.ObjectId == lastHygieneModel.ObjectId || x.TargetObjectId == lastHygieneModel.ObjectId)
                    .OrderByDescending(x => x.Id).FirstOrDefault().When;

                lastHygieneModel.LastUpdated = when == null ? string.Empty : timeConverter.CalculateRelativeTime(when);
                lastHygieneModel.LastUpdatedBy = _context.Users.Where(x => x.UserId == hygieneObject.ModifiedById).FirstOrDefault().Username;
                lastHygieneModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastHygieneModel.ObjectId);
            }

            var issueObjects = _context.Set<Issue>().Where(x => x.HouseId == currentHouseId);

            if (issueObjects.Count() > 0)
            {
                var issueObject = new Models.Object();
                var objects = new List<Models.Object>();

                foreach (var issue in issueObjects)
                {
                    objects.Add(_context.Set<Models.Object>().Where(x => x.ObjectId == issue.ObjectId).FirstOrDefault());
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
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var nextMonth = DateTime.Now.AddMonths(1).Month.ToString();
            var thisMonth = DateTime.Now.Month.ToString();
            var rentDueDate = string.Empty;
            var rentDueDateDay = _context.HouseSettings.Find(currentHouseId).RentDueDateDay;

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
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var messages = htmlRenderHelper.BuildMessagesForChat(_context.Messages.ToList(), currentHouseId);

            return messages;
        }
        #endregion

        public void SetUserToCurrentHouse()
        {
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var userIsAlreadyApartOfCurrentHouse = _context.HouseUsers.Any(x => x.UserId == CurrentUserId && x.HouseId == currentHouseId);

            if (!userIsAlreadyApartOfCurrentHouse)
            {
                var houseUser = new HouseUser()
                {
                    HouseId = currentHouseId,
                    UserId = (long)CurrentUserId
                };

                _context.HouseUsers.Add(houseUser);
                _context.SaveChanges();
            }
        }
    }
}
