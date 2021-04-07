using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // api/account/register
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] UserPostViewModel user)
        {
            try
            {
                var userAlreadyExists = new BaseCRUDService(0).Any<UserPostViewModel>(x => x.Name == user.Name);

                if (userAlreadyExists)
                    return StatusCode(StatusCodes.Status403Forbidden, "This username is already taken.");

                user = new BaseCRUDService(0).Save(user);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/account/login
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] UserPostViewModel user)
        {
            try
            {
                user = new BaseCRUDService(0).GetEntity<UserPostViewModel>(x => x.Name == user.Name && x.Password == user.Password);

                if (user != null)
                    return Ok(user);
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
