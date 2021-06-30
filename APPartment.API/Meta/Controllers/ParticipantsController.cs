using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    public class ParticipantsController : BaseAPIController
    {
        public ParticipantsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet("{targetObjectID:long}")]
        public ActionResult Get(long targetObjectID)
        {
            try
            {
                var result = BaseCRUDService.GetCollection<ObjectParticipantPostViewModel>(x => x.TargetObjectID == targetObjectID);

                foreach (var participant in result)
                {
                    participant.Username = BaseCRUDService.GetEntity<UserPostViewModel>(participant.UserID).Name;
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost("{targetObjectID:long}")]
        public ActionResult Post(long targetObjectID, [FromBody] string username)
        {
            try
            {
                var userExists = BaseCRUDService.Any<UserPostViewModel>(x => x.Name == username);

                if (userExists)
                {
                    var user = BaseCRUDService.GetEntity<UserPostViewModel>(x => x.Name == username);
                    BaseCRUDService.AddUserAsParticipantToObjectIfNecessary(targetObjectID, user.ID);
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
