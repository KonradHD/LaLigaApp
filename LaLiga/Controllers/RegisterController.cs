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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace LaLiga.Controllers
{
    public class RegisterController : Controller
    {
        private readonly LaLigaContext _context;
        private readonly PasswordHasher<Uzytkownik> _hasher = new();

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
                    wiek = user.wiek,
                    imie = user.imie,
                    nazwisko = user.nazwisko,
                    data_dolaczenia = DateTime.Now,
                    rola = user.rola
                };
                newUser.haslo = _hasher.HashPassword(newUser, user.haslo);

                var users = _context.Uzytkownik.Where(u => u.email.Equals(user.email));
                if (users.Count() > 0)
                {
                    ModelState.AddModelError("email", "Adres email już istnieje w bazie, spróbuj się zalogować.");
                    return View(user);
                }

                _context.Uzytkownik.Add(newUser);
                await _context.SaveChangesAsync();

                // Auto logowanie po rejestracji:
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.id.ToString()),
                new Claim(ClaimTypes.Email, newUser.email),
                new Claim(ClaimTypes.Role, newUser.rola)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Core", "Login");
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