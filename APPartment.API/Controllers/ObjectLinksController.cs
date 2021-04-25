using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Link;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectLinksController : ControllerBase
    {
        // api/objectlinks/82
        [HttpGet("{targetObjectID:long}")]
        public ActionResult Get(long targetObjectID)
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var forwardLinks = new BaseCRUDService(currentUserID).GetCollection<ObjectLinkPostViewModel>(x => x.TargetObjectID == targetObjectID);
                var backwardLinks = new BaseCRUDService(currentUserID).GetCollection<ObjectLinkPostViewModel>(x => x.ObjectBID == targetObjectID);

                foreach (var link in forwardLinks)
                {
                    link.ObjectBName = new BaseCRUDService(currentUserID).GetEntity(link.ObjectBID).Name;
                }

                foreach (var link in backwardLinks)
                {
                    link.ObjectBName = new BaseCRUDService(currentUserID).GetEntity(link.TargetObjectID).Name;
                }

                var links = forwardLinks;
                links.AddRange(backwardLinks);

                return Ok(links);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/objectlinks/post
        [HttpPost]
        [Route("post")]
        public ActionResult Post([FromBody] ObjectLinkPostViewModel link)
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                link = new BaseCRUDService(currentUserID).Save(link);
                new BaseCRUDService(currentUserID).AddUserAsParticipantToObject(link.TargetObjectID, currentUserID, link.ObjectTypeID);

                link.ObjectBName = new BaseCRUDService(currentUserID).GetEntity(link.ObjectBID).Name;

                return Ok(link);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
