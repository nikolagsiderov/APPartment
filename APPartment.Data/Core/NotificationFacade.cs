using APPartment.Data.Server.Models.Notification;
using System.Collections.Generic;

namespace APPartment.Data.Core
{
    public class NotificationFacade : BaseFacade
    {
        public NotificationFacade() : base()
        {
        }

        public void Notify(string title, string content, List<long> userIDs, long currentHomeID, long currentUserID)
        {
            var notification = new Notification() { Name = title, Details = content, HomeID = currentHomeID };
            var savedNotification = this.Create<Notification>(notification, currentUserID);

            foreach (var userID in userIDs)
            {
                var notificationParticipant = new NotificationParticipant() { NotificationID = savedNotification.ID, UserID = userID };
                this.Create<NotificationParticipant>(notificationParticipant, currentUserID);
            }
        }
    }
}
