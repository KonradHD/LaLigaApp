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
    public class StrzelecController : Controller
    {
        private readonly LaLigaContext _context;

        public StrzelecController(LaLigaContext context)
        {
            _context = context;
        }

        // GET: Strzelec
        public async Task<IActionResult> Index()
        {
            var laLigaContext = _context.Strzelec.Include(s => s.mecz).Include(s => s.zawodnik);
            return View(await laLigaContext.ToListAsync());
        }

        // GET: Strzelec/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strzelec = await _context.Strzelec
                .Include(s => s.mecz)
                .Include(s => s.zawodnik)
                .FirstOrDefaultAsync(m => m.id_druzyny == id);
            if (strzelec == null)
            {
                return NotFound();
            }

            return View(strzelec);
        }

        // GET: Strzelec/Create
        public IActionResult Create()
        {
            ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu");
            ViewData["id_druzyny"] = new SelectList(_context.Zawodnik, "id_druzyny", "id_druzyny");
            return View();
        }

        // POST: Strzelec/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_druzyny,numer,id_meczu,gole,asysty")] Strzelec strzelec)
        {
            if (ModelState.IsValid)
            {
                _context.Add(strzelec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", strzelec.id_meczu);
            ViewData["id_druzyny"] = new SelectList(_context.Zawodnik, "id_druzyny", "id_druzyny", strzelec.id_druzyny);
            return View(strzelec);
        }

        // GET: Strzelec/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strzelec = await _context.Strzelec.FindAsync(id);
            if (strzelec == null)
            {
                return NotFound();
            }
            ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", strzelec.id_meczu);
            ViewData["id_druzyny"] = new SelectList(_context.Zawodnik, "id_druzyny", "id_druzyny", strzelec.id_druzyny);
            return View(strzelec);
        }

        // POST: Strzelec/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_druzyny,numer,id_meczu,gole,asysty")] Strzelec strzelec)
        {
            if (id != strzelec.id_druzyny)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(strzelec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StrzelecExists(strzelec.id_druzyny))
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
            ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", strzelec.id_meczu);
            ViewData["id_druzyny"] = new SelectList(_context.Zawodnik, "id_druzyny", "id_druzyny", strzelec.id_druzyny);
            return View(strzelec);
        }

        // GET: Strzelec/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strzelec = await _context.Strzelec
                .Include(s => s.mecz)
                .Include(s => s.zawodnik)
                .FirstOrDefaultAsync(m => m.id_druzyny == id);
            if (strzelec == null)
            {
                return NotFound();
            }

            return View(strzelec);
        }

        // POST: Strzelec/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var strzelec = await _context.Strzelec.FindAsync(id);
            if (strzelec != null)
            {
                _context.Strzelec.Remove(strzelec);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrzelecExists(int id)
        {
            return _context.Strzelec.Any(e => e.id_druzyny == id);
        }
    }
}
