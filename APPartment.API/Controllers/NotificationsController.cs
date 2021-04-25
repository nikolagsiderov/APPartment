using System.Linq;
using APPartment.Infrastructure.Services;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Notification;
using APPartment.Infrastructure.UI.Web.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        // api/notifications/contents
        [HttpGet]
        [Route("contents")]
        public ActionResult GetContents()
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

                var notifications = new NotificationService(currentUserID, currentHomeID).GetNotifications();
                var result = NotificationsRenderer.BuildNotificationsContent(notifications);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/notifications/count
        [HttpGet]
        [Route("count")]
        public ActionResult GetCount()
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

                var result = new BaseCRUDService(currentUserID).Count<NotificationParticipantPostViewModel>(x => x.UserID == currentUserID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
