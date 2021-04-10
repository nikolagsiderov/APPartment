using System.Collections.Generic;
using System.Linq;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        // api/events/82
        [HttpGet("{targetObjectID:long}")]
        public ActionResult Get(long targetObjectID)
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var events = new BaseCRUDService(currentUserID).GetCollection<EventPostViewModel>(x => x.TargetObjectID == targetObjectID);

                foreach (var @event in events)
                {
                    var participantUsernames = new List<string>();
                    var eventParticipants = new BaseCRUDService(currentUserID).GetCollection<EventParticipantPostViewModel>(x => x.EventID == @event.ID);

                    foreach (var participant in eventParticipants)
                    {
                        var participantUsername = new BaseCRUDService(currentUserID).GetEntity<UserPostViewModel>(x => x.ID == participant.UserID).Name;
                        participantUsernames.Add(participantUsername);
                    }

                    @event.EventParticipantNames = string.Join(", ", participantUsernames);
                }

                return Ok(events);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/events/post
        [HttpPost]
        [Route("post")]
        public ActionResult Post([FromBody] EventPostViewModel @event)
        {
            try
            {
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var participants = @event.ParticipantUserIDs;

                @event = new BaseCRUDService(currentUserID).Save(@event);
                new BaseCRUDService(currentUserID).AddUserAsParticipantToObject(@event.TargetObjectID, currentUserID, @event.ObjectTypeID);

                foreach (var userID in participants)
                {
                    var eventParticipant = new EventParticipantPostViewModel() { EventID = @event.ID, UserID = long.Parse(userID) };
                    new BaseCRUDService(currentUserID).Save(eventParticipant);
                }

                var participantUsernames = new List<string>();
                var eventParticipants = new BaseCRUDService(currentUserID).GetCollection<EventParticipantPostViewModel>(x => x.EventID == @event.ID);

                foreach (var participant in eventParticipants)
                {
                    var participantUsername = new BaseCRUDService(currentUserID).GetEntity<UserPostViewModel>(x => x.ID == participant.UserID).Name;
                    participantUsernames.Add(participantUsername);
                }

                @event.EventParticipantNames = string.Join(", ", participantUsernames);

                return Ok(@event);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
