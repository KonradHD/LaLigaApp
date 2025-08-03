using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LaLiga.Data;
using System.Globalization;
using LaLiga.Service;
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

// Add services to the container.
builder.Services.AddControllersWithViews();

//Dodanie obsługo sesji
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;//plik cookie jest niedostępny przez skrypt po stronie klienta
    options.Cookie.IsEssential = true;//pliki cookie sesji będą zapisywane dzięki czemu sesje będzie mogła być śledzona podczas nawigacji lub przeładowania strony
});
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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{number?}")
    .WithStaticAssets();


app.Run();
