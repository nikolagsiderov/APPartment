using System.Collections.Generic;
using System.Linq;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/home/{homeID}/[controller]")]
    [ApiController]
    public class NeighborsController : ControllerBase
    {
        // api/home/4/neighbors/neighbors
        [HttpGet]
        [Route("neighbors")]
        public ActionResult<List<HomeDisplayViewModel>> GetNeighbors(long homeID)
        {
            try
            {
                var result = new BaseCRUDService(0).GetCollection<HomeDisplayViewModel>(x => x.HomeID != homeID);

                if (result.Any())
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/4/neighbors/12
        [HttpGet]
        [Route("{neighborID}")]
        public ActionResult<List<HomeDisplayViewModel>> GetNeighbors(long homeID, long neighborID)
        {
            try
            {
                var result = new BaseCRUDService(0).GetEntity<HomeDisplayViewModel>(neighborID);

                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
