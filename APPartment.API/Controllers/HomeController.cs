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
using APPartment.Infrastructure.UI.Common.ViewModels.Chore;
using APPartment.Infrastructure.UI.Common.ViewModels.Issue;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Tools;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseAPIController
    {
        public HomeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

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

                var result = BaseCRUDService.GetEntity<HomePostViewModel>(homeID);

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

                var result = BaseCRUDService.GetCollection<HomePostViewModel>();

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

                var homeNameAlreadyExists = BaseCRUDService.Any<HomePostViewModel>(x => x.Name == home.Name);

                if (homeNameAlreadyExists)
                    return StatusCode(StatusCodes.Status403Forbidden, "This home name is already taken.");

                home = BaseCRUDService.Save(home);
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

                home = BaseCRUDService.GetEntity<HomePostViewModel>(x => x.Name == home.Name && x.Password == home.Password);

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
                var messages = BaseCRUDService.GetCollection<MessageDisplayViewModel>(x => x.HomeID == CurrentHomeID && x.CreatedByID != 0);
                var messagesResult = new ChatRenderer(CurrentUserID, CurrentHomeID).BuildMessagesForChat(messages);

                var result = new HomePageDisplayModel()
                {
                    Messages = messagesResult
                };

                if (BaseCRUDService.Any<HomeSettingPostViewModel>(x => x.HomeID == CurrentHomeID))
                {
                    var nextMonth = DateTime.Now.AddMonths(1).Month.ToString();
                    var thisMonth = DateTime.Now.Month.ToString();
                    var rentDueDate = string.Empty;
                    var rentDueDateDay = BaseCRUDService.GetEntity<HomeSettingPostViewModel>(x => x.HomeID == CurrentHomeID).RentDueDateDay;

                    if (rentDueDateDay.ToString() != "0")
                    {
                        var dateString = $"{rentDueDateDay}/{nextMonth}/{DateTime.Now.Year.ToString()}";

                        if (DateTime.Parse(dateString).AddMonths(-1).Date > DateTime.Now.Date)
                            dateString = $"{rentDueDateDay}/{thisMonth}/{DateTime.Now.Year.ToString()}";

                        rentDueDate = DateTime.Parse(dateString).ToLongDateString();
                    }

                    result.RentDueDate = rentDueDate;
                }

                if (BaseCRUDService.Any<InventoryPostViewModel>(x => x.HomeID == CurrentHomeID))
                {
                    var latestInventoryItem = BaseCRUDService
                                        .GetCollection<InventoryPostViewModel>(x => x.HomeID == CurrentHomeID)
                                        .OrderByDescending(x => x.ModifiedDate)
                                        .FirstOrDefault();

                    result.InventoryLastUpdate = TimeConverter.CalculateRelativeTime(latestInventoryItem.ModifiedDate.Value);
                }
                else
                {
                    result.InventoryLastUpdate = "Now";
                }

                if (BaseCRUDService.Any<ChorePostViewModel>(x => x.HomeID == CurrentHomeID))
                {
                    var latestChore = BaseCRUDService
                                        .GetCollection<ChorePostViewModel>(x => x.HomeID == CurrentHomeID)
                                        .OrderByDescending(x => x.ModifiedDate)
                                        .FirstOrDefault();

                    result.ChoresLastUpdate = TimeConverter.CalculateRelativeTime(latestChore.ModifiedDate.Value);
                }
                else
                {
                    result.ChoresLastUpdate = "Now";
                }

                if (BaseCRUDService.Any<IssuePostViewModel>(x => x.HomeID == CurrentHomeID))
                {
                    var latestIssue = BaseCRUDService
                                        .GetCollection<IssuePostViewModel>(x => x.HomeID == CurrentHomeID)
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
                var result = new HomeStatusPostViewModel();

                if (BaseCRUDService.Any<HomeStatusPostViewModel>(x => x.HomeID == CurrentHomeID))
                    result = BaseCRUDService.GetEntity<HomeStatusPostViewModel>(x => x.HomeID == CurrentHomeID);

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
                var homeStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(homeStatusDetailsString))
                    homeStatusDetails = homeStatusDetailsString;

                if (BaseCRUDService.Any<HomeStatusPostViewModel>(x => x.HomeID == CurrentHomeID))
                {
                    var status = BaseCRUDService.GetEntity<HomeStatusPostViewModel>(x => x.HomeID == CurrentHomeID);

                    status.Status = int.Parse(homeStatusString);
                    status.Details = homeStatusDetails;
                    status.UserID = CurrentUserID;

                    status = BaseCRUDService.Save(status);

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
                        UserID = CurrentUserID,
                        HomeID = CurrentHomeID
                    };

                    status = BaseCRUDService.Save(status);

                    if (status != null)
                        return Ok(status);
                    else
                        return NotFound();
                }
            }
            catch (Exception ex)
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
                var result = BaseCRUDService.GetEntity<HomeSettingPostViewModel>(x => x.HomeID == CurrentHomeID);

                if (result != null)
                {
                    var homeModel = BaseCRUDService.GetEntity<HomePostViewModel>(CurrentHomeID);
                    result.HomeName = homeModel.Name;
                }

                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (Exception ex)
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
                var settingsExists = BaseCRUDService.GetEntity<HomeSettingPostViewModel>(x => x.HomeID == CurrentHomeID);

                if (settingsExists != null)
                    settings.ID = settingsExists.ID;

                settings.HomeID = CurrentHomeID;

                var home = BaseCRUDService.GetEntity<HomePostViewModel>(CurrentHomeID);
                var currentHomeName = home.Name;

                if (!string.IsNullOrEmpty(settings.HomeName) && !home.Name.Equals(settings.HomeName))
                {
                    home.Name = settings.HomeName;
                    BaseCRUDService.Save(home);
                }

                settings = BaseCRUDService.Save(settings);

                if (!string.IsNullOrEmpty(settings.HomeName) && !currentHomeName.Equals(settings.HomeName))
                    settings.ChangeHttpSession = true;

                if (settings != null)
                    return Ok(settings);
                else
                    return NotFound();
            }
            catch (Exception ex)
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
                var adjustedMessage = string.Join(" <br /> ", messageText.Split('\n').ToList());
                var message = new MessageDisplayViewModel() { Details = adjustedMessage, CreatedByID = CurrentUserID, HomeID = CurrentHomeID, CreatedDate = DateTime.Now };

                message = BaseCRUDService.Save(message);

                return Ok(message);
            }
            catch (Exception ex)
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
                var homeUser = new HomeUserPostViewModel()
                {
                    HomeID = CurrentHomeID,
                    UserID = CurrentUserID
                };

                var userIsAlreadyApartOfCurrentHome = BaseCRUDService.Any<HomeUserPostViewModel>(x => x.UserID == homeUser.UserID && x.HomeID == homeUser.HomeID);

                if (!userIsAlreadyApartOfCurrentHome)
                    BaseCRUDService.Save(homeUser);

                return Ok();
            }
            catch (Exception ex)
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
                var result = BaseCRUDService.GetCollection(x => x.HomeID == CurrentHomeID);

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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
