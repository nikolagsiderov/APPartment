using APPartment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APPartment.Utilities
{
    public class HtmlRenderHelper
    {
        public List<string> BuildMessagesForChat(List<Message> messages, long currentHouseId)
        {
            return messages.Where(x => x.HouseId == currentHouseId).OrderByDescending(x => x.Id).Take(10).OrderBy(x => x.Id).Select(x => $"{x.Username}: {x.Text}").ToList();
        }

        public List<string> BuildComments(List<Comment> comments, long targetId)
        {
            return comments.Where(x => x.TargetId == targetId)
                .OrderByDescending(x => x.Id).Take(20).Select(x => $"<strong>{x.Username}</strong>: {x.Text}").ToList();
        }

        public string BuildPostComment(Comment comment)
        {
            return $"<strong>{comment.Username}</strong>: {comment.Text}";
        }
    }
}
