using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaLiga.Data;
using LaLiga.Models;

namespace LaLiga.Controllers
{
    public class MeczController : Controller
    {
        private readonly LaLigaContext _context;

        public MeczController(LaLigaContext context)
        {
            _context = context;
        }

        // GET: Mecz
        public async Task<IActionResult> Index()
        {
            var laLigaContext = _context.Mecz.Include(m => m.goscie).Include(m => m.gospodarze);
            return View(await laLigaContext.ToListAsync());
        }

        // GET: Mecz/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecz
                .Include(m => m.goscie)
                .Include(m => m.gospodarze)
                .FirstOrDefaultAsync(m => m.id_meczu == id);
            if (mecz == null)
            {
                return NotFound();
            }

            return View(mecz);
        }

        // GET: Mecz/Create
        public IActionResult Create()
        {
            ViewData["id_gosci"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny");
            ViewData["id_gospodarzy"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny");
            return View();
        }

        // POST: Mecz/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_meczu,id_gosci,id_gospodarzy,termin")] Mecz mecz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mecz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_gosci"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", mecz.id_gosci);
            ViewData["id_gospodarzy"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", mecz.id_gospodarzy);
            return View(mecz);
        }

        // GET: Mecz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecz.FindAsync(id);
            if (mecz == null)
            {
                return NotFound();
            }
            ViewData["id_gosci"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", mecz.id_gosci);
            ViewData["id_gospodarzy"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", mecz.id_gospodarzy);
            return View(mecz);
        }

        // POST: Mecz/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_meczu,id_gosci,id_gospodarzy,termin")] Mecz mecz)
        {
            if (id != mecz.id_meczu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mecz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeczExists(mecz.id_meczu))
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
            ViewData["id_gosci"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", mecz.id_gosci);
            ViewData["id_gospodarzy"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", mecz.id_gospodarzy);
            return View(mecz);
        }

        // GET: Mecz/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = await _context.Mecz
                .Include(m => m.goscie)
                .Include(m => m.gospodarze)
                .FirstOrDefaultAsync(m => m.id_meczu == id);
            if (mecz == null)
            {
                return NotFound();
            }

            return View(mecz);
        }

        // POST: Mecz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mecz = await _context.Mecz.FindAsync(id);
            if (mecz != null)
            {
                _context.Mecz.Remove(mecz);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeczExists(int id)
        {
            return _context.Mecz.Any(e => e.id_meczu == id);
        }
    }
}
