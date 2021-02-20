using APPartment.Data.Server.Models.Core;
using APPartment.Data.Server.Models.MetaObjects;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.UI.Utilities
{
    public class HtmlRenderHelper
    {
        public HtmlRenderHelper()
        {
        }

        public List<string> BuildMessagesForChat(List<Message> messages, long homeId)
        {
            // TODO: Get username where we got TestUser
            return messages.Where(x => x.HomeId == homeId).OrderByDescending(x => x.Id).Take(10).OrderByDescending(x => x.Id).Select(x => $"TestUser: {x.Details}").ToList();
        }

        public List<string> BuildComments(List<Comment> comments, long targetObjectId)
        {
            // TODO: Get username where we got TestUser
            return comments.Where(x => x.TargetObjectId == targetObjectId)
                .OrderByDescending(x => x.Id).Take(20).Select(x => $"<strong>TestUser</strong>: {x.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{x.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>").ToList();
        }

        public string BuildPostComment(Comment comment)
        {
            // TODO: Get username where we got TestUser
            return $"<strong>TestUser</strong>: {comment.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{comment.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>";
        }
    }
}
