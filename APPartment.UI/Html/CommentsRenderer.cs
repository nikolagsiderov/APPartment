using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.Clingons.Comment;
using APPartment.UI.ViewModels.User;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.UI.Html
{
    public class CommentsRenderer
    {
        private readonly BaseWebService BaseWebService;

        public CommentsRenderer(long? currentUserID)
        {
            BaseWebService = new BaseWebService(currentUserID);
        }

        public List<string> BuildComments(List<CommentPostViewModel> comments)
        {
            return comments
                .OrderByDescending(x => x.ID)
                .Take(20)
                .Select(x => $"<strong>{BaseWebService.GetEntity<UserPostViewModel>(x.CreatedByID).Name}</strong>: {x.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{x.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>")
                .ToList();
        }

        public string BuildPostComment(CommentPostViewModel comment)
        {
            return $"<strong>{BaseWebService.GetEntity<UserPostViewModel>(comment.CreatedByID).Name}</strong>: {comment.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{comment.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>";
        }
    }
}
