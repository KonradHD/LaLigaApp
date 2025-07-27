using System.Text.Json;
using LaLiga.Models;

public class APIManager
{
    private HttpClient client = new HttpClient();

    private HttpRequestMessage getRequest(string url)
    {
        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Headers =
                {
                    { "x-rapidapi-key", "0d6737ba6emshbace7481a631987p185cddjsn51c040c979c8" },
                    { "x-rapidapi-host", "api-football-v1.p.rapidapi.com" },
                },
        };
        return request;
    }

    public async Task createData(string APIUrl, string filePath)
    {
        if (!File.Exists(filePath))
        {
            HttpRequestMessage request = getRequest(APIUrl);
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                File.WriteAllText(filePath, body);
            }

        }
    }

    public List<Druzyna> getTeamsData(string filePath)
    {
        string json = File.ReadAllText(filePath);

        RootTeam? root = JsonSerializer.Deserialize<RootTeam>(json);

        List<Team> teams = root.response.Select(r => r.team).ToList();
        List<Venue> venues = root.response.Select(v => v.venue).ToList();
        foreach (Team team in teams)
        {
            System.Console.WriteLine(team.ToString());
        }
        foreach (Venue venue in venues)
        {
            System.Console.WriteLine("Venue: " + venue.name);
        }

        List<Druzyna> druzyny = new List<Druzyna>();
        if (teams.Count >= venues.Count)
        {
            for (int i = 0; i < venues.Count; i++)
            {
                Druzyna druzyna = new Druzyna(teams[i].id, teams[i].name, venues[i].name);
                druzyny.Add(druzyna);
            }
        }
        return druzyny;
    }

    public List<Zawodnik> getPlayersData(string filePathPlayers, string filePathInfo, int idDruzyny)
    {
        string jsonPlayers = File.ReadAllText(filePathPlayers);
        string jsonInfo = File.ReadAllText(filePathInfo);
        List<Zawodnik> zawodnicy = new List<Zawodnik>();
        System.Console.WriteLine("========================================================");

        RootPlayer? rootPlayers = JsonSerializer.Deserialize<RootPlayer>(jsonPlayers);
        RootPlayerInfo? rootInfo = JsonSerializer.Deserialize<RootPlayerInfo>(jsonInfo);

        if (rootPlayers != null && rootInfo != null)
        {
            List<Player> players = rootPlayers.response.Select(p => p.player).ToList();
            List<PlayerInfo> playersInfo = rootInfo.response.Select(p => p.player).ToList();
            foreach (var p in players)
            {
                System.Console.WriteLine("Imie: " + p.name);
            }

            foreach (var p in playersInfo)
            {
                System.Console.WriteLine("Info: " + p.firstname);
            }
            foreach (Player player in players)
            {
                PlayerInfo? playerInfo = playersInfo.Find(p => p.id == player.id);
                if (playerInfo != null)
                {
                    Zawodnik zawodnik = new Zawodnik(idDruzyny, player.number, playerInfo.firstname, playerInfo.lastname, player.position, player.age, playerInfo.nationality, playerInfo.injured);
                    zawodnicy.Add(zawodnik);
                }
                else
                {
                    Zawodnik zawodnik = new Zawodnik(idDruzyny, player.number, player.name, player.position, player.age);
                    zawodnicy.Add(zawodnik);
                }
            }
        }
        return zawodnicy;
    }
}