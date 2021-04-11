using APPartment.Data.Server.Models.Chat;
using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Chat
{
    [IMapFrom(typeof(Message))]
    public class MessageDisplayViewModel : PostViewModel
    {
    }
}
