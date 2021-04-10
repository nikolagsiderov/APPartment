using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.Infrastructure.UI.Web.Html
{
    public static class EventsRenderer
    {
        public static List<string> BuildEvents(List<EventPostViewModel> events)
        {
            return events
                .OrderByDescending(x => x.ID)
                .Take(20)
                .Select(e => $"<div class='row'><div class='col-md-2'>{e.Name}</div><div class='col-md-3'>{e.Details}</div><div class='col-md-2'>{e.StartDate.ToString("dd'/'MM'/'yyyy")}</div><div class='col-md-2'>{e.EndDate.ToString("dd'/'MM'/'yyyy")}</div><div class='col-md-2'>{e.EventParticipantNames}</div></div>")
                .ToList();
        }

        public static string BuildPostEvent(EventPostViewModel @event)
        {
            return $"<div class='row'><div class='col-md-2'>{@event.Name}</div><div class='col-md-3'>{@event.Details}</div><div class='col-md-2'>{@event.StartDate.ToString("dd'/'MM'/'yyyy")}</div><div class='col-md-2'>{@event.EndDate.ToString("dd'/'MM'/'yyyy")}</div><div class='col-md-2'>{@event.EventParticipantNames}</div></div>";
        }
    }
}
