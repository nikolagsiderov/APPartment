using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPNotificationParticipant = APPartment.Data.Server.Models.Notification.NotificationParticipant;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Notification
{
    [IMapFrom(typeof(APPNotificationParticipant))]
    public class NotificationParticipantPostViewModel : PostViewModel
    {
        public long NotificationID { get; set; }

        public long UserID { get; set; }
    }
}
