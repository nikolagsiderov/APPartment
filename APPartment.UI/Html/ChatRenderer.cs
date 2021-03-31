using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.Chat;
using APPartment.UI.ViewModels.User;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.UI.Html
{
    public class ChatRenderer
    {
        private readonly BaseWebService BaseWebService;

        public ChatRenderer(long? currentUserID)
        {
            BaseWebService = new BaseWebService(currentUserID);
        }

        public List<string> BuildMessagesForChat(List<MessageDisplayViewModel> messages)
        {
            return messages
                .OrderByDescending(x => x.ID)
                .Take(10)
                .Select(x => $"{BaseWebService.GetEntity<UserPostViewModel>(x.CreatedByID).Name}: {x.Details}")
                .ToList();
        }
    }
}
