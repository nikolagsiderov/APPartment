using System.Linq;
using APPartment.UI.Services.Base;
using APPartment.UI.Html;
using APPartment.UI.ViewModels.Clingons.Comment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        // api/comments/82
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

                // TODO: x.CreatedById != 0 should be handled as case when user is deleted
                var comment = new BaseWebService(currentUserID).GetCollection<CommentPostViewModel>(x => x.TargetObjectID == targetObjectID && x.CreatedByID != 0);
                var result = new CommentsRenderer(currentUserID).BuildComments(comment);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/comments/post
        [HttpPost]
        [Route("post")]
        public ActionResult Post([FromBody] CommentPostViewModel comment)
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

                comment = new BaseWebService(currentUserID).Save(comment);
                new BaseWebService(currentUserID).AddUserAsParticipantToObject(comment.TargetObjectID, currentUserID, comment.ObjectTypeID);
                var result = new CommentsRenderer(currentUserID).BuildPostComment(comment);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
