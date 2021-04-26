using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : BaseAPIController
    {
        public ParticipantsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        // api/participants/82
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
    }
}
