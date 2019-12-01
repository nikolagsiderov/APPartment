using APPartment.Data;
using APPartment.Models;
using APPartment.Controllers.Base;
using SmartBreadcrumbs.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APPartment.Controllers
{
    public class IssuesController : BaseCRUDController<Issue>
    {
        public IssuesController(DataAccessContext context) : base(context)
        {
        }

        [Breadcrumb("<i class='fas fa-exclamation-circle' style='font-size:20px'></i> Issues")]
        public override Task<IActionResult> Index(string sortOrder, string searchString)
        {
            return base.Index(sortOrder, searchString);
        }

        public override void PopulateViewData()
        {
        }
    }
}
