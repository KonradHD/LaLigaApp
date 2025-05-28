using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaLiga.Data;
using LaLiga.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using LaLiga.Filters;

namespace LaLiga.Controllers
{
    [RequireLogin]
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
            var laLigaContext = _context.Mecz.Include(m => m.goscie).Include(m => m.gospodarze).AsNoTracking();
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

        protected void FillTeamsList(string type, object? selectedTeam = null)
        {
            var selectedTeams = from d in _context.Druzyna
                                orderby d.id_druzyny
                                select d;
            var Teams = selectedTeams.AsNoTracking();
            SelectList suitedTeams = new SelectList(Teams, "id_druzyny", "nazwa_druzyny", selectedTeam);
            if (type.Equals("goście"))
            {
                ViewBag.id_gosci = suitedTeams;
            }
            else if (type.Equals("gospodarze"))
            {
                ViewBag.id_gospodarzy = suitedTeams;
            }
        }

        // GET: Mecz/Create
        public IActionResult Create()
        {
            FillTeamsList("goście");
            FillTeamsList("gospodarze");
            return View();
        }

        // POST: Mecz/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_meczu,id_gosci,id_gospodarzy,termin")] Mecz mecz, IFormCollection form)
        {
            string goscieId = form["id_gosci"].ToString();
            string gospodarzeId = form["id_gospodarzy"].ToString();
            if (goscieId.Equals(gospodarzeId))
            {
                ModelState.AddModelError("id_gosci", "Drużyna gospodarzy i gości nie mogą być takie same.");
                FillTeamsList("goście");
                FillTeamsList("gospodarze");
                return View();
            }
            if (ModelState.IsValid)
            {
                Druzyna? druzynaGosci = null;
                var Goscie = _context.Druzyna.Where(d => d.id_druzyny == int.Parse(goscieId));
                if (Goscie.Count() > 0)
                {
                    druzynaGosci = Goscie.First();
                }
                Druzyna? druzynaGospodarzy = null;
                var Gospodarze = _context.Druzyna.Where(d => d.id_druzyny == int.Parse(gospodarzeId));
                if (Gospodarze.Count() > 0)
                {
                    druzynaGospodarzy = Gospodarze.First();
                }

                mecz.goscie = druzynaGosci;
                mecz.gospodarze = druzynaGospodarzy;
                _context.Add(mecz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var modelStateEntry in ModelState)
            {
                foreach (var error in modelStateEntry.Value.Errors)
                {
                    Console.WriteLine($"Błąd w polu '{modelStateEntry.Key}': {error.ErrorMessage}");
                }
            }
            return View(mecz);
        }

        // GET: Mecz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var mecz = _context.Mecz.Where(m => m.id_meczu == id).Include(m => m.goscie).Include(m => m.gospodarze).First();
            var mecz = await _context.Mecz.FindAsync(id);
            if (mecz == null)
            {
                return NotFound();
            }
            FillTeamsList("goście", mecz.id_gosci);
            FillTeamsList("gospodarze", mecz.id_gospodarzy);
            System.Console.WriteLine(mecz.id_gospodarzy);
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
            FillTeamsList("goście", mecz.id_gosci);
            FillTeamsList("gospodarze", mecz.id_gospodarzy);
            return View(mecz);
        }

        // GET: Mecz/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecz = _context.Mecz.Where(m => m.id_meczu == id)
                .Include(m => m.goscie)
                .Include(m => m.gospodarze)
                .First();
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
