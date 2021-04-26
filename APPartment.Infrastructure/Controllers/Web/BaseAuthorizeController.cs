using APPartment.Infrastructure.Attributes;
using Microsoft.AspNetCore.Http;

namespace APPartment.Infrastructure.Controllers.Web
{
    [Authorize]
    public abstract class BaseAuthorizeController : BaseController
    {
        public BaseAuthorizeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }
    }
}