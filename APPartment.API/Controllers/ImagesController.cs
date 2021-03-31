using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.Clingons.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APPartment.API.Controllers
{
    // TODO: Complete this here...
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        // api/images/image/43
        [HttpGet("image/{ID:long}")]
        public ActionResult GetImage(long ID)
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

                var result = new BaseWebService(currentUserID).GetEntity<ImagePostViewModel>(ID);

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
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var result = new BaseWebService(currentUserID).GetCollection<ImagePostViewModel>(x => x.TargetObjectID == targetObjectID);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
