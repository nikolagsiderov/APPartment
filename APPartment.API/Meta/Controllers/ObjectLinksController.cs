using System;
using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Link;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    public class ObjectLinksController : BaseAPIController
    {
        public ObjectLinksController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet("{targetObjectID:long}")]
        public ActionResult Get(long targetObjectID)
        {
            try
            {
                var forwardLinks = BaseCRUDService.GetCollection<ObjectLinkPostViewModel>(x => x.TargetObjectID == targetObjectID);
                var backwardLinks = BaseCRUDService.GetCollection<ObjectLinkPostViewModel>(x => x.ObjectBID == targetObjectID);

                foreach (var link in forwardLinks)
                {
                    link.ObjectBName = BaseCRUDService.GetEntity(link.ObjectBID).Name;
                }

                foreach (var link in backwardLinks)
                {
                    link.ObjectBName = BaseCRUDService.GetEntity(link.TargetObjectID).Name;
                }

                var links = forwardLinks;
                links.AddRange(backwardLinks);

                return Ok(links);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult Post(ObjectLinkPostViewModel link)
        {
            try
            {
                link = BaseCRUDService.Save(link);

                link.ObjectBName = BaseCRUDService.GetEntity(link.ObjectBID).Name;

                return Ok(link);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
