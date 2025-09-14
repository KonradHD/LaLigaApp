using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LaLiga.Models;
using LaLiga.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LaLiga.Service
{
    public interface IAuthenticationService
    {
        Task<string> GenerateJwtTokenAsync(Uzytkownik user);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token);
        Task<Uzytkownik?> ValidateUserAsync(string email, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly LaLigaContext _context;
        private readonly PasswordHasher<Uzytkownik> _hasher;
        private readonly IConfiguration _configuration;

        public AuthenticationService(LaLigaContext context, IConfiguration configuration)
        {
            _context = context;
            _hasher = new PasswordHasher<Uzytkownik>();
            _configuration = configuration;
        }

        public async Task<string> GenerateJwtTokenAsync(Uzytkownik user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234567890"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, user.rola),
                new Claim(ClaimTypes.Name, $"{user.imie} {user.nazwisko}"),
                new Claim("UserId", user.id.ToString()),
                new Claim("UserAge", user.wiek.ToString()),
                new Claim("JoinDate", user.data_dolaczenia.ToString("yyyy-MM-dd"))
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "LaLigaApp",
                audience: _configuration["Jwt:Audience"] ?? "LaLigaAppUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Short-lived token
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere12345678901234567890");

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"] ?? "LaLigaApp",
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"] ?? "LaLigaAppUsers",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Uzytkownik?> ValidateUserAsync(string email, string password)
        {
            var user = await _context.Uzytkownik
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.email == email);

            if (user == null)
                return null;

            var result = _hasher.VerifyHashedPassword(user, user.haslo, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}

