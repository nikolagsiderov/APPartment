using APPartment.Data.Server.Models.Notification;
using APPartment.ORM.Framework.Core;
using System.Collections.Generic;

namespace APPartment.Data.Core
{
    public class NotificationFacade : BaseFacade
    {
        private readonly DaoContext dao;

        public NotificationFacade() : base()
        {
            dao = new DaoContext();
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
