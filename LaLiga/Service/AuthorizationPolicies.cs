using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LaLiga.Service
{
    public static class AuthorizationPolicies
    {
        public const string AdminOnly = "AdminOnly";
        public const string ModeratorOrAdmin = "ModeratorOrAdmin";
        public const string UserOrHigher = "UserOrHigher";
        public const string MinimumAge = "MinimumAge";

        public static void ConfigureAuthorizationPolicies(AuthorizationOptions options)
        {
            // Admin only policy
            options.AddPolicy(AdminOnly, policy =>
                policy.RequireRole("Admin"));

            // Moderator or Admin policy
            options.AddPolicy(ModeratorOrAdmin, policy =>
                policy.RequireRole("Moderator", "Admin"));

            // User or higher policy (User, Moderator, Admin)
            options.AddPolicy(UserOrHigher, policy =>
                policy.RequireRole("User", "Moderator", "Admin"));

            // Minimum age policy (18+)
            options.AddPolicy(MinimumAge, policy =>
                policy.RequireAssertion(context =>
                {
                    var ageClaim = context.User.FindFirst("UserAge");
                    if (ageClaim == null || !int.TryParse(ageClaim.Value, out int age))
                        return false;
                    
                    return age >= 18;
                }));

            // Custom policy for match management
            options.AddPolicy("MatchManagement", policy =>
                policy.RequireAssertion(context =>
                {
                    var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
                    return role == "Admin" || role == "Moderator";
                }));

            // Custom policy for user management
            options.AddPolicy("UserManagement", policy =>
                policy.RequireRole("Admin"));
        }
    }
}

