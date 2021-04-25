using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using APPartment.Infrastructure.UI.Common.ViewModels.Home;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Web.Html;
using APPartment.Infrastructure.UI.Common.ViewModels.Chat;
using APPartment.Infrastructure.UI.Common.ViewModels.Inventory;
using APPartment.Infrastructure.UI.Common.Tools;
using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using APPartment.Infrastructure.UI.Common.ViewModels.Issue;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // api/home/5
        [HttpGet("{homeID:long}")]
        public ActionResult<HomePostViewModel> Get(long homeID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).GetEntity<HomePostViewModel>(homeID);

                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home
        [HttpGet]
        public ActionResult<List<HomePostViewModel>> Get()
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).GetCollection<HomePostViewModel>();

                if (result.Any())
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/register
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] HomePostViewModel home)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var homeNameAlreadyExists = new BaseCRUDService(0).Any<HomePostViewModel>(x => x.Name == home.Name);

                if (homeNameAlreadyExists)
                    return StatusCode(StatusCodes.Status403Forbidden, "This home name is already taken.");

                home = new BaseCRUDService(currentUserID).Save(home);
                return Ok(home);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/login
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] HomePostViewModel home)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                home = new BaseCRUDService(currentUserID).GetEntity<HomePostViewModel>(x => x.Name == home.Name && x.Password == home.Password);

                if (home != null)
                    return Ok(home);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/page
        [HttpGet]
        [Route("page")]
        public ActionResult GetHomePage()
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var messages = new BaseCRUDService(currentUserID).GetCollection<MessageDisplayViewModel>(x => x.HomeID == currentHomeID && x.CreatedByID != 0);
                var messagesResult = new ChatRenderer(currentUserID).BuildMessagesForChat(messages);

                var result = new HomePageDisplayModel()
                {
                    Messages = messagesResult
                };

                if (new BaseCRUDService(currentUserID).Any<HomeSettingPostViewModel>(x => x.HomeID == currentHomeID))
                {
                    var nextMonth = DateTime.Now.AddMonths(1).Month.ToString();
                    var thisMonth = DateTime.Now.Month.ToString();
                    var rentDueDate = string.Empty;
                    var rentDueDateDay = new BaseCRUDService(currentUserID).GetEntity<HomeSettingPostViewModel>(x => x.HomeID == currentHomeID).RentDueDateDay;

                    if (rentDueDateDay.ToString() != "0")
                    {
                        var dateString = $"{rentDueDateDay}/{nextMonth}/{DateTime.Now.Year.ToString()}";

                        if (DateTime.Parse(dateString).AddMonths(-1).Date > DateTime.Now.Date)
                            dateString = $"{rentDueDateDay}/{thisMonth}/{DateTime.Now.Year.ToString()}";

                        rentDueDate = DateTime.Parse(dateString).ToLongDateString();
                    }

                    result.RentDueDate = rentDueDate;
                }

                if (new BaseCRUDService(currentUserID)
                    .Any<InventoryPostViewModel>(x => x.HomeID == currentHomeID))
                {
                    var latestInventoryItem = new BaseCRUDService(currentUserID)
                                        .GetCollection<InventoryPostViewModel>(x => x.HomeID == currentHomeID)
                                        .OrderByDescending(x => x.ModifiedDate)
                                        .FirstOrDefault();

                    result.InventoryLastUpdate = TimeConverter.CalculateRelativeTime(latestInventoryItem.ModifiedDate.Value);
                }
                else
                {
                    result.InventoryLastUpdate = "Now";
                }

                if (new BaseCRUDService(currentUserID)
                    .Any<ChorePostViewModel>(x => x.HomeID == currentHomeID))
                {
                    var latestChore = new BaseCRUDService(currentUserID)
                                        .GetCollection<ChorePostViewModel>(x => x.HomeID == currentHomeID)
                                        .OrderByDescending(x => x.ModifiedDate)
                                        .FirstOrDefault();

                    result.ChoresLastUpdate = TimeConverter.CalculateRelativeTime(latestChore.ModifiedDate.Value);
                }
                else
                {
                    result.ChoresLastUpdate = "Now";
                }

                if (new BaseCRUDService(currentUserID)
                    .Any<IssuePostViewModel>(x => x.HomeID == currentHomeID))
                {
                    var latestIssue = new BaseCRUDService(currentUserID)
                                        .GetCollection<IssuePostViewModel>(x => x.HomeID == currentHomeID)
                                        .OrderByDescending(x => x.ModifiedDate)
                                        .FirstOrDefault();

                    result.IssuesLastUpdate = TimeConverter.CalculateRelativeTime(latestIssue.ModifiedDate.Value);
                }
                else
                {
                    result.IssuesLastUpdate = "Now";
                }

                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/status
        [HttpGet]
        [Route("status")]
        public ActionResult GetHomeStatus()
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var result = new HomeStatusPostViewModel();

                if (new BaseCRUDService(currentUserID).Any<HomeStatusPostViewModel>(x => x.HomeID == currentHomeID))
                    result = new BaseCRUDService(currentUserID).GetEntity<HomeStatusPostViewModel>(x => x.HomeID == currentHomeID);

                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/status?homeStatusString=something&homeStatusDetailsString=somethingelse
        [HttpPost]
        [Route("status")]
        public ActionResult PostHomeStatus(string homeStatusString, string homeStatusDetailsString)
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var homeStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(homeStatusDetailsString))
                    homeStatusDetails = homeStatusDetailsString;

                if (new BaseCRUDService(currentUserID).Any<HomeStatusPostViewModel>(x => x.HomeID == currentHomeID))
                {
                    var status = new BaseCRUDService(currentUserID).GetEntity<HomeStatusPostViewModel>(x => x.HomeID == currentHomeID);

                    status.Status = int.Parse(homeStatusString);
                    status.Details = homeStatusDetails;
                    status.UserID = currentUserID;

                    new BaseCRUDService(currentUserID).Save(status);

                    if (status != null)
                        return Ok(status);
                    else
                        return NotFound();
                }
                else
                {
                    var status = new HomeStatusPostViewModel()
                    {
                        Status = int.Parse(homeStatusString),
                        Details = homeStatusDetails,
                        UserID = currentUserID,
                        HomeID = currentHomeID
                    };

                    new BaseCRUDService(currentUserID).Save(status);

                    if (status != null)
                        return Ok(status);
                    else
                        return NotFound();
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/settings
        [HttpGet]
        [Route("settings")]
        public ActionResult GetHomeSettings()
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var result = new BaseCRUDService(currentUserID).GetEntity<HomeSettingPostViewModel>(x => x.HomeID == currentHomeID);

                if (result != null)
                {
                    var homeModel = new BaseCRUDService(currentUserID).GetEntity<HomePostViewModel>(currentHomeID);
                    result.HomeName = homeModel.Name;
                }

                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/settings
        [HttpPost]
        [Route("settings")]
        public ActionResult PostHomeSettings([FromBody] HomeSettingPostViewModel settings)
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var settingsExists = new BaseCRUDService(currentUserID).GetEntity<HomeSettingPostViewModel>(x => x.HomeID == currentHomeID);

                if (settingsExists != null)
                    settings.ID = settingsExists.ID;

                settings.HomeID = currentHomeID;

                var home = new BaseCRUDService(currentUserID).GetEntity<HomePostViewModel>(currentHomeID);
                var currentHomeName = home.Name;

                if (!string.IsNullOrEmpty(settings.HomeName) && !home.Name.Equals(settings.HomeName))
                {
                    home.Name = settings.HomeName;
                    new BaseCRUDService(currentUserID).Save(home);
                }

                settings = new BaseCRUDService(currentUserID).Save(settings);

                if (!string.IsNullOrEmpty(settings.HomeName) && !currentHomeName.Equals(settings.HomeName))
                    settings.ChangeHttpSession = true;

                if (settings != null)
                    return Ok(settings);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/chat?username=something&messageText=somethingelse
        [HttpPost]
        [Route("chat")]
        public ActionResult PostChatMessage(string username, string messageText)
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var adjustedMessage = string.Join(" <br /> ", messageText.Split('\n').ToList());
                var message = new MessageDisplayViewModel() { Details = adjustedMessage, CreatedByID = currentUserID, HomeID = currentHomeID, CreatedDate = DateTime.Now };

                message = new BaseCRUDService(currentUserID).Save(message);

                return Ok(message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/homeuser
        [HttpPost]
        [Route("homeuser")]
        public ActionResult SetUserToCurrentHome()
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var homeUser = new HomeUserPostViewModel()
                {
                    HomeID = currentHomeID,
                    UserID = currentUserID
                };

                var userIsAlreadyApartOfCurrentHome = new BaseCRUDService(currentUserID).Any<HomeUserPostViewModel>(x => x.UserID == homeUser.UserID && x.HomeID == homeUser.HomeID);

                if (!userIsAlreadyApartOfCurrentHome)
                    new BaseCRUDService(currentUserID).Save(homeUser);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/objects/422
        [HttpGet]
        [Route("objects/{excludedObjectID}")]
        public ActionResult GetObjects(long excludedObjectID)
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var result = new BaseCRUDService(currentUserID).GetCollection(x => x.HomeID == currentHomeID);

                foreach (var item in result)
                {
                    if (item.ObjectID == excludedObjectID)
                    {
                        result.Remove(item);
                        break;
                    }
                }

                if (result.Any())
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
