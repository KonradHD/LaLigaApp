using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaLiga.Data;
using LaLiga.Models;
using LaLiga.Filters;
using AspNetCoreGeneratedDocument;

namespace LaLiga.Controllers
{
    [RequireLogin]
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
        [HttpGet("Zawodnik/Details/{id_druzyny}/{numer}")]
        public async Task<IActionResult> Details(int? id_druzyny, int? numer)
        {
            if (id_druzyny == null || numer == null)
            {
                return NotFound();
            }

            var zawodnik = await _context.Zawodnik
                .Include(z => z.druzyna)
                .FirstOrDefaultAsync(m => m.id_druzyny == id_druzyny && m.numer == numer);
            if (zawodnik == null)
            {
                return NotFound();
            }
            return View(zawodnik);
        }

        protected void FillPlayerList(object? selectedTeam = null)
        {
            var Teams = from d in _context.Druzyna
                        select d;
            Teams = Teams.AsNoTracking();
            ViewBag.id_druzyny = new SelectList(Teams, "id_druzyny", "nazwa_druzyny", selectedTeam);
        }

        // GET: Zawodnik/Create
        public IActionResult Create()
        {
            FillPlayerList();
            //ViewData["id_druzyny"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny");
            return View();
        }

        // POST: Zawodnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_druzyny,numer,imie,nazwisko,pozycja,wiek,wartosc_rynkowa")] Zawodnik zawodnik, IFormCollection form)
        {
            string druzynaId = form["id_druzyny"].ToString();
            if (ModelState.IsValid)
            {
                Druzyna? druzyna = null;
                var druzyny = _context.Druzyna.Where(d => d.id_druzyny == int.Parse(druzynaId));
                if (druzyny.Count() > 0)
                {
                    druzyna = druzyny.First();
                }
                zawodnik.druzyna = druzyna;
                _context.Add(zawodnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillPlayerList(zawodnik.id_druzyny);
            //ViewData["id_druzyny"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", zawodnik.id_druzyny);
            return View(zawodnik);
        }

        // GET: Zawodnik/Edit/5
        [HttpGet("Zawodnik/Edit/{id_druzyny}/{numer}")]
        public async Task<IActionResult> Edit(int? id_druzyny, int? numer)
        {
            if (id_druzyny == null || numer == null)
            {
                return NotFound();
            }

            var zawodnik = _context.Zawodnik.Where(z => z.id_druzyny == id_druzyny && z.numer == numer).Include(z => z.druzyna).First();
            if (zawodnik == null)
            {
                return NotFound();
            }
            FillPlayerList(zawodnik.id_druzyny);
            //ViewData["id_druzyny"] = new SelectList(_context.Druzyna, "id_druzyny", "id_druzyny", zawodnik.id_druzyny);
            return View(zawodnik);
        }

        // POST: Zawodnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id_druzyny, int numer, [Bind("id_druzyny,numer,imie,nazwisko,pozycja,wiek,wartosc_rynkowa")] Zawodnik zawodnik)
        {
            if (id_druzyny != zawodnik.id_druzyny || numer != zawodnik.numer)
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
                    if (!ZawodnikExists(zawodnik.id_druzyny, zawodnik.numer))
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
            FillPlayerList(zawodnik.id_druzyny);
            return View(zawodnik);
        }

        // GET: Zawodnik/Delete/5
        [HttpGet("Zawodnik/Delete/{id_druzyny}/{numer}")]
        public async Task<IActionResult> Delete(int? id_druzyny, int? numer)
        {
            if (id_druzyny == null || numer == null)
            {
                return NotFound();
            }

            var zawodnik = await _context.Zawodnik
                .Include(z => z.druzyna)
                .FirstOrDefaultAsync(m => m.id_druzyny == id_druzyny && m.numer == numer);
            if (zawodnik == null)
            {
                return NotFound();
            }

            return View(zawodnik);
        }

        // POST: Zawodnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id_druzyny, int numer)
        {
            var zawodnik = await _context.Zawodnik.FirstOrDefaultAsync(m => m.id_druzyny == id_druzyny && m.numer == numer);
            if (zawodnik != null)
            {
                _context.Zawodnik.Remove(zawodnik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> TopPrice()
        {
            var mostValuablePlayers = _context.Zawodnik.OrderByDescending(z => (double)z.wartosc_rynkowa).Include(z => z.druzyna).AsNoTracking();
            return View(await mostValuablePlayers.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> TopPrice(string? nazwa_druzyny, int? wiekPowyzej, int? wiekPonizej)
        {
            ViewBag.NazwaDruzyny = nazwa_druzyny;
            ViewBag.WiekPowyzej = wiekPowyzej;
            ViewBag.wiekPonizej = wiekPonizej;

            var playersList = _context.Zawodnik
            .Include(z => z.druzyna)
            .AsQueryable();

            if (!string.IsNullOrEmpty(nazwa_druzyny))
            {
                playersList = playersList.Where(z => z.druzyna.nazwa_druzyny.Equals(nazwa_druzyny));
            }

            if (wiekPowyzej != null)
            {
                playersList = playersList.Where(z => z.wiek >= wiekPowyzej);
            }

            if (wiekPonizej != null)
            {
                playersList.Where(z => z.wiek <= wiekPonizej);
            }

            playersList = playersList.OrderByDescending(z => (double)z.wartosc_rynkowa).AsNoTracking();
            return View(await playersList.ToListAsync());
        }


        private bool ZawodnikExists(int id, int numer)
        {
            return _context.Zawodnik.Any(e => e.id_druzyny == id && numer == e.numer);
        }
    }
}
