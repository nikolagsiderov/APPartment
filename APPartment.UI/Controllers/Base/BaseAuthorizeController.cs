using APPartment.UI.Attributes;
using Microsoft.AspNetCore.Http;

namespace APPartment.UI.Controllers.Base
{
    [Authorize]
    public abstract class BaseAuthorizeController : BaseController
    {
        public BaseAuthorizeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }
    }
}