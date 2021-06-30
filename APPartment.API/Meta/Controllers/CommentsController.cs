using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Comment;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Web.Html;
using APPartment.Infrastructure.Controllers.Api;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    public class CommentsController : BaseAPIController
    {
        public CommentsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet("{targetObjectID:long}")]
        public ActionResult Get(long targetObjectID)
        {
            try
            {
                // TODO: x.CreatedById != 0 should be handled as case when user is deleted
                var comment = BaseCRUDService.GetCollection<CommentPostViewModel>(x => x.TargetObjectID == targetObjectID && x.CreatedByID != 0);
                var result = new CommentsRenderer(CurrentUserID, CurrentHomeID).BuildComments(comment);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult Post(CommentPostViewModel comment)
        {
            try
            {
                comment = BaseCRUDService.Save(comment);
                var result = new CommentsRenderer(CurrentUserID, CurrentHomeID).BuildPostComment(comment);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
