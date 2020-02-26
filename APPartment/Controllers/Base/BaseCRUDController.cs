using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPartment.Data;
using APPartment.Models;
using APPartment.Models.Declaration;
using APPartment.Utilities;
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
        private HtmlRenderHelper htmlRenderHelper = new HtmlRenderHelper();

        public BaseCRUDController(DataAccessContext context)
        {
            _context = context;
        }

        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<T>().Where(x => x.HouseId == currentHouseId).ToListAsync();

            return View("_Grid", modelObjects);
        }

        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> IndexCompleted()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<T>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == true).ToListAsync();

            return View("_Grid", modelObjects);
        }

        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> IndexNotCompleted()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<T>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == false).ToListAsync();

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

            model.Comments = GetComments(model.ObjectId);

            return View("_Details", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Details,Status,IsCompleted,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,HouseId,ObjectId")] T model)
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

                // Define Object
                var objectDb = new APPartment.Models.Object();
                _context.Add(objectDb);
                await _context.SaveChangesAsync();

                // Assign Base Object to Object
                model.ObjectId = objectDb.ObjectId;

                _context.Add(model);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
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

            model.Comments = GetComments(model.ObjectId);

            return View("_Edit", model);
        }

        [Breadcrumb("<i class='fas fa-edit'></i> Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Details,Status,IsCompleted,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,HouseId,ObjectId")] T model)
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

        public async Task<IActionResult> Complete(long? id)
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

            model.IsCompleted = true;

            _context.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarkNotCompleted(long? id)
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

            model.IsCompleted = false;

            _context.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

        // Base Object Metadata
        public List<string> GetComments(long targetId)
        {
            var comments = htmlRenderHelper.BuildComments(_context.Comment.ToList(), targetId);

            return comments;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(long targetId, string commentText)
        {
            var username = HttpContext.Session.GetString("Username");

            var comment = new Comment()
            {
                Text = commentText,
                TargetId = targetId,
                Username = username
            };

            _context.Add(comment);
            await _context.SaveChangesAsync();

            var result = htmlRenderHelper.BuildPostComment(comment);

            return Json(result);
        }

        private bool ObjectExists(long id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }

        public virtual void PopulateViewData()
        { }
    }
}