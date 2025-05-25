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
    public class ZawodnikController : Controller
    {
        private readonly LaLigaContext _context;

        public ZawodnikController(LaLigaContext context)
        {
            _context = context;
        }

        // GET: Zawodnik
        public async Task<IActionResult> Index()
        {
            var laLigaContext = _context.Zawodnik.Include(z => z.druzyna);
            return View(await laLigaContext.ToListAsync());
        }

        // GET: Zawodnik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zawodnik = await _context.Zawodnik
                .Include(z => z.druzyna)
                .FirstOrDefaultAsync(m => m.id_druzyny == id);
            if (zawodnik == null)
            {
                return NotFound();
            }

            return View(zawodnik);
        }

        // GET: Zawodnik/Create
        public IActionResult Create()
        {
            ViewData["id_druzyny"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny");
            return View();
        }

        // POST: Zawodnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_druzyny,numer,imie,nazwisko,pozycja,wiek,wartosc_rynkowa")] Zawodnik zawodnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zawodnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_druzyny"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", zawodnik.id_druzyny);
            return View(zawodnik);
        }

        // GET: Zawodnik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zawodnik = await _context.Zawodnik.FindAsync(id);
            if (zawodnik == null)
            {
                return NotFound();
            }
            ViewData["id_druzyny"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", zawodnik.id_druzyny);
            return View(zawodnik);
        }

        // POST: Zawodnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_druzyny,numer,imie,nazwisko,pozycja,wiek,wartosc_rynkowa")] Zawodnik zawodnik)
        {
            if (id != zawodnik.id_druzyny)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zawodnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZawodnikExists(zawodnik.id_druzyny))
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
            ViewData["id_druzyny"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", zawodnik.id_druzyny);
            return View(zawodnik);
        }

        // GET: Zawodnik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zawodnik = await _context.Zawodnik
                .Include(z => z.druzyna)
                .FirstOrDefaultAsync(m => m.id_druzyny == id);
            if (zawodnik == null)
            {
                return NotFound();
            }

            return View(zawodnik);
        }

        // POST: Zawodnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zawodnik = await _context.Zawodnik.FindAsync(id);
            if (zawodnik != null)
            {
                _context.Zawodnik.Remove(zawodnik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZawodnikExists(int id)
        {
            return _context.Zawodnik.Any(e => e.id_druzyny == id);
        }
    }
}
