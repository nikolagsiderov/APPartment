using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : BaseAPIController
    {
        private FileService fileService { get; set; }

        public ImagesController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            fileService = new FileService(CurrentUserID, CurrentHomeID);
        }

        [HttpGet("image/{ID:long}")]
        public ActionResult GetImage(long ID)
        {
            try
            {
                var result = BaseCRUDService.GetEntity<ImagePostViewModel>(ID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpGet("{targetObjectID:long}")]
        public ActionResult Get(long targetObjectID)
        {
            try
            {
                var result = BaseCRUDService.GetCollection<ImagePostViewModel>(x => x.TargetObjectID == targetObjectID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost("upload/{targetObjectID:long}")]
        public ActionResult Upload(long targetObjectID, ICollection<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    fileService.UploadImage(file, targetObjectID);
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpDelete("image/{ID:long}")]
        public ActionResult Delete(long ID)
        {
            try
            {
                fileService.DeleteImage(ID);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
