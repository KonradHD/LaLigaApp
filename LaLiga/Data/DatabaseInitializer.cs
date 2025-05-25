using System.Collections;
using LaLiga.Models;
using Microsoft.EntityFrameworkCore;

namespace LaLiga.Data
{
    public class DatabaseInitializer
    {
        public static void Initialize(LaLigaContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Druzyna.Any())
            {
                List<Druzyna> druzyny = new List<Druzyna>
                {
                    new Druzyna { nazwa_druzyny = "Alavés" },
                    new Druzyna { nazwa_druzyny = "Athletic Club" },
                    new Druzyna { nazwa_druzyny = "Atlético Madrid" },
                    new Druzyna { nazwa_druzyny = "Barcelona" },
                    new Druzyna { nazwa_druzyny = "Cádiz" },
                    new Druzyna { nazwa_druzyny = "Celta Vigo" },
                    new Druzyna { nazwa_druzyny = "Getafe" },
                    new Druzyna { nazwa_druzyny = "Girona" },
                    new Druzyna { nazwa_druzyny = "Granada" },
                    new Druzyna { nazwa_druzyny = "Las Palmas" },
                    new Druzyna { nazwa_druzyny = "Mallorca" },
                    new Druzyna { nazwa_druzyny = "Osasuna" },
                    new Druzyna { nazwa_druzyny = "Rayo Vallecano" },
                    new Druzyna { nazwa_druzyny = "Real Betis" },
                    new Druzyna { nazwa_druzyny = "Real Madrid" },
                    new Druzyna { nazwa_druzyny = "Real Sociedad" },
                    new Druzyna { nazwa_druzyny = "Sevilla" },
                    new Druzyna { nazwa_druzyny = "Valencia" },
                    new Druzyna { nazwa_druzyny = "Villarreal" },
                    new Druzyna { nazwa_druzyny = "Almería" }
                };

                context.Druzyna.AddRange(druzyny);
                context.SaveChangesAsync();
            }

            if (!context.Zawodnik.Any())
            {
                List<Zawodnik> zawodnicy = new List<Zawodnik>
                {
                    new Zawodnik{}
                };
            }
        }
    }
}