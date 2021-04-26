using APPartment.Data.Server;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Notification;
using System.Collections.Generic;

namespace APPartment.Infrastructure.Services
{
    public class NotificationService : BaseCRUDService
    {
        private NotificationFacade NotificationFacade;

        public NotificationService(long? currentUserID, long? currentHomeID) : base(currentUserID, currentHomeID)
        {
            NotificationFacade = new NotificationFacade();
        }

        public List<NotificationPostViewModel> GetNotifications()
        {
            var currentUserNotificationParticipation = this.GetCollection<NotificationParticipantPostViewModel>(x => x.UserID == (long)CurrentUserID);
            var notifications = new List<NotificationPostViewModel>();

            foreach (var participation in currentUserNotificationParticipation)
            {
                var notification = this.GetEntity<NotificationPostViewModel>(participation.NotificationID);
                notifications.Add(notification);
            }

            return notifications;
        }

        public void Notify(string title, string content, List<long> userIDs)
        {
            NotificationFacade.Notify(title, content, userIDs, (long)CurrentHomeID, (long)CurrentUserID);
        }
    }
}
