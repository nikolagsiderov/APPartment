using APPartment.Data.Server.Models.Base;
using APPartment.Data.Server.Models.Chat;
using APPartment.UI.Attributes;

namespace APPartment.UI.ViewModels.Chat
{
    [IMapFrom(typeof(Message))]
    public class MessageDisplayViewModel : HomeBaseObject
    {
    }
}
