using APPartment.Data.Core;
using APPartment.UI.ViewModels.Notification;
using System.Collections.Generic;

namespace APPartment.UI.Services.Base
{
    public class NotificationService : BaseWebService
    {
        private NotificationFacade NotificationFacade;
        private long? CurrentUserID;
        private long? CurrentHomeID;

        public NotificationService(long? currentUserID, long? currentHomeID) : base(currentUserID)
        {
            NotificationFacade = new NotificationFacade();
            CurrentUserID = currentUserID;
            CurrentHomeID = currentHomeID;
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
