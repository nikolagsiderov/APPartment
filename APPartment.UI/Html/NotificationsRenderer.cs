using APPartment.UI.ViewModels.Notification;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.UI.Html
{
    public static class NotificationsRenderer
    {
        public static string BuildNotificationsContent(List<NotificationPostViewModel> notifications)
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
