using System.Collections.Generic;
using System.Linq;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Home;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/home/{homeID}/[controller]")]
    [ApiController]
    public class RoommatesController : ControllerBase
    {
        // api/home/4/chores/mine
        [HttpGet]
        [Route("roommates")]
        public ActionResult<List<UserDisplayViewModel>> GetRoommates(long homeID)
        {
            try
            {
                var homeUsers = new BaseCRUDService(0).GetCollection<HomeUserPostViewModel>(x => x.HomeID == homeID);
                var result = new List<UserDisplayViewModel>();

                foreach (var homeUser in homeUsers)
                {
                    result.Add(new BaseCRUDService(0).GetEntity<UserDisplayViewModel>(homeUser.UserID));
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
