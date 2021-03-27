using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using APPNotification = APPartment.Data.Server.Models.Notification.Notification;

namespace APPartment.UI.ViewModels.Notification
{
    [IMapFrom(typeof(APPNotification))]
    public class NotificationPostViewModel : PostViewModelWithHome
    {
    }
}
