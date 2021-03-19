using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.Chat;
using APPartment.UI.ViewModels.Comment;
using APPartment.UI.ViewModels.User;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.UI.Utilities
{
    public class HtmlRenderHelper
    {
        private readonly BaseWebService BaseWebService;

        public HtmlRenderHelper(long? currentUserId)
        {
            BaseWebService = new BaseWebService(currentUserId);
        }

        public List<string> BuildMessagesForChat(List<MessageDisplayViewModel> messages, long homeId)
        {
            return messages
                .OrderByDescending(x => x.Id)
                .Take(10)
                .Select(x => $"{BaseWebService.GetEntity<UserPostViewModel>(x.CreatedById).Name}: {x.Details}")
                .ToList();
        }

        public List<string> BuildComments(List<CommentPostViewModel> comments, long targetObjectId)
        {
            return comments
                .OrderByDescending(x => x.Id)
                .Take(20)
                .Select(x => $"<strong>{BaseWebService.GetEntity<UserPostViewModel>(x.CreatedById).Name}</strong>: {x.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{x.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>")
                .ToList();
        }

        public string BuildPostComment(CommentPostViewModel comment)
        {
            return $"<strong>{BaseWebService.GetEntity<UserPostViewModel>(comment.CreatedById).Name}</strong>: {comment.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{comment.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>";
        }
    }
}
