using System.Collections;
using LaLiga.Models;
using LaLiga.Service;
using Microsoft.EntityFrameworkCore;

namespace LaLiga.Data
{
    public class DatabaseInitializer
    {
        private static APIManager apiManager = new APIManager();
        private static int year = 2024;

        public static async Task Initialize(LaLigaContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Druzyna.Any())
            {
                await apiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/teams?league=140&season={year}&country=Spain", "APIs/Data/teamData.txt");
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
                        await apiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/players?team={id}&league=140&season={year}", playerInfoDataPath);
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
                await context.SaveChangesAsync();
            }

            if (!context.Mecz.Any())
            {
                try
                {
                    await apiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/fixtures?league=140&season={year}", "APIs/Data/matchesData.txt");
                    List<Mecz> mecze = apiManager.getMatchesData("APIs/Data/matchesData.txt");
                    context.Mecz.AddRange(mecze);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[BŁĄD]: {e.Message}");
                }
                await context.SaveChangesAsync();
            }

            Dictionary<int, int> matchHomeId = await context.Mecz.Select(m => new { m.id_meczu, m.id_gospodarzy })
                                                                 .OrderBy(m => m.id_meczu)
                                                                 //.Skip(10)
                                                                 .Take(10)
                                                                 .ToDictionaryAsync(m => m.id_meczu, m => m.id_gospodarzy);
            if (!context.Strzelec.Any())
            {

                Dictionary<int, int> playersNumber = await context.Zawodnik.Select(z => new { z.APIid, z.numer }).ToDictionaryAsync(z => z.APIid, z => z.numer);
                int biggestNumber = playersNumber.Values.Max();
                foreach (int id in matchHomeId.Keys)
                {
                    try
                    {
                        Console.WriteLine($"przetwarzam strzelców meczu {id}");
                        string shooterDataPath = $"APIs/Data/shooterData{id}.txt";

                        await apiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/fixtures?id={id}", shooterDataPath);
                        List<Strzelec> strzelcy = apiManager.getShootersData(shooterDataPath, id);
                        foreach (Strzelec strzelec in strzelcy)
                        {
                            int number;
                            if (playersNumber.TryGetValue(strzelec.APIid, out number))
                            {
                                strzelec.SetNumber(number);
                            }
                            else
                            {
                                number = biggestNumber;

                                string onePlayerDataPath = $"APIs/Data/onePlayerData{strzelec.APIid}.txt";
                                await apiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/players?id={strzelec.APIid}&season={year}", onePlayerDataPath);
                                Zawodnik zawodnik = apiManager.getOnePlayerData(onePlayerDataPath, strzelec.id_druzyny, number, strzelec.APIid);
                                context.Zawodnik.Add(zawodnik);
                                await context.SaveChangesAsync();

                                strzelec.SetNumber(zawodnik.numer);
                                biggestNumber++;
                            }
                            System.Console.WriteLine($"{strzelec.APIid}: {strzelec.numer}");
                        }
                        context.Strzelec.AddRange(strzelcy);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[BŁĄD] Dla meczu {id}: {ex.Message}");
                    }
                }
                await context.SaveChangesAsync();
            }


            foreach (var entry in matchHomeId)
            {
                string statisticsPath = $"APIs/Data/statsData{entry.Key}.txt";
                try
                {
                    await apiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/fixtures/statistics?fixture={entry.Key}", statisticsPath);
                    Statystyki stat = apiManager.getStatisticsData(statisticsPath, entry.Key, entry.Value);
                    Statystyki? statDb = context.Statystyki.FirstOrDefault(s => s.id_meczu == stat.id_meczu);
                    if (statDb != null && !stat.Equals(statDb))
                    {
                        context.Entry(stat).Property(s => s.strzaly_gospodarzy).IsModified = true;
                        context.Entry(stat).Property(s => s.strzaly_gosci).IsModified = true;
                        context.Entry(stat).Property(s => s.posiadanie_pilki_gospodarzy).IsModified = true;
                        context.Entry(stat).Property(s => s.posiadanie_pilki_gosci).IsModified = true;
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"BLAD PRZY STATYSTYKACH Z MECZU {entry.Key}: {e.Message}");
                }
            }
            await context.SaveChangesAsync();
        }
    }
}