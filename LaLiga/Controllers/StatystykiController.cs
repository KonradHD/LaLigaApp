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
    public class StatystykiController : Controller
    {
        private readonly LaLigaContext _context;

        public StatystykiController(LaLigaContext context)
        {
            _context = context;
        }

        // GET: Statystyki
        public async Task<IActionResult> Index()
        {
            var laLigaContext = _context.Statystyki.Include(s => s.mecz);
            return View(await laLigaContext.ToListAsync());
        }

        // GET: Statystyki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statystyki = await _context.Statystyki
                .Include(s => s.mecz)
                .FirstOrDefaultAsync(m => m.id_meczu == id);
            if (statystyki == null)
            {
                return NotFound();
            }

            return View(statystyki);
        }

        // GET: Statystyki/Create
        public IActionResult Create()
        {
            ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu");
            return View();
        }

        // POST: Statystyki/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_meczu,gole_gospodarzy,gole_gosci,strzaly_gospodarzy,strzaly_gosci")] Statystyki statystyki)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statystyki);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", statystyki.id_meczu);
            return View(statystyki);
        }

        // GET: Statystyki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statystyki = await _context.Statystyki.FindAsync(id);
            if (statystyki == null)
            {
                return NotFound();
            }
            ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", statystyki.id_meczu);
            return View(statystyki);
        }

        // POST: Statystyki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_meczu,gole_gospodarzy,gole_gosci,strzaly_gospodarzy,strzaly_gosci")] Statystyki statystyki)
        {
            if (id != statystyki.id_meczu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statystyki);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatystykiExists(statystyki.id_meczu))
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
            ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", statystyki.id_meczu);
            return View(statystyki);
        }

        // GET: Statystyki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statystyki = await _context.Statystyki
                .Include(s => s.mecz)
                .FirstOrDefaultAsync(m => m.id_meczu == id);
            if (statystyki == null)
            {
                return NotFound();
            }

            return View(statystyki);
        }

        // POST: Statystyki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statystyki = await _context.Statystyki.FindAsync(id);
            if (statystyki != null)
            {
                _context.Statystyki.Remove(statystyki);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatystykiExists(int id)
        {
            return _context.Statystyki.Any(e => e.id_meczu == id);
        }
    }
}
