using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LaLiga.Service;

namespace LaLiga.Controllers
{
    [Authorize] // Requires authentication for all actions
    public class ExampleSecureController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Get user information from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userAge = User.FindFirstValue("UserAge");

            ViewBag.UserInfo = new
            {
                Id = userId,
                Email = userEmail,
                Role = userRole,
                Name = userName,
                Age = userAge
            };

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Only Admin role can access
        public IActionResult AdminOnly()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.ModeratorOrAdmin)] // Using custom policy
        public IActionResult ModeratorOrAdmin()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.UserOrHigher)] // Using custom policy
        public IActionResult UserOrHigher()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.MinimumAge)] // Age-based policy
        public IActionResult AdultsOnly()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "MatchManagement")] // Custom policy for match management
        public IActionResult ManageMatches()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "UserManagement")] // Custom policy for user management
        public IActionResult ManageUsers()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous] // This action doesn't require authentication
        public IActionResult PublicInfo()
        {
            return View();
        }

        // Example of conditional authorization based on user data
        [HttpGet]
        [Authorize]
        public IActionResult ConditionalAccess()
        {
            var userAge = int.Parse(User.FindFirstValue("UserAge") ?? "0");
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userAge < 18)
            {
                return RedirectToAction("AccessDenied", "Login");
            }

            if (userRole != "Admin" && userRole != "Moderator")
            {
                return RedirectToAction("AccessDenied", "Login");
            }

            return View();
        }
    }
}

