using System.Collections.Generic;
using System.Linq;
using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("{userID:long}")]
        public ActionResult<UserPostViewModel> Get(long userID)
        {
            try
            {
                var result = new BaseWebService(0).GetEntity<UserPostViewModel>(userID);

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

        [HttpGet]
        public ActionResult<List<UserPostViewModel>> Get()
        {
            try
            {
                var result = new BaseWebService(0).GetCollection<UserPostViewModel>();

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
    }
}
