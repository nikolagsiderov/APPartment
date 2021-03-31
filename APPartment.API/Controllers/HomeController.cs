using APPartment.UI.Services.Base;
using APPartment.UI.Utilities;
using APPartment.UI.ViewModels.Chat;
using APPartment.UI.ViewModels.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("{homeID:long}")]
        public ActionResult<HomePostViewModel> Get(long homeID)
        {
            try
            {
                var currentUserID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseWebService(currentUserID).GetEntity<HomePostViewModel>(homeID);

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

        [HttpGet]
        public ActionResult<List<HomePostViewModel>> Get()
        {
            try
            {
                var currentUserID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseWebService(currentUserID).GetCollection<HomePostViewModel>();

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

        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] HomePostViewModel home)
        {
            try
            {
                var currentUserID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var homeNameAlreadyExists = new BaseWebService(0).Any<HomePostViewModel>(x => x.Name == home.Name);

                if (homeNameAlreadyExists)
                    return StatusCode(StatusCodes.Status403Forbidden, "This home name is already taken.");

                home = new BaseWebService(currentUserID).Save(home);
                return Ok(home);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] HomePostViewModel home)
        {
            try
            {
                var currentUserID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                home = new BaseWebService(currentUserID).GetEntity<HomePostViewModel>(x => x.Name == home.Name && x.Password == home.Password);

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

        [HttpGet]
        [Route("page")]
        public ActionResult GetHomePage()
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var messages = new BaseWebService(currentUserID).GetCollection<MessageDisplayViewModel>(x => x.HomeID == currentHomeID && x.CreatedByID != 0);
                var messagesResult = new HtmlRenderHelper(currentUserID).BuildMessagesForChat(messages);

                var result = new HomePageDisplayModel()
                {
                    Messages = messagesResult
                };

                if (new BaseWebService(currentUserID).Any<HomeSettingPostViewModel>(x => x.HomeID == currentHomeID))
                {
                    var nextMonth = DateTime.Now.AddMonths(1).Month.ToString();
                    var thisMonth = DateTime.Now.Month.ToString();
                    var rentDueDate = string.Empty;
                    var rentDueDateDay = new BaseWebService(currentUserID).GetEntity<HomeSettingPostViewModel>(x => x.HomeID == currentHomeID).RentDueDateDay;

                    if (rentDueDateDay.ToString() != "0")
                    {
                        var dateString = $"{rentDueDateDay}/{nextMonth}/{DateTime.Now.Year.ToString()}";

                        if (DateTime.Parse(dateString).AddMonths(-1).Date > DateTime.Now.Date)
                            dateString = $"{rentDueDateDay}/{thisMonth}/{DateTime.Now.Year.ToString()}";

                        rentDueDate = DateTime.Parse(dateString).ToLongDateString();
                    }

                    result.RentDueDate = rentDueDate;
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

        [HttpGet]
        [Route("status")]
        public ActionResult GetHomeStatus()
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var result = new HomeStatusPostViewModel();

                if (new BaseWebService(currentUserID).Any<HomeStatusPostViewModel>(x => x.HomeID == currentHomeID))
                    result = new BaseWebService(currentUserID).GetEntity<HomeStatusPostViewModel>(x => x.HomeID == currentHomeID);

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

        [HttpPost]
        [Route("status")]
        public ActionResult PostHomeStatus(string homeStatusString, string homeStatusDetailsString)
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var homeStatusDetails = string.Empty;

                if (!string.IsNullOrEmpty(homeStatusDetailsString))
                    homeStatusDetails = homeStatusDetailsString;

                if (new BaseWebService(currentUserID).Any<HomeStatusPostViewModel>(x => x.HomeID == currentHomeID))
                {
                    var status = new BaseWebService(currentUserID).GetEntity<HomeStatusPostViewModel>(x => x.HomeID == currentHomeID);

                    status.Status = int.Parse(homeStatusString);
                    status.Details = homeStatusDetails;
                    status.UserID = currentUserID;

                    new BaseWebService(currentUserID).Save(status);

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

                    new BaseWebService(currentUserID).Save(status);

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

        [HttpGet]
        [Route("settings")]
        public ActionResult GetHomeSettings()
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var result = new BaseWebService(currentUserID).GetEntity<HomeSettingPostViewModel>(x => x.HomeID == currentHomeID);

                if (result != null)
                {
                    var homeModel = new BaseWebService(currentUserID).GetEntity<HomePostViewModel>(currentHomeID);
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

        [HttpPost]
        [Route("settings")]
        public ActionResult PostHomeSettings([FromBody] HomeSettingPostViewModel settings)
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var settingsExists = new BaseWebService(currentUserID).GetEntity<HomeSettingPostViewModel>(x => x.HomeID == currentHomeID);

                if (settingsExists != null)
                    settings.ID = settingsExists.ID;

                settings.HomeID = currentHomeID;

                var home = new BaseWebService(currentUserID).GetEntity<HomePostViewModel>(currentHomeID);
                var currentHomeName = home.Name;

                if (!string.IsNullOrEmpty(settings.HomeName) && !home.Name.Equals(settings.HomeName))
                {
                    home.Name = settings.HomeName;
                    new BaseWebService(currentUserID).Save(home);
                }

                settings = new BaseWebService(currentUserID).Save(settings);

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

        [HttpPost]
        [Route("chat")]
        public ActionResult PostChatMessage(string username, string messageText)
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var adjustedMessage = string.Join(" <br /> ", messageText.Split('\n').ToList());
                var message = new MessageDisplayViewModel() { Details = adjustedMessage, CreatedByID = currentUserID, HomeID = currentHomeID, CreatedDate = DateTime.Now };

                message = new BaseWebService(currentUserID).Save(message);

                return Ok(message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost]
        [Route("homeuser")]
        public ActionResult SetUserToCurrentHome()
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
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

                var userIsAlreadyApartOfCurrentHome = new BaseWebService(currentUserID).Any<HomeUserPostViewModel>(x => x.UserID == homeUser.UserID && x.HomeID == homeUser.HomeID);

                if (!userIsAlreadyApartOfCurrentHome)
                    new BaseWebService(currentUserID).Save(homeUser);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
