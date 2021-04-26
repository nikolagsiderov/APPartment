using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseAPIController
    {
        public AccountController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        // api/account/register
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] UserPostViewModel user)
        {
            try
            {
                var userAlreadyExists = BaseCRUDService.Any<UserPostViewModel>(x => x.Name == user.Name);

                if (userAlreadyExists)
                    return StatusCode(StatusCodes.Status403Forbidden, "This username is already taken.");

                user = BaseCRUDService.Save(user);
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
                user = BaseCRUDService.GetEntity<UserPostViewModel>(x => x.Name == user.Name && x.Password == user.Password);

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
