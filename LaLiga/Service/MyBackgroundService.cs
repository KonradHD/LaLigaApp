using LaLiga.Data;
using LaLiga.Models;

namespace LaLiga.Service
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private int year = 2024;
        private APIManager ApiManager = new APIManager();

        public MyBackgroundService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string sourceFilePath = "APIs/Data/matchesData.txt";
            while (!stoppingToken.IsCancellationRequested)
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
                string destFilePath = $"APIs/Data/MatchesData{date}.txt";

                try
                {
                    // Tworzymy scope
                    using (var scope = _services.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<LaLigaContext>();

                        List<int> ids = context.Mecz.Select(m => m.id_meczu).ToList();
                        await ApiManager.createData($"https://api-football-v1.p.rapidapi.com/v3/fixtures?league=140&season={year}", destFilePath);

                        if (ApiManager.checkNewResults(sourceFilePath, destFilePath))
                        {
                            List<Mecz> mecze = ApiManager.getChangedMatches(sourceFilePath, destFilePath);

                            foreach (var mecz in mecze)
                            {
                                if (ids.Contains(mecz.id_meczu))
                                {
                                    context.Entry(mecz).Property(m => m.id_gosci).IsModified = true;
                                    context.Entry(mecz).Property(m => m.id_gospodarzy).IsModified = true;
                                    context.Entry(mecz).Property(m => m.termin).IsModified = true;
                                    context.Entry(mecz).Property(m => m.sedzia).IsModified = true;
                                }
                                else
                                {
                                    context.Add(mecz);
                                }
                            }

                            await context.SaveChangesAsync();
                            sourceFilePath = destFilePath;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Błąd w tle: {e.Message}\n{e.StackTrace}");

                }

                await Task.Delay(TimeSpan.FromHours(12));
            }
        }
    }

}