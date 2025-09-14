using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LaLiga.Data;
using System.Globalization;
using LaLiga.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LaLiga.Middleware;
var builder = WebApplication.CreateBuilder(args);


// Dodaj logowanie do konsoli i ustaw poziom
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddHostedService<MyBackgroundService>();

builder.Host.ConfigureHostOptions(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(3); // czas zamykania aplikacja gdy działa BackgroundService
});


builder.Services.AddDbContext<LaLigaContext>(options =>
    options
        .UseSqlite(builder.Configuration.GetConnectionString("LaLigaContext") ?? throw new InvalidOperationException("Connection string 'LaLigaContext' not found."))
        .EnableSensitiveDataLogging());

// Configure JWT Settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "YourSuperSecretKeyHere12345678901234567890");

// Add Authentication Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "LaLigaApp",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "LaLigaAppUsers",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
})
.AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/Login/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configure Authorization with custom policies
builder.Services.AddAuthorization(options =>
{
    AuthorizationPolicies.ConfigureAuthorizationPolicies(options);
});
// Add services to the container.
builder.Services.AddControllersWithViews();


//Dodanie obsługo sesji
//builder.Services.AddDistributedMemoryCache();

// builder.Services.AddSession(options =>
// {
//     options.IdleTimeout = TimeSpan.FromMinutes(10);
//     options.Cookie.HttpOnly = true;//plik cookie jest niedostępny przez skrypt po stronie klienta
//     options.Cookie.IsEssential = true;//pliki cookie sesji będą zapisywane dzięki czemu sesje będzie mogła być śledzona podczas nawigacji lub przeładowania strony
// });
//KONIEC

var app = builder.Build();

var cultureInfo = new CultureInfo("pl-PL");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Seed danych 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LaLigaContext>();
        await DatabaseInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Błąd podczas seedowania danych: " + ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Add security headers middleware
app.UseSecurityHeaders();

app.UseAuthentication(); // <- UWAGA: musi być przed Authorization
app.UseAuthorization();

app.MapStaticAssets();
//app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{number?}")
    .WithStaticAssets();


app.Run();
