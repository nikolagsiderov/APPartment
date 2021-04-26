using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPEventParticipant = APPartment.Data.Server.Models.Clingons.EventParticipant;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event
{
    [IMapFrom(typeof(APPEventParticipant))]
    public class EventParticipantPostViewModel : PostViewModel
    {
        public long EventID { get; set; }

        public long UserID { get; set; }
    }
}
