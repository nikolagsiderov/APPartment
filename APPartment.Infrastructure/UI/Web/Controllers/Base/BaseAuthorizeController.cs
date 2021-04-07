using APPartment.Infrastructure.UI.Web.Attributes;
using Microsoft.AspNetCore.Http;

namespace APPartment.Infrastructure.UI.Web.Controllers.Base
{
    [Authorize]
    public abstract class BaseAuthorizeController : BaseController
    {
        public BaseAuthorizeController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }
    }
}