using System;
using System.Linq;
using System.Threading.Tasks;
using APPartment.Data;
using APPartment.Models.Declaration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;

namespace APPartment.Controllers.Base
{
    public abstract class BaseCRUDController<T> : Controller
        where T : class, IBaseObject
    {
        private readonly DataAccessContext _context;

        public BaseCRUDController(DataAccessContext context)
        {
            _context = context;
        }

        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DetailsSortParm"] = sortOrder == "details_asc" ? "details_desc" : "details_asc";
            ViewData["CurrentFilter"] = searchString;

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<T>().Where(x => x.HouseId == currentHouseId).ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                modelObjects = modelObjects.Where(s => s.Name.Contains(searchString)
                                       || s.Details.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    modelObjects = modelObjects.OrderByDescending(s => s.Name).ToList();
                    break;
                case "details_asc":
                    modelObjects = modelObjects.OrderBy(s => s.Details).ToList();
                    break;
                case "details_desc":
                    modelObjects = modelObjects.OrderByDescending(s => s.Details).ToList();
                    break;
                default:
                    modelObjects = modelObjects.OrderBy(s => s.Name).ToList();
                    break;
            }

            return View("_Grid", modelObjects);
        }

        [Breadcrumb("<i class='fas fa-info-circle'></i> Details")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return View("_Details", model);
        }

        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        public IActionResult Create()
        {
            return View("_Create");
        }

        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Details,Status,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,HouseId")] T model)
        {
            if (ModelState.IsValid)
            {
                var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                model.HouseId = currentHouseId;

                // Base History Properties
                model.CreatedBy = _context.User.Find(long.Parse(HttpContext.Session.GetString("UserId"))).Username;
                model.CreatedDate = DateTime.Now;
                model.ModifiedBy = model.CreatedBy;
                model.ModifiedDate = model.CreatedDate;

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View("_Create", model);
        }

        [Breadcrumb("<i class='fas fa-edit'></i> Edit")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Set<T>().FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View("_Edit", model);
        }

        [Breadcrumb("<i class='fas fa-edit'></i> Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Details,Status,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,HouseId")] T model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

                model.HouseId = currentHouseId;

                // Base History Properties
                model.ModifiedBy = _context.User.Find(long.Parse(HttpContext.Session.GetString("UserId"))).Username;
                model.ModifiedDate = DateTime.Now;

                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjectExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View("_Edit", model);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            _context.Set<T>().Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObjectExists(long id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }

        public virtual void PopulateViewData()
        { }
    }
}