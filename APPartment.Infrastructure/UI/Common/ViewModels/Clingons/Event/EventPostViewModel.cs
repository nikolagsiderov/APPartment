using APPartment.Infrastructure.Services.Attributes;
using APPEvent = APPartment.Data.Server.Models.Clingons.Event;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using System;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event
{
    [IMapFrom(typeof(APPEvent))]
    public class EventPostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Date)]
        public DateTime StartDate { get; set; }

        [APPUIHint(Templates.Date)]
        public DateTime EndDate { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long TargetObjectID { get; set; }

        public string[] ParticipantUserIDs { get; set; }

        public string EventParticipantNames { get; set; }
    }
}
