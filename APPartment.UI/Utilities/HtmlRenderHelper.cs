using APPartment.Data.Core;
using APPartment.Data.Server.Models.Core;
using APPartment.Data.Server.Models.MetaObjects;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.UI.Utilities
{
    public class HtmlRenderHelper
    {
        private readonly BaseFacade baseFacade;

        public HtmlRenderHelper()
        {
            baseFacade = new BaseFacade();
        }

        public List<string> BuildMessagesForChat(List<Message> messages, long homeId)
        {
            return messages
                .OrderByDescending(x => x.Id)
                .Take(10)
                .Select(x => $"{baseFacade.GetObject<User>(x.CreatedById).Name}: {x.Details}")
                .ToList();
        }

        public List<string> BuildComments(List<Comment> comments, long targetObjectId)
        {
            return comments
                .OrderByDescending(x => x.Id)
                .Take(20)
                .Select(x => $"<strong>{baseFacade.GetObject<User>(x.CreatedById).Name}</strong>: {x.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{x.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>")
                .ToList();
        }

        public string BuildPostComment(Comment comment)
        {
            return $"<strong>{baseFacade.GetObject<User>(comment.CreatedById).Name}</strong>: {comment.Details} <br/> <strong><span class=\"text-muted\" style=\"font-size: x-small;\">{comment.CreatedDate.ToString("dd'/'MM'/'yyyy HH:mm:ss")}</span></strong>";
        }
    }
}
