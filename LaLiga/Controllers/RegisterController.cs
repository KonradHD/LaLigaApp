using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaLiga.Data;
using LaLiga.Models;
using LaLiga.Service;

namespace LaLiga.Controllers
{
    public class RegisterController : Controller
    {
        private readonly LaLigaContext _context;

        public RegisterController(LaLigaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("id", "email", "haslo", "wiek", "imie", "nazwisko", "data_dolaczenia", "rola")] Uzytkownik user)
        {
            if (ModelState.IsValid)
            {
                Uzytkownik newUser = new Uzytkownik
                {
                    email = user.email,
                    haslo = HashHelper.HashMD5(user.haslo),
                    wiek = user.wiek,
                    imie = user.imie,
                    nazwisko = user.nazwisko,
                    data_dolaczenia = DateTime.Now,
                    rola = "user"
                };

                var users = _context.Uzytkownik.Where(u => u.email.Equals(user.email));
                if (users.Count() > 0)
                {
                    ModelState.AddModelError("email", "Adres email już istnieje w bazie, spróbuj się zalogować.");
                    return View(user);
                }

                _context.Uzytkownik.Add(newUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Login");
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            user.data_dolaczenia = DateTime.Now;
            user.rola = "user";
            return View(user);
        }
    }
}