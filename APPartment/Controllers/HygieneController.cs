using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;

namespace APPartment.Controllers
{
    public class HygieneController : BaseCRUDController<Hygiene>
    {
        public HygieneController(DataAccessContext context) : base(context)
        {
        }
    }
}
