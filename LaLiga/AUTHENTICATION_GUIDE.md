# LaLiga App - Authentication & Authorization Guide

## Overview

This application implements a comprehensive authentication and authorization system using both JWT tokens and cookie-based authentication for maximum security and flexibility.

## Features

### 🔐 Authentication Features
- **JWT Token Authentication**: Stateless authentication with short-lived tokens
- **Refresh Token System**: Secure token refresh mechanism
- **Cookie Authentication**: Traditional session-based authentication
- **Password Hashing**: Secure password storage using ASP.NET Core Identity hasher
- **Security Headers**: Protection against common web vulnerabilities

### 🛡️ Authorization Features
- **Role-Based Access Control (RBAC)**: Admin, Moderator, User roles
- **Custom Authorization Policies**: Flexible permission system
- **Age-Based Authorization**: Content restrictions based on user age
- **Resource-Based Authorization**: Fine-grained access control

## Configuration

### JWT Settings (appsettings.json)
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyHere12345678901234567890",
    "Issuer": "LaLigaApp",
    "Audience": "LaLigaAppUsers",
    "ExpiryInMinutes": 30,
    "RefreshTokenExpiryInDays": 7
  }
}
```

### Security Headers
The application automatically adds security headers:
- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `X-XSS-Protection: 1; mode=block`
- `Content-Security-Policy`
- `Referrer-Policy: strict-origin-when-cross-origin`

## Usage Examples

### 1. Basic Authentication

```csharp
[Authorize] // Requires any authenticated user
public class SecureController : Controller
{
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        return View();
    }
}
```

### 2. Role-Based Authorization

```csharp
[Authorize(Roles = "Admin")] // Only Admin role
public IActionResult AdminOnly()
{
    return View();
}

[Authorize(Roles = "Admin,Moderator")] // Admin or Moderator
public IActionResult ModeratorOrAdmin()
{
    return View();
}
```

### 3. Custom Policy Authorization

```csharp
[Authorize(Policy = AuthorizationPolicies.ModeratorOrAdmin)]
public IActionResult ManageContent()
{
    return View();
}

[Authorize(Policy = AuthorizationPolicies.MinimumAge)]
public IActionResult AdultsOnly()
{
    return View();
}
```

### 4. Conditional Authorization

```csharp
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
```

### 5. Public Access

```csharp
[AllowAnonymous] // No authentication required
public IActionResult PublicInfo()
{
    return View();
}
```

## Available Authorization Policies

### Built-in Policies
- `AdminOnly`: Only Admin role
- `ModeratorOrAdmin`: Admin or Moderator roles
- `UserOrHigher`: User, Moderator, or Admin roles
- `MinimumAge`: Users 18 years or older

### Custom Policies
- `MatchManagement`: Admin or Moderator roles
- `UserManagement`: Admin role only

## Token Management

### Login Process
1. User submits credentials
2. System validates credentials
3. JWT token generated (30 minutes expiry)
4. Refresh token generated (7 days expiry)
5. Both tokens stored in secure HTTP-only cookies
6. User redirected to protected area

### Token Refresh
```csharp
// Automatic refresh via AJAX
$.post('/Login/RefreshToken', function(data) {
    // Tokens automatically updated in cookies
    console.log('Tokens refreshed');
});
```

### Logout Process
```csharp
// Revoke tokens and clear cookies
$.post('/Login/RevokeToken', function(data) {
    window.location.href = '/Login';
});
```

## Security Best Practices

### 1. Password Security
- Passwords are hashed using ASP.NET Core Identity hasher
- Salt is automatically generated and stored
- No plain text passwords in database

### 2. Token Security
- JWT tokens are short-lived (30 minutes)
- Refresh tokens are long-lived but revocable
- Tokens stored in HTTP-only, secure cookies
- SameSite=Strict prevents CSRF attacks

### 3. Session Security
- Sliding expiration for cookie authentication
- Secure cookie policy in production
- Automatic session cleanup

### 4. Database Security
- Refresh tokens stored with expiry dates
- Automatic cleanup of expired tokens
- Token revocation capability

## API Endpoints

### Authentication Endpoints
- `POST /Login` - User login
- `POST /Login/RefreshToken` - Refresh JWT token
- `POST /Login/RevokeToken` - Revoke tokens and logout
- `GET /Login/AccessDenied` - Access denied page

### User Information
Access user information from claims:
```csharp
var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
var userEmail = User.FindFirstValue(ClaimTypes.Email);
var userRole = User.FindFirstValue(ClaimTypes.Role);
var userName = User.FindFirstValue(ClaimTypes.Name);
var userAge = User.FindFirstValue("UserAge");
var joinDate = User.FindFirstValue("JoinDate");
```

## Database Schema

### RefreshToken Table
```sql
CREATE TABLE RefreshTokens (
    Id INTEGER PRIMARY KEY,
    Token TEXT NOT NULL,
    ExpiryDate TEXT NOT NULL,
    UserId INTEGER NOT NULL,
    IsRevoked INTEGER NOT NULL DEFAULT 0,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Uzytkownik(id)
);
```

## Migration Required

After implementing this system, you'll need to create a migration for the RefreshToken table:

```bash
dotnet ef migrations add AddRefreshTokens
dotnet ef database update
```

## Testing

### Test User Roles
- **Admin**: Full access to all features
- **Moderator**: Can manage matches and content
- **User**: Basic access to view data

### Test Age Restrictions
- Users under 18: Limited access to certain features
- Users 18+: Full access based on role

## Troubleshooting

### Common Issues
1. **Token Expired**: Automatically refresh via `/Login/RefreshToken`
2. **Access Denied**: Check user role and age requirements
3. **Database Errors**: Ensure RefreshToken table exists
4. **Cookie Issues**: Check HTTPS configuration in production

### Debug Information
Enable detailed logging in `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore.Authentication": "Debug"
    }
  }
}
```

## Production Considerations

1. **Change JWT Key**: Use a strong, unique key in production
2. **HTTPS Only**: Ensure all cookies are secure
3. **Rate Limiting**: Implement rate limiting for login attempts
4. **Monitoring**: Monitor failed authentication attempts
5. **Backup**: Regular backup of user data and tokens

