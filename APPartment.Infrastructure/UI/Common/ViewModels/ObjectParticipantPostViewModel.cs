using APPartment.Data.Server.Models;
using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;

namespace APPartment.Infrastructure.UI.Common.ViewModels
{
    [IMapFrom(typeof(ObjectParticipant))]
    public class ObjectParticipantPostViewModel : PostViewModel
    {
        public long TargetObjectID { get; set; }

        public long UserID { get; set; }

        public string Username { get; set; }
    }
}
