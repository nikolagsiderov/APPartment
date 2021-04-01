using APPartment.Data.Server.Models;
using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;

namespace APPartment.UI.ViewModels
{
    [IMapFrom(typeof(ObjectParticipant))]
    public class ObjectParticipantPostViewModel : PostViewModel
    {
        public long TargetObjectID { get; set; }

        public long UserID { get; set; }

        public string Username { get; set; }
    }
}
