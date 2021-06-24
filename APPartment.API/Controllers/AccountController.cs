using APPartment.Infrastructure.Controllers.Api;
using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using APPAreas = APPartment.Infrastructure.UI.Common.Constants.Areas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace APPartment.API.Controllers
{
    [Area(APPAreas.Account)]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AccountController : BaseAPICRUDController<UserDisplayViewModel, UserPostViewModel>
    {
        protected override Expression<System.Func<UserDisplayViewModel, bool>> FilterExpression => null;

        public AccountController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

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

        protected override void NormalizeDisplayModel(UserDisplayViewModel model)
        {
        }

        protected override void NormalizePostModel(UserPostViewModel model)
        {
        }
    }
}
