using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartBreadcrumbs.Attributes;

namespace APPartment.Controllers
{
    public class HygieneController : BaseCRUDController<Hygiene>
    {
        public HygieneController(DataAccessContext context) : base(context)
        {
        }

        [Breadcrumb("<i class='fas fa-recycle' style='font-size:20px'></i> Hygiene")]
        public override Task<IActionResult> Index(string sortOrder, string searchString)
        {
            return base.Index(sortOrder, searchString);
        }

        public override void PopulateViewData()
        {
        }
    }
}
