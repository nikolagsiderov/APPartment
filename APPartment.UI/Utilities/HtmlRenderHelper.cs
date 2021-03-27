using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.Chat;
using APPartment.UI.ViewModels.Clingons.Comment;
using APPartment.UI.ViewModels.Notification;
using APPartment.UI.ViewModels.User;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.UI.Utilities
{
    public class HtmlRenderHelper
    {
        private readonly BaseWebService BaseWebService;

        public HtmlRenderHelper(long? currentUserID)
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

        public string BuildNotificationsContent(List<NotificationPostViewModel> notifications)
        {
            var headerTitle = "<h6 class=\"dropdown-header dropdown-notifications-header\">Notifications</h6>";
            var contents = new List<string>();
            var result = headerTitle;

            if (!notifications.Any())
            {
                var content = @$"<a class='dropdown-item dropdown-notifications-item' href='#!' id='no-notification'>
                    <div class='dropdown-notifications-item-icon bg-info'><i class='fas fa-info' style='color: white;'></i></div>
                    <div class='dropdown-notifications-item-content'>
                        <div class='dropdown-notifications-item-content-details' id='no-notificationTitle'>No notifications!</div>
                        <div class='dropdown-notifications-item-content-text' id='no-notificationDetails'>No notifications yet!</div>
                    </div>
                </a>";

                contents.Add(content);
            }

            foreach (var notification in notifications)
            {
                var content = @$"<a class='dropdown-item dropdown-notifications-item' href='#!' id='notification-{notification.ID}'>
                    <div class='dropdown-notifications-item-icon bg-info'><i class='fas fa-info' style='color: white;'></i></div>
                    <div class='dropdown-notifications-item-content'>
                        <div class='dropdown-notifications-item-content-details' id='notificationTitle-{notification.ID}'>{notification.Name}</div>
                        <div class='dropdown-notifications-item-content-text' id='notificationDetails-{notification.ID}'>{notification.Details}</div>
                    </div>
                </a>";

                contents.Add(content);
            }

            if (contents.Any())
                result += string.Join(" ", contents);

            return result;
        }
    }
}
