using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
using APPartment.Common;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using System.Collections.Generic;

namespace APPartment.API.Controllers
{
    [Area(APPAreas.Default)]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseAPIController
    {
        public HomeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet("{homeID:long}")]
        public ActionResult<HomePostViewModel> Get(long homeID)
        {
            try
            {
                var result = BaseCRUDService.GetEntity<HomePostViewModel>(homeID);

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

        [HttpGet]
        public ActionResult<List<HomePostViewModel>> Get()
        {
            try
            {
                var result = BaseCRUDService.GetCollection<HomePostViewModel>();

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

        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] HomePostViewModel home)
        {
            try
            {
                var homeNameAlreadyExists = BaseCRUDService.Any<HomePostViewModel>(x => x.Name == home.Name);

                if (homeNameAlreadyExists)
                    return StatusCode(StatusCodes.Status403Forbidden, "This home name is already taken.");

                home = BaseCRUDService.Save(home);
                return Ok(home);
            }
            catch (Exception ex)
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
                home = BaseCRUDService.GetEntity<HomePostViewModel>(x => x.Name == home.Name && x.Password == home.Password);

                if (home != null)
                    return Ok(home);
                else
                    return NotFound();
            }
            catch (Exception ex)
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
                var messages = BaseCRUDService.GetCollection<MessageDisplayViewModel>(x => x.HomeID == CurrentHomeID && x.CreatedByID != 0);
                var messagesResult = new ChatRenderer(CurrentUserID, CurrentHomeID).BuildMessagesForChat(messages);

                var result = new HomePageDisplayModel()
                {
                    Messages = messagesResult
                };

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

        // api/home/chat?username=something&messageText=somethingelse
        [HttpPost]
        [Route("chat")]
        public ActionResult PostChatMessage(string username, string messageText)
        {
            try
            {
                var adjustedMessage = string.Join(" <br /> ", messageText.Split('\n').ToList());
                var message = new MessageDisplayViewModel() { Details = adjustedMessage, CreatedByID = (long)CurrentUserID, HomeID = CurrentHomeID, CreatedDate = DateTime.Now };

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
                    UserID = (long)CurrentUserID
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
                var result = BaseCRUDService.GetCollection(x => x.HomeID == CurrentHomeID && x.ObjectID != excludedObjectID);

                foreach (var item in result)
                {
                    var areaName = item.Area;
                    var controllerName = item.Area;
                    var page = "Details";
                    item.WebLink = string.Format($"{Configuration.DefaultBaseURL}/{areaName}/{controllerName}/{page}/{item.MainID}");
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

        [HttpGet]
        [Route("objects/search/{keyWords}")]
        public ActionResult GetAllObjects(string keyWords)
        {
            try
            {
                var result = BaseCRUDService.GetCollection(x => x.HomeID == CurrentHomeID && (x.Name == keyWords || x.Details == keyWords));

                foreach (var item in result)
                {
                    var areaName = item.Area;
                    var controllerName = item.Area;
                    var page = "Details";
                    item.WebLink = string.Format($"{Configuration.DefaultBaseURL}/{areaName}/{controllerName}/{page}/{item.MainID}");
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
