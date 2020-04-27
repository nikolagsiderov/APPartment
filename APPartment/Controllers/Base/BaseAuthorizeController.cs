using APPartment.Attributes;
using APPartment.Data;
using Microsoft.AspNetCore.Http;

namespace APPartment.Controllers.Base
{
    [Authorize]
    public abstract class BaseAuthorizeController : BaseController
    {
        public BaseAuthorizeController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
        {
        }
    }
}