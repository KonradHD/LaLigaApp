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
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace LaLiga.Controllers
{
    public class LoginController : Controller
    {
        private readonly LaLigaContext _context;
        private readonly Service.IAuthenticationService _authService;

        public LoginController(LaLigaContext context, Service.IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string login, string password)
        {
            // Przeglądarka na której będzie odpalone dwie karty z tą aplikacją będzie posiadała tylko jedną sesję, 
            // więc dane zapisane w sesji będą odpowiadały wszystkim zalogowanym użytkownikow

            /* string HashedPassword = HashHelper.HashMD5(password);
            var uzytkownicy = _context.Uzytkownik.Where(u => u.haslo.Equals(HashedPassword) && u.email.Equals(login)).AsNoTracking();
            if (uzytkownicy.Count() > 0)
            {
                Uzytkownik uzytkownik = uzytkownicy.First();
                // ładowanie starej sesji 
                await HttpContext.Session.LoadAsync();

                // czyszczenie starej sesji 
                HttpContext.Session.Clear();

                // wymuszenie identyfikacji nowej sesji 
                await HttpContext.Session.CommitAsync();

                // Ustawienie sesji
                HttpContext.Session.SetInt32("id", uzytkownik.id);
                HttpContext.Session.SetString("Email", uzytkownik.email);
                HttpContext.Session.SetString("rola", uzytkownik.rola);

                return RedirectToAction("Core");
            }

            ViewBag.Error = "Nieprawidłowy login lub hasło";
            return View(nameof(Index)); // widok logowania z błędem */

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Podaj login i hasło.";
                return View();
            }

            var uzytkownik = await _authService.ValidateUserAsync(login, password);

            if (uzytkownik == null)
            {
                ViewBag.Error = "Nieprawidłowy login lub hasło.";
                return View();
            }
            {
                // Generate JWT token
                var jwtToken = await _authService.GenerateJwtTokenAsync(uzytkownik);
                
                // Generate refresh token
                var refreshToken = _authService.GenerateRefreshToken();
                
                // Store refresh token in database
                var refreshTokenEntity = new RefreshToken
                {
                    Token = refreshToken,
                    UserId = uzytkownik.id,
                    ExpiryDate = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                };
                
                _context.RefreshTokens.Add(refreshTokenEntity);
                await _context.SaveChangesAsync();

                // Set JWT token in cookie for web access
                Response.Cookies.Append("JWTToken", jwtToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                // Set refresh token in cookie
                Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                // Also set up cookie authentication for backward compatibility
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, uzytkownik.id.ToString()),
                    new Claim(ClaimTypes.Email, uzytkownik.email),
                    new Claim(ClaimTypes.Role, uzytkownik.rola)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Core");
            }

            ViewBag.Error = "Nieprawidłowy login lub hasło.";
            return View();
        }


        [HttpGet]
        [Authorize]
        public IActionResult Core()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Uzytkownik.FindAsync(id);
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token not found");
            }

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            var user = await _context.Uzytkownik.FindAsync(storedToken.UserId);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            // Generate new JWT token
            var newJwtToken = await _authService.GenerateJwtTokenAsync(user);

            // Generate new refresh token
            var newRefreshToken = _authService.GenerateRefreshToken();

            // Revoke old refresh token
            storedToken.IsRevoked = true;
            _context.RefreshTokens.Update(storedToken);

            // Store new refresh token
            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);
            await _context.SaveChangesAsync();

            // Set new tokens in cookies
            Response.Cookies.Append("JWTToken", newJwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { token = newJwtToken, refreshToken = newRefreshToken });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RevokeToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var storedToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
                
                if (storedToken != null)
                {
                    storedToken.IsRevoked = true;
                    _context.RefreshTokens.Update(storedToken);
                    await _context.SaveChangesAsync();
                }
            }

            Response.Cookies.Delete("JWTToken");
            Response.Cookies.Delete("RefreshToken");
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return Ok("Token revoked successfully");
        }
    }
}