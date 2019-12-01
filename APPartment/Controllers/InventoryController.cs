using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APPartment.Controllers
{
    public class InventoryController : BaseCRUDController<Inventory>
    {
        public InventoryController(DataAccessContext context) : base(context)
        {
        }

        [Breadcrumb("<i class='fas fa-tasks' style='font-size:20px'></i> Inventory")]
        public override Task<IActionResult> Index(string sortOrder, string searchString)
        {
            return base.Index(sortOrder, searchString);
        }

        public override void PopulateViewData()
        {
        }
    }
}
