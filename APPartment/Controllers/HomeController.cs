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

namespace APPartment.Controllers
{
    public class HomeController : Controller
    {
        #region Breadcrumbs
        private const string Default_Breadcrumb = "<i class='fas fa-home' style='font-size:20px'></i> Home";
        private const string Settings_Breadcrumb = "<i class='fas fa-cog' style='font-size:20px'></i> Settings";
        private const string About_Breadcrumb = "<i class='fas fa-info-circle' style='font-size:20px'></i> About";
        private const string History_Breadcrumb = "<i class='fas fa-history' style='font-size:20px'></i> History";
        #endregion

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

        public HomeController(DataAccessContext context)
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
        [DefaultBreadcrumb(Default_Breadcrumb)]
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

            var homeDisplayModel = new HomeDisplayModel()
            {
                Messages = GetMessages(currentHouseId),
                BaseObjects = displayObjects
            };

            if (_context.HouseStatuses.Where(x => x.HouseId == currentHouseId).Any())
            {
                homeDisplayModel.HouseStatus = _context.HouseStatuses.Where(x => x.HouseId == currentHouseId).OrderByDescending(x => x.Id).FirstOrDefault();
            }

            if (_context.HouseSettings.Any(x => x.HouseId == currentHouseId))
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
                var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

                dataContext.Save(house, currentUserId, null, 0);

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
        [Breadcrumb(Settings_Breadcrumb)]
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

            ViewData["HouseName"] = HttpContext.Session.GetString("HouseName").ToString();

            return View();
        }

        [HttpPost]
        public IActionResult Settings(HouseSettings settings)
        {
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

            var houseModel = _context.Find<House>(currentHouseId);
            settings.HouseId = currentHouseId;

            if (!string.IsNullOrEmpty(settings.HouseName) || settings.HouseName != houseModel.Name)
            {
                houseModel.Name = settings.HouseName;

                dataContext.Update(houseModel, currentUserId, currentHouseId);
                HttpContext.Session.SetString("HouseName", houseModel.Name.ToString());
            }

            if (settings.Id == 0)
            {
                settings.HouseId = currentHouseId;
                houseSettingsDataContext.Save(settings, currentUserId, houseModel.ObjectId, currentHouseId);
            }
            else
            {
                houseSettingsDataContext.Update(settings, currentUserId, currentHouseId);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Breadcrumb(About_Breadcrumb)]
        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(string username, string messageText)
        {
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var houseModel = _context.Find<House>(currentHouseId);

            var message = new Message() { Username = username, Text = messageText, UserId = currentUserId, HouseId = currentHouseId, CreatedDate = DateTime.Now };

            await messageDataContext.SaveAsync(message, currentUserId, houseModel.ObjectId, currentHouseId);

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

                var houseModel = _context.Find<House>(currentHouseId);

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

                    houseStatusDataContext.Update(currentHouseStatus, (long)currentUserId, (long)currentHouseId);
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

                    houseStatusDataContext.Save(houseStatus, (long)currentUserId, houseModel.ObjectId, (long)currentHouseId);
                }
            }

            return Json("");
        }

        [HttpGet]
        [Breadcrumb(History_Breadcrumb)]
        public IActionResult History()
        {
            var historyModel = new HomeHistoryDisplayView();
            long? currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));
            var history = _context.Histories.Where(x => x.HouseId == currentHouseId).ToList();

            historyModel.History = historyHtmlBuilder.BuildHomeHistory(history);

            ViewData["HouseName"] = HttpContext.Session.GetString("HouseName").ToString();

            return View(historyModel);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Getters
        public List<BaseObject> GetDisplayObject(long? currentHouseId)
        {
            var displayObjects = new List<BaseObject>();
            var lastInventoryModel = new Inventory();
            var lastHygieneModel = new Hygiene();
            var lastIssueModel = new Issue();

            var inventoryObjects = _context.Set<Inventory>().Where(x => x.HouseId == currentHouseId);

            if (inventoryObjects.Count() > 0)
            {
                var inventoryObject = _context.Set<Models.Object>().Where(x => x.ObjectTypeId == (long)ObjectTypes.Inventory).OrderByDescending(x => x.ModifiedDate).First();
                lastInventoryModel = _context.Set<Inventory>().Where(x => x.ObjectId == inventoryObject.ObjectId).FirstOrDefault();

                var when = _context.Histories.Where(x => x.ObjectId == lastInventoryModel.ObjectId || x.TargetId == lastInventoryModel.ObjectId)
                    .OrderByDescending(x => x.Id).FirstOrDefault().When;

                lastInventoryModel.LastUpdated = when == null ? string.Empty : timeConverter.CalculateRelativeTime(when);
                lastInventoryModel.LastUpdatedBy = _context.Users.Where(x => x.UserId == inventoryObject.ModifiedById).FirstOrDefault().Username;
                lastInventoryModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastInventoryModel.ObjectId);
            }

            var hygieneObjects = _context.Set<Hygiene>().Where(x => x.HouseId == currentHouseId);

            if (hygieneObjects.Count() > 0)
            {
                var hygieneObject = _context.Set<Models.Object>().Where(x => x.ObjectTypeId == (long)ObjectTypes.Hygiene).OrderByDescending(x => x.ModifiedDate).First();
                lastHygieneModel = _context.Set<Hygiene>().Where(x => x.ObjectId == hygieneObject.ObjectId).FirstOrDefault();

                var when = _context.Histories.Where(x => x.ObjectId == lastHygieneModel.ObjectId || x.TargetId == lastHygieneModel.ObjectId)
                    .OrderByDescending(x => x.Id).FirstOrDefault().When;

                lastHygieneModel.LastUpdated = when == null ? string.Empty : timeConverter.CalculateRelativeTime(when);
                lastHygieneModel.LastUpdatedBy = _context.Users.Where(x => x.UserId == hygieneObject.ModifiedById).FirstOrDefault().Username;
                lastHygieneModel.LastUpdate = historyHtmlBuilder.BuildLastUpdateBaseObjectHistoryForWidget(lastHygieneModel.ObjectId);
            }

            var issueObjects = _context.Set<Issue>().Where(x => x.HouseId == currentHouseId);

            if (issueObjects.Count() > 0)
            {
                var issueObject = _context.Set<Models.Object>().Where(x => x.ObjectTypeId == (long)ObjectTypes.Issue).OrderByDescending(x => x.ModifiedDate).First();
                lastIssueModel = _context.Set<Issue>().Where(x => x.ObjectId == issueObject.ObjectId).FirstOrDefault();

                var when = _context.Histories.Where(x => x.ObjectId == lastIssueModel.ObjectId || x.TargetId == lastIssueModel.ObjectId)
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
            var rentDueDateDay = _context.HouseSettings.Find(long.Parse(HttpContext.Session.GetString("HouseId"))).RentDueDateDay;

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

        private List<string> GetMessages(long currentHouseId)
        {
            var messages = htmlRenderHelper.BuildMessagesForChat(_context.Messages.ToList(), currentHouseId);

            return messages;
        }
        #endregion
    }
}
