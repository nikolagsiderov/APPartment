using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;

namespace APPartment.Controllers
{
    public class IssuesController : BaseCRUDController<Issue>
    {
        public IssuesController(DataAccessContext context) : base(context)
        {
        }
    }
}
