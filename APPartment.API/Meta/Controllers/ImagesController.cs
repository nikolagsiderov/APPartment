using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    // TODO: Complete this here...
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : BaseAPIController
    {
        public ImagesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        // api/images/image/43
        [HttpGet("image/{ID:long}")]
        public ActionResult GetImage(long ID)
        {
            try
            {
                var result = BaseCRUDService.GetEntity<ImagePostViewModel>(ID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/images/76
        [HttpGet("{targetObjectID:long}")]
        public ActionResult Get(long targetObjectID)
        {
            try
            {
                var result = BaseCRUDService.GetCollection<ImagePostViewModel>(x => x.TargetObjectID == targetObjectID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
