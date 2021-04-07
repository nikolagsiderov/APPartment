using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPNotification = APPartment.Data.Server.Models.Notification.Notification;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Notification
{
    [IMapFrom(typeof(APPNotification))]
    public class NotificationPostViewModel : PostViewModelWithHome
    {
    }
}
