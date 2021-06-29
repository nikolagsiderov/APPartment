using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Notification;
using APPartment.Infrastructure.UI.Web.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    public class NotificationsController : BaseAPIController
    {
        public NotificationsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet]
        [Route("contents")]
        public ActionResult GetContents()
        {
            try
            {
                var notifications = new NotificationService(CurrentUserID, CurrentHomeID).GetNotifications();
                var result = NotificationsRenderer.BuildNotificationsContent(notifications);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpGet]
        [Route("count")]
        public ActionResult GetCount()
        {
            try
            {
                var result = BaseCRUDService.Count<NotificationParticipantPostViewModel>(x => x.UserID == CurrentUserID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
