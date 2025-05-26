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
            var laLigaContext = _context.Statystyki
                        .Include(s => s.mecz)
                            .ThenInclude(m => m.goscie)
                        .Include(s => s.mecz)
                            .ThenInclude(m => m.gospodarze)
                        .AsNoTracking();
            return View(await laLigaContext.ToListAsync());
        }

        // GET: Statystyki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statystyki = _context.Statystyki.Where(m => m.id_meczu == id)
                .Include(s => s.mecz)
                    .ThenInclude(m => m.goscie)
                .Include(s => s.mecz)
                    .ThenInclude(m => m.gospodarze)
                .First();
            if (statystyki == null)
            {
                return NotFound();
            }

            return View(statystyki);
        }

        protected void FillStatsList(object? selectedMatch = null)
        {
            var selectedMatches = from m in _context.Mecz
                                  join d_gosci in _context.Druzyna on m.id_gosci equals d_gosci.id_druzyny
                                  join d_gosp in _context.Druzyna on m.id_gospodarzy equals d_gosp.id_druzyny
                                  select new
                                  {
                                      id_meczu = m.id_meczu,
                                      rywale = d_gosp.nazwa_druzyny + " vs " + d_gosci.nazwa_druzyny
                                  };
            var sel = selectedMatches.AsNoTracking();
            ViewBag.id_meczu = new SelectList(sel, "id_meczu", "rywale", selectedMatch);
        }

        // GET: Statystyki/Create
        public IActionResult Create()
        {
            FillStatsList();
            return View();
        }

        // POST: Statystyki/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_meczu,gole_gospodarzy,gole_gosci,strzaly_gospodarzy,strzaly_gosci")] Statystyki statystyki, IFormCollection form)
        {
            string meczId = form["id_meczu"].ToString();
            System.Console.WriteLine(meczId);

            if (ModelState.IsValid)
            {
                var stats = _context.Statystyki.Where(s => s.id_meczu == int.Parse(meczId));
                if (stats.Count() > 0)
                {
                    ModelState.AddModelError("id_meczu", "Statystyki tego meczu już istnieją.");
                    FillStatsList();
                    return View();
                }
                Mecz? mecz = null;
                var mecze = _context.Mecz.Where(m => m.id_meczu == int.Parse(meczId));
                if (mecze.Count() > 0)
                {
                    mecz = mecze.First();
                }
                statystyki.mecz = mecz;
                _context.Add(statystyki);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", statystyki.id_meczu);
            return View(statystyki);
        }

        // GET: Statystyki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statystyki = _context.Statystyki.Where(m => m.id_meczu == id)
                .Include(s => s.mecz)
                    .ThenInclude(m => m.goscie)
                .Include(s => s.mecz)
                    .ThenInclude(m => m.gospodarze)
                .First();
            if (statystyki == null)
            {
                return NotFound();
            }
            //FillStatsList(statystyki.id_meczu);
            return View(statystyki);
        }

        // POST: Statystyki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_meczu,gole_gospodarzy,gole_gosci,strzaly_gospodarzy,strzaly_gosci")] Statystyki statystyki, IFormCollection form)
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

            var statystyki = _context.Statystyki.Where(m => m.id_meczu == id)
                .Include(s => s.mecz)
                    .ThenInclude(m => m.goscie)
                .Include(s => s.mecz)
                    .ThenInclude(m => m.gospodarze)
                .First();
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
