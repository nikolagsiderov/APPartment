using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using APPNotificationParticipant = APPartment.Data.Server.Models.Notification.NotificationParticipant;

namespace APPartment.UI.ViewModels.Notification
{
    [IMapFrom(typeof(APPNotificationParticipant))]
    public class NotificationParticipantPostViewModel : PostViewModel
    {
        public long NotificationID { get; set; }

        public long UserID { get; set; }
    }
}
