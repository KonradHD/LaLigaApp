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
    public class DruzynaController : Controller
    {
        private readonly LaLigaContext _context;

        public DruzynaController(LaLigaContext context)
        {
            _context = context;
        }

        // GET: Druzyna
        public async Task<IActionResult> Index()
        {
            return View(await _context.Druzyna.ToListAsync());
        }

        // GET: Druzyna/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzyna
                .FirstOrDefaultAsync(m => m.id_druzyny == id);
            if (druzyna == null)
            {
                return NotFound();
            }

            return View(druzyna);
        }

        // GET: Druzyna/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Druzyna/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_druzyny,nazwa_druzyny,punkty,gole")] Druzyna druzyna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(druzyna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(druzyna);
        }

        // GET: Druzyna/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzyna.FindAsync(id);
            if (druzyna == null)
            {
                return NotFound();
            }
            return View(druzyna);
        }

        // POST: Druzyna/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_druzyny,nazwa_druzyny,punkty,gole")] Druzyna druzyna)
        {
            if (id != druzyna.id_druzyny)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(druzyna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DruzynaExists(druzyna.id_druzyny))
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
            return View(druzyna);
        }

        // GET: Druzyna/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var druzyna = await _context.Druzyna
                .FirstOrDefaultAsync(m => m.id_druzyny == id);
            if (druzyna == null)
            {
                return NotFound();
            }

            return View(druzyna);
        }

        // POST: Druzyna/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var druzyna = await _context.Druzyna.FindAsync(id);
            if (druzyna != null)
            {
                _context.Druzyna.Remove(druzyna);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DruzynaExists(int id)
        {
            return _context.Druzyna.Any(e => e.id_druzyny == id);
        }
    }
}
