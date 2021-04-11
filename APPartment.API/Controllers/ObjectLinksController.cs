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
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                var links = new BaseCRUDService(currentUserID).GetCollection<ObjectLinkPostViewModel>(x => x.TargetObjectID == targetObjectID);

                foreach (var link in links)
                {
                    //TODO: Fill in ObjectAName & ObjectBName
                    // In order to do that, we need to call .GetEntity<BusinessObject>(ObjectAID)
                }

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
                var currentUserID = 0l;
                var currentHomeID = 0l;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                link = new BaseCRUDService(currentUserID).Save(link);
                new BaseCRUDService(currentUserID).AddUserAsParticipantToObject(link.TargetObjectID, currentUserID, link.ObjectTypeID);

                //TODO: Fill in ObjectAName & ObjectBName
                // In order to do that, we need to call .GetEntity<BusinessObject>(ObjectAID)

                return Ok(link);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
