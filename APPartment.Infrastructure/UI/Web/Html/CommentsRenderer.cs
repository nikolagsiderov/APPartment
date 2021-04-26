using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Comment;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.Infrastructure.UI.Web.Html
{
    public class CommentsRenderer
    {
        private readonly BaseCRUDService BaseService;

        public CommentsRenderer(long? currentUserID, long? currentHomeID)
        {
            BaseService = new BaseCRUDService(currentUserID, currentHomeID);
        }

        public List<string> BuildComments(List<CommentPostViewModel> comments)
        {
            return comments
                .OrderByDescending(x => x.ID)
                .Take(20)
                .Select(x => $"<strong>{BaseService.GetEntity<UserPostViewModel>(x.CreatedByID).Name}</strong>: {x.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{x.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>")
                .ToList();
        }

        public string BuildPostComment(CommentPostViewModel comment)
        {
            return $"<strong>{BaseService.GetEntity<UserPostViewModel>(comment.CreatedByID).Name}</strong>: {comment.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{comment.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>";
        }
    }
}
