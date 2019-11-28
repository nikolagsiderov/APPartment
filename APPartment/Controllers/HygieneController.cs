using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APPartment.Data;
using APPartment.Models;

namespace APPartment.Controllers
{
    public class HygieneController : Controller
    {
        private readonly DataAccessContext _context;

        public HygieneController(DataAccessContext context)
        {
            _context = context;
        }

        // GET: Hygiene
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Hygiene";

            return View("_Grid", await _context.Hygiene.ToListAsync());
        }

        // GET: Hygiene/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hygiene = await _context.Hygiene
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hygiene == null)
            {
                return NotFound();
            }

            return View("_Details", hygiene);
        }

        // GET: Hygiene/Create
        public IActionResult Create()
        {
            return View("_Create");
        }

        // POST: Hygiene/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Details,Status,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,HouseId")] Hygiene hygiene)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hygiene);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("_Create", hygiene);
        }

        // GET: Hygiene/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hygiene = await _context.Hygiene.FindAsync(id);
            if (hygiene == null)
            {
                return NotFound();
            }
            return View("_Edit", hygiene);
        }

        // POST: Hygiene/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Details,Status,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,HouseId")] Hygiene hygiene)
        {
            if (id != hygiene.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hygiene);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HygieneExists(hygiene.Id))
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
            return View("_Edit", hygiene);
        }

        // GET: Hygiene/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hygiene = await _context.Hygiene
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hygiene == null)
            {
                return NotFound();
            }

            _context.Hygiene.Remove(hygiene);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HygieneExists(long id)
        {
            return _context.Hygiene.Any(e => e.Id == id);
        }
    }
}
