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
    public class LoginController : Controller
    {
        private readonly LaLigaContext _context;

        public LoginController(LaLigaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index(string? login, string? password)
        {
            string HashedPassword = HashHelper.HashMD5(password);
            var uzytkownicy = _context.Uzytkownik.Where(u => u.haslo.Equals(HashedPassword) && u.email.Equals(login)).AsNoTracking();
            if (uzytkownicy.Count() > 0)
            {
                Uzytkownik uzytkownik = uzytkownicy.First();
                // Ustawienie sesji
                HttpContext.Session.SetInt32("id", uzytkownik.id);
                HttpContext.Session.SetString("Email", uzytkownik.email);

                return RedirectToAction("Core"); // lub inna strona po zalogowaniu
            }
            ViewBag.Error = "Nieprawidłowy login lub hasło";
            return View(nameof(Index)); // widok logowania z błędem
        }


        public IActionResult Core()
        {
            if (HttpContext.Session.GetString("id") != null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Profile()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (id == null)
                return RedirectToAction("Login");

            var użytkownik = _context.Uzytkownik.Find(id.Value);
            return View(użytkownik);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}