using System.Collections;
using LaLiga.Models;
using LaLiga.Service;
using Microsoft.EntityFrameworkCore;

namespace LaLiga.Data
{
    public class DatabaseInitializer
    {
        private static APIManager apiManager = new APIManager();
        public static async Task Initialize(LaLigaContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Druzyna.Any())
            {
                await apiManager.createData("https://api-football-v1.p.rapidapi.com/v3/teams?league=140&season=2024&country=Spain", "APIs/Data/teamData.txt");
                List<Druzyna> druzyny = apiManager.getTeamsData("APIs/Data/teamData.txt");

                context.Druzyna.AddRange(druzyny);
                await context.SaveChangesAsync();
            }

            if (!context.Zawodnik.Any())
            {
                List<int> ids = await context.Druzyna.Select(d => d.id_druzyny).ToListAsync();
                foreach (int id in ids)
                {
                    try
                    {
                        Console.WriteLine($"przetwarzam zawodników druzyny {id}");
                        string playerDataPath = $"APIs/Data/playerData{id}.txt";
                        string playerInfoDataPath = $"APIs/Data/playerInfoData{id}.txt";

                        await apiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/players/squads?team={id}", playerDataPath);
                        await apiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/players?team={id}&league=140&season=2024", playerInfoDataPath);
                        List<Zawodnik> zawodnicy = apiManager.getPlayersData(playerDataPath, playerInfoDataPath, id);
                        context.Zawodnik.AddRange(zawodnicy);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[BŁĄD] Dla drużyny {id}: {ex.Message}");
                    }
                }
                await context.SaveChangesAsync();
            }

            if (!context.Uzytkownik.Any())
            {
                Uzytkownik admin = new Uzytkownik
                {
                    email = "admin@gmail.com",
                    haslo = HashHelper.HashMD5("admin"),
                    imie = "Konrad",
                    nazwisko = "Ćwięka",
                    wiek = 21,
                    data_dolaczenia = DateTime.Now,
                    rola = "admin"
                };
                context.Uzytkownik.Add(admin);
                context.SaveChangesAsync();
            }

            if (!context.Mecz.Any())
            {
                try
                {
                    await apiManager.createData("https://api-football-v1.p.rapidapi.com/v3/fixtures?league=140&season=2024", "APIs/Data/matchesData.txt");
                    List<Mecz> mecze = apiManager.getMatchesData("APIs/Data/matchesData.txt");
                    context.Mecz.AddRange(mecze);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[BŁĄD]: {e.Message}");
                }
                await context.SaveChangesAsync();
            }

            /* if (!context.Statystyki.Any())
            {
                var statystykiList = new List<Statystyki>
                {
                    new Statystyki { id_meczu = 1, gole_gospodarzy = 2, gole_gosci = 1, strzaly_gospodarzy = 12, strzaly_gosci = 6 },
                    new Statystyki { id_meczu = 2, gole_gospodarzy = 0, gole_gosci = 0, strzaly_gospodarzy = 7, strzaly_gosci = 5 },
                    new Statystyki { id_meczu = 3, gole_gospodarzy = 3, gole_gosci = 2, strzaly_gospodarzy = 15, strzaly_gosci = 11 },
                    new Statystyki { id_meczu = 4, gole_gospodarzy = 1, gole_gosci = 1, strzaly_gospodarzy = 9, strzaly_gosci = 8 },
                    new Statystyki { id_meczu = 5, gole_gospodarzy = 0, gole_gosci = 2, strzaly_gospodarzy = 4, strzaly_gosci = 13 },
                    new Statystyki { id_meczu = 6, gole_gospodarzy = 4, gole_gosci = 0, strzaly_gospodarzy = 18, strzaly_gosci = 3 },
                    new Statystyki { id_meczu = 7, gole_gospodarzy = 2, gole_gosci = 2, strzaly_gospodarzy = 10, strzaly_gosci = 10 },
                    new Statystyki { id_meczu = 8, gole_gospodarzy = 1, gole_gosci = 3, strzaly_gospodarzy = 6, strzaly_gosci = 14 },
                    new Statystyki { id_meczu = 9, gole_gospodarzy = 0, gole_gosci = 1, strzaly_gospodarzy = 5, strzaly_gosci = 8 },
                    new Statystyki { id_meczu = 10, gole_gospodarzy = 3, gole_gosci = 1, strzaly_gospodarzy = 16, strzaly_gosci = 9 },

                    new Statystyki { id_meczu = 11, gole_gospodarzy = 2, gole_gosci = 0, strzaly_gospodarzy = 11, strzaly_gosci = 4 },
                    new Statystyki { id_meczu = 12, gole_gospodarzy = 0, gole_gosci = 2, strzaly_gospodarzy = 3, strzaly_gosci = 12 },
                    new Statystyki { id_meczu = 13, gole_gospodarzy = 1, gole_gosci = 2, strzaly_gospodarzy = 7, strzaly_gosci = 11 },
                    new Statystyki { id_meczu = 14, gole_gospodarzy = 1, gole_gosci = 0, strzaly_gospodarzy = 8, strzaly_gosci = 5 },
                    new Statystyki { id_meczu = 15, gole_gospodarzy = 2, gole_gosci = 3, strzaly_gospodarzy = 10, strzaly_gosci = 15 },
                    new Statystyki { id_meczu = 16, gole_gospodarzy = 4, gole_gosci = 2, strzaly_gospodarzy = 17, strzaly_gosci = 13 },
                    new Statystyki { id_meczu = 17, gole_gospodarzy = 0, gole_gosci = 0, strzaly_gospodarzy = 6, strzaly_gosci = 6 },
                    new Statystyki { id_meczu = 18, gole_gospodarzy = 2, gole_gosci = 1, strzaly_gospodarzy = 12, strzaly_gosci = 9 },
                    new Statystyki { id_meczu = 19, gole_gospodarzy = 1, gole_gosci = 1, strzaly_gospodarzy = 8, strzaly_gosci = 8 },
                    new Statystyki { id_meczu = 20, gole_gospodarzy = 3, gole_gosci = 0, strzaly_gospodarzy = 14, strzaly_gosci = 4 },

                    new Statystyki { id_meczu = 21, gole_gospodarzy = 1, gole_gosci = 2, strzaly_gospodarzy = 9, strzaly_gosci = 13 },
                    new Statystyki { id_meczu = 22, gole_gospodarzy = 2, gole_gosci = 2, strzaly_gospodarzy = 13, strzaly_gosci = 13 },
                    new Statystyki { id_meczu = 23, gole_gospodarzy = 0, gole_gosci = 1, strzaly_gospodarzy = 4, strzaly_gosci = 7 },
                    new Statystyki { id_meczu = 24, gole_gospodarzy = 1, gole_gosci = 3, strzaly_gospodarzy = 10, strzaly_gosci = 16 },
                    new Statystyki { id_meczu = 25, gole_gospodarzy = 2, gole_gosci = 0, strzaly_gospodarzy = 11, strzaly_gosci = 5 },
                    new Statystyki { id_meczu = 26, gole_gospodarzy = 3, gole_gosci = 2, strzaly_gospodarzy = 15, strzaly_gosci = 10 },
                    new Statystyki { id_meczu = 27, gole_gospodarzy = 0, gole_gosci = 4, strzaly_gospodarzy = 5, strzaly_gosci = 17 },
                    new Statystyki { id_meczu = 28, gole_gospodarzy = 1, gole_gosci = 1, strzaly_gospodarzy = 9, strzaly_gosci = 9 },
                    new Statystyki { id_meczu = 29, gole_gospodarzy = 2, gole_gosci = 1, strzaly_gospodarzy = 13, strzaly_gosci = 6 },
                    new Statystyki { id_meczu = 30, gole_gospodarzy = 3, gole_gosci = 3, strzaly_gospodarzy = 18, strzaly_gosci = 18 },
                };

                context.Statystyki.AddRange(statystykiList);
                context.SaveChangesAsync();
            } */

            if (!context.Strzelec.Any())
            {
                /* List<Strzelec> strzelcy = new List<Strzelec>
                {
                    // Mecz 1: Alavés vs Athletic Club
                    new Strzelec { id_meczu = 1, id_druzyny = 1, numer = 3, gole = 1, asysty = 0 },
                    new Strzelec { id_meczu = 1, id_druzyny = 2, numer = 11, gole = 2, asysty = 1 },

                    // Mecz 2: Atlético Madrid vs Barcelona
                    new Strzelec { id_meczu = 2, id_druzyny = 4, numer = 8, gole = 1, asysty = 0 },
                    new Strzelec { id_meczu = 2, id_druzyny = 3, numer = 6, gole = 1, asysty = 1 },

                    // Mecz 3: Cádiz vs Celta Vigo
                    new Strzelec { id_meczu = 3, id_druzyny = 5, numer = 3, gole = 0, asysty = 1 },
                    new Strzelec { id_meczu = 3, id_druzyny = 6, numer = 4, gole = 2, asysty = 0 },

                    // Mecz 4: Getafe vs Girona
                    new Strzelec { id_meczu = 4, id_druzyny = 7, numer = 20, gole = 1, asysty = 1 },
                    new Strzelec { id_meczu = 4, id_druzyny = 8, numer = 1, gole = 0, asysty = 2 },

                    // Mecz 5: Granada vs Las Palmas
                    new Strzelec { id_meczu = 5, id_druzyny = 10, numer = 13, gole = 1, asysty = 0 },
                    new Strzelec { id_meczu = 5, id_druzyny = 9, numer = 7, gole = 2, asysty = 0 },

                    // Mecz 6: Mallorca vs Osasuna
                    new Strzelec { id_meczu = 6, id_druzyny = 11, numer = 14, gole = 1, asysty = 1 },
                    new Strzelec { id_meczu = 6, id_druzyny = 12, numer = 5, gole = 0, asysty = 1 },

                    // Mecz 7: Rayo Vallecano vs Real Betis
                    new Strzelec { id_meczu = 7, id_druzyny = 14, numer = 10, gole = 2, asysty = 0 },

                    // Mecz 8: Real Madrid vs Real Sociedad
                    new Strzelec { id_meczu = 8, id_druzyny = 15, numer = 22, gole = 1, asysty = 2 },
                    new Strzelec { id_meczu = 8, id_druzyny = 16, numer = 4, gole = 0, asysty = 1 },

                    // Mecz 9: Sevilla vs Valencia
                    new Strzelec { id_meczu = 9, id_druzyny = 17, numer = 16, gole = 1, asysty = 0 },
                    new Strzelec { id_meczu = 9, id_druzyny = 18, numer = 14, gole = 1, asysty = 1 },

                    // Mecz 10: Villarreal vs Almería
                    new Strzelec { id_meczu = 10, id_druzyny = 19, numer = 6, gole = 3, asysty = 1 },
                    new Strzelec { id_meczu = 10, id_druzyny = 20, numer = 5, gole = 1, asysty = 0 },
                };

                context.Strzelec.AddRange(strzelcy);
                context.SaveChangesAsync(); */
            }
        }
    }
}