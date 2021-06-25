using System.Collections.Generic;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : BaseAPIController
    {
        public EventsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        // api/events/82
        [HttpGet("{targetObjectID:long}")]
        public ActionResult Get(long targetObjectID)
        {
            try
            {
                var events = BaseCRUDService.GetCollection<EventPostViewModel>(x => x.TargetObjectID == targetObjectID);

                foreach (var @event in events)
                {
                    var participantUsernames = new List<string>();
                    var eventParticipants = BaseCRUDService.GetCollection<EventParticipantPostViewModel>(x => x.EventID == @event.ID);

                    foreach (var participant in eventParticipants)
                    {
                        var participantUsername = BaseCRUDService.GetEntity<UserPostViewModel>(x => x.ID == participant.UserID).Name;
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
                var participants = @event.ParticipantUserIDs;

                @event = BaseCRUDService.Save(@event);
                BaseCRUDService.AddUserAsParticipantToObject(@event.TargetObjectID, (long)CurrentUserID, @event.ObjectTypeID);

                foreach (var userID in participants)
                {
                    var eventParticipant = new EventParticipantPostViewModel() { EventID = @event.ID, UserID = long.Parse(userID) };
                    BaseCRUDService.Save(eventParticipant);
                }

                var participantUsernames = new List<string>();
                var eventParticipants = BaseCRUDService.GetCollection<EventParticipantPostViewModel>(x => x.EventID == @event.ID);

                foreach (var participant in eventParticipants)
                {
                    var participantUsername = BaseCRUDService.GetEntity<UserPostViewModel>(x => x.ID == participant.UserID).Name;
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
