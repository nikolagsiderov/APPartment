﻿using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Chat;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.Infrastructure.UI.Web.Html
{
    public class ChatRenderer
    {
        private readonly BaseCRUDService BaseService;

        public ChatRenderer(long? currentUserID, long? currentHomeID)
        {
            BaseService = new BaseCRUDService(currentUserID, currentHomeID);
        }

        public List<string> BuildMessagesForChat(List<MessageDisplayViewModel> messages)
        {
            return messages
                .OrderByDescending(x => x.ID)
                .Select(x => $"{BaseService.GetEntity<UserPostViewModel>(x.CreatedByID).Name}: {x.Details}")
                .ToList();
        }
    }
}
