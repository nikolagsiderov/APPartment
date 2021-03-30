using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] UserPostViewModel user)
        {
            try
            {
                var userExists = new BaseWebService(0).Any<UserPostViewModel>(x => x.Name == user.Name);

                if (userExists)
                    return StatusCode(StatusCodes.Status400BadRequest, "This username is already taken.");

                user = new BaseWebService(0).Save(user);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpGet]
        [Route("login")]
        public ActionResult Login(string username, string password)
        {
            try
            {
                var user = new BaseWebService(0).GetEntity<UserPostViewModel>(x => x.Name == username && x.Password == password);

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
