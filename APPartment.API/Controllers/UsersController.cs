using System.Collections.Generic;
using System.Linq;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Home;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // api/users/4
        [HttpGet("{userID:long}")]
        public ActionResult<UserPostViewModel> Get(long userID)
        {
            try
            {
                var result = new BaseCRUDService(0).GetEntity<UserPostViewModel>(userID);

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

        // api/users/home/3
        [HttpGet]
        [Route("home/{homeID}")]
        public ActionResult<List<UserPostViewModel>> GetCurrentHomeUsers(long homeID)
        {
            try
            {
                var homeUsers = new BaseCRUDService(0).GetCollection<HomeUserPostViewModel>(x => x.HomeID == homeID);
                var result = new List<UserPostViewModel>();

                foreach (var homeUser in homeUsers)
                {
                    result.Add(new BaseCRUDService(0).GetEntity<UserPostViewModel>(homeUser.UserID));
                }

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
