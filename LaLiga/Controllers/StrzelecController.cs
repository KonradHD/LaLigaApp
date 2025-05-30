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

namespace LaLiga.Controllers
{
    [RequireLogin]
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
            var laLigaContext = _context.Strzelec
                .Include(s => s.mecz)
                    .ThenInclude(m => m.goscie)
                .Include(s => s.mecz)
                    .ThenInclude(m => m.gospodarze)
                .Include(s => s.zawodnik);
            return View(await laLigaContext.ToListAsync());
        }

        // GET: Strzelec/Details/5
        [HttpGet("Strzelec/Details/{id_druzyny}/{numer}/{id_meczu}")]
        public async Task<IActionResult> Details(int? id_druzyny, int? numer, int? id_meczu)
        {
            if (id_druzyny == null || id_meczu == null || numer == null)
            {
                return NotFound();
            }

            var strzelec = _context.Strzelec.Where(s => s.id_druzyny == id_druzyny && s.numer == numer && s.id_meczu == id_meczu)
            .Include(s => s.mecz)
                .ThenInclude(m => m.goscie)
            .Include(s => s.mecz)
                .ThenInclude(m => m.gospodarze)
            .Include(s => s.zawodnik).First();
            if (strzelec == null)
            {
                return NotFound();
            }

            return View(strzelec);
        }

        protected void FillMatchList(object? selectedMatch = null)
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

        protected void FillPlayerList(object? selectedPlayerId = null, object? selectedPlayerNumber = null)
        {
            var selectedPlayers = from z in _context.Zawodnik
                                  join d in _context.Druzyna on z.id_druzyny equals d.id_druzyny
                                  select new
                                  {
                                      id_druzyny = z.id_druzyny,
                                      numer = z.numer,
                                      nazwa_druzyny = d.nazwa_druzyny
                                  };
            var Players = selectedPlayers.AsNoTracking();
            ViewBag.id_druzyny = new SelectList(Players, "id_druzyny", "nazwa_druzyny", selectedPlayerId);
            ViewBag.numer = new SelectList(Players, "numer", "numer", selectedPlayerNumber);
        }

        // GET: Strzelec/Create
        public IActionResult Create()
        {
            FillMatchList();
            FillPlayerList();
            return View();
        }

        // POST: Strzelec/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_druzyny,numer,id_meczu,gole,asysty")] Strzelec strzelec, IFormCollection form)
        {
            string druzynaId = form["id_druzyny"].ToString();
            string meczId = form["id_meczu"].ToString();
            string numer = form["numer"].ToString();

            if (ModelState.IsValid)
            {
                Zawodnik? zawodnik = null;
                var zawodnicy = _context.Zawodnik.Where(z => z.id_druzyny == int.Parse(druzynaId) && z.numer == int.Parse(numer));
                if (zawodnicy.Count() > 0)
                {
                    zawodnik = zawodnicy.First();
                }

                Mecz? mecz = null;
                var mecze = _context.Mecz.Where(m => m.id_meczu == int.Parse(meczId));
                if (mecze.Count() > 0)
                {
                    mecz = mecze.First();
                }

                if (zawodnik.id_druzyny != mecz.id_gosci && zawodnik.id_druzyny != mecz.id_gospodarzy)
                {
                    ModelState.AddModelError("id_druzyny", "Zawodnik musi należeć do drużyny, która bierze udział w meczu.");
                    FillMatchList();
                    FillPlayerList();
                    return View();
                }

                strzelec.mecz = mecz;
                strzelec.zawodnik = zawodnik;
                _context.Add(strzelec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            FillMatchList(strzelec.id_meczu);
            FillPlayerList(strzelec.id_druzyny, strzelec.numer);
            //ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", strzelec.id_meczu);
            //ViewData["id_druzyny"] = new SelectList(_context.Zawodnik, "id_druzyny", "id_druzyny", strzelec.id_druzyny);
            return View(strzelec);
        }

        // GET: Strzelec/Edit/5
        [HttpGet("Strzelec/Edit/{id_druzyny}/{numer}/{id_meczu}")]
        public async Task<IActionResult> Edit(int? id_druzyny, int? numer, int? id_meczu)
        {
            if (id_druzyny == null || id_meczu == null || numer == null)
            {
                return NotFound();
            }

            var strzelec = _context.Strzelec.Where(s => s.id_druzyny == id_druzyny && s.numer == numer && s.id_meczu == id_meczu)
            .Include(s => s.mecz)
                .ThenInclude(m => m.goscie)
            .Include(s => s.mecz)
                .ThenInclude(m => m.gospodarze)
            .Include(s => s.zawodnik).First();
            if (strzelec == null)
            {
                return NotFound();
            }
            FillMatchList(strzelec.id_meczu);
            FillPlayerList(strzelec.id_druzyny, strzelec.numer);
            //ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", strzelec.id_meczu);
            //ViewData["id_druzyny"] = new SelectList(_context.Zawodnik, "id_druzyny", "id_druzyny", strzelec.id_druzyny);
            return View(strzelec);
        }

        // POST: Strzelec/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Strzelec/Edit/{id_druzyny:int}/{numer:int}/{id_meczu:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id_druzyny, int numer, int id_meczu, [Bind("id_druzyny,numer,id_meczu,gole,asysty")] Strzelec strzelec)
        {
            if (id_druzyny != strzelec.id_druzyny || id_meczu != strzelec.id_meczu || numer != strzelec.numer)
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
                    if (!StrzelecExists(strzelec.id_druzyny, strzelec.numer, strzelec.id_meczu))
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
            FillMatchList(strzelec.id_meczu);
            FillPlayerList(strzelec.id_druzyny, strzelec.numer);
            //ViewData["id_meczu"] = new SelectList(_context.Mecz, "id_meczu", "id_meczu", strzelec.id_meczu);
            //ViewData["id_druzyny"] = new SelectList(_context.Zawodnik, "id_druzyny", "id_druzyny", strzelec.id_druzyny);
            return View(strzelec);
        }

        // GET: Strzelec/Delete/5
        [HttpGet("Strzelec/Delete/{id_druzyny}/{numer}/{id_meczu}")]
        public async Task<IActionResult> Delete(int? id_druzyny, int? numer, int? id_meczu)
        {
            if (id_druzyny == null || id_meczu == null || numer == null)
            {
                return NotFound();
            }

            var strzelec = _context.Strzelec.Where(s => s.id_druzyny == id_druzyny && s.numer == numer && s.id_meczu == id_meczu)
            .Include(s => s.mecz)
                .ThenInclude(m => m.goscie)
            .Include(s => s.mecz)
                .ThenInclude(m => m.gospodarze)
            .Include(s => s.zawodnik).First();
            if (strzelec == null)
            {
                return NotFound();
            }

            return View(strzelec);
        }

        // POST: Strzelec/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id_druzyny, int numer, int id_meczu)
        {
            var strzelec = await _context.Strzelec.FirstOrDefaultAsync(s => s.id_druzyny == id_druzyny && s.numer == numer && s.id_meczu == id_meczu);
            if (strzelec != null)
            {
                _context.Strzelec.Remove(strzelec);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> TopShooters()
        {
            var Shooters = _context.Strzelec
            .OrderByDescending(s => s.gole)
            .ThenByDescending(s => s.asysty)
            .Include(s => s.zawodnik)
                .ThenInclude(z => z.druzyna)
            .AsNoTracking();
            return View(await Shooters.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> TopShooters(int? wiekPonizej, string? pozycja, string? nazwa_druzyny)
        {
            ViewBag.WiekPonizej = wiekPonizej;
            ViewBag.Pozycja = pozycja;
            ViewBag.NazwaDruzyny = nazwa_druzyny;

            var Shooters = _context.Strzelec
                            .Include(s => s.zawodnik)
                                .ThenInclude(z => z.druzyna)
                            .AsQueryable();

            if (wiekPonizej != null)
            {
                Shooters = Shooters.Where(s => s.zawodnik.wiek <= wiekPonizej);
            }
            if (!string.IsNullOrEmpty(pozycja))
            {
                Shooters = Shooters.Where(s => s.zawodnik.pozycja == pozycja);
            }
            if (!string.IsNullOrEmpty(nazwa_druzyny))
            {
                Shooters = Shooters.Where(s => s.zawodnik.druzyna.nazwa_druzyny == nazwa_druzyny);
            }

            Shooters = Shooters
                .OrderByDescending(s => s.gole)
                .ThenByDescending(s => s.asysty)
                .AsNoTracking();
            return View(await Shooters.ToListAsync());
        }

        private bool StrzelecExists(int id_druzyny, int numer, int id_meczu)
        {
            return _context.Strzelec.Any(s => s.id_druzyny == id_druzyny && s.numer == numer && s.id_meczu == id_meczu);
        }
    }
}
