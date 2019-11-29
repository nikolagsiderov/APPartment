using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;

namespace APPartment.Controllers
{
    public class InventoryController : BaseCRUDController<Inventory>
    {
        public InventoryController(DataAccessContext context) : base(context)
        {
        }
    }
}
