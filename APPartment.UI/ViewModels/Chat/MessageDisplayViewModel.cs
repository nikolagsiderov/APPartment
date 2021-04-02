using APPartment.Data.Server.Models.Chat;
using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;

namespace APPartment.UI.ViewModels.Chat
{
    [IMapFrom(typeof(Message))]
    public class MessageDisplayViewModel : PostViewModelWithHome
    {
    }
}
