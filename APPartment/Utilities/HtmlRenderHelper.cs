using APPartment.Data;
using APPartment.Models;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.Utilities
{
    public class HtmlRenderHelper
    {
        private DataAccessContext context;

        public HtmlRenderHelper(DataAccessContext context)
        {
            this.context = context;
        }

        public List<string> BuildMessagesForChat(List<Message> messages, long homeId)
        {
            return messages.Where(x => x.HomeId == homeId).OrderByDescending(x => x.Id).Take(10).OrderBy(x => x.Id).Select(x => $"{x.Username}: {x.Text}").ToList();
        }

        public List<string> BuildComments(List<Comment> comments, long targetId)
        {
            return comments.Where(x => x.TargetId == targetId)
                .OrderByDescending(x => x.Id).Take(20).Select(x => $"<strong>{x.Username}</strong>: {x.Text} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{context.Objects.Where(y => y.ObjectId == x.ObjectId).FirstOrDefault().CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>").ToList();
        }

        public string BuildPostComment(Comment comment)
        {
            return $"<strong>{comment.Username}</strong>: {comment.Text} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{context.Objects.Where(y => y.ObjectId == comment.ObjectId).FirstOrDefault().CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>";
        }
    }
}
