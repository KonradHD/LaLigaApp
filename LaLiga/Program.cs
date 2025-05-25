using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LaLiga.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LaLigaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LaLigaContext") ?? throw new InvalidOperationException("Connection string 'LaLigaContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed danych 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LaLigaContext>();
        DatabaseInitializer.Initialize(context);
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
