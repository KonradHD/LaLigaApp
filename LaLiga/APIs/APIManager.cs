using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using LaLiga.APIs.Match;
using LaLiga.APIs.Shooter;
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

        RootPlayerInfo? rootInfo = JsonSerializer.Deserialize<RootPlayerInfo>(jsonInfo);
        RootPlayer? rootPlayers = JsonSerializer.Deserialize<RootPlayer>(jsonPlayers);

        if (rootPlayers != null && rootInfo != null)
        {
            List<Player> players = rootPlayers.response[0].players;
            List<PlayerInfo> playersInfo = rootInfo.response.Select(p => p.player).ToList();
            foreach (Player player in players)
            {
                if (player.number != null && zawodnicy.FirstOrDefault(z => z.numer == player.number) == null)
                {
                    PlayerInfo? playerInfo = playersInfo.FirstOrDefault(p => p.id == player.id);
                    if (playerInfo != null)
                    {
                        Zawodnik zawodnik = new Zawodnik(idDruzyny, player.number ?? 0, playerInfo.firstname, playerInfo.lastname, player.position,
                                                        player.age, playerInfo.nationality, playerInfo.injured, player.id);
                        zawodnicy.Add(zawodnik);
                    }
                    else
                    {
                        Zawodnik zawodnik = new Zawodnik(idDruzyny, player.number ?? 0, player.name, player.position, player.age, player.id);
                        zawodnicy.Add(zawodnik);
                    }
                }
            }
        }
        return zawodnicy;
    }

    public List<Mecz> getMatchesData(string filePath)
    {
        string jsonMatch = File.ReadAllText(filePath);

        Root? root = JsonSerializer.Deserialize<Root>(jsonMatch);
        List<Mecz> mecze = new List<Mecz>();

        if (root != null)
        {
            List<Fixture> fixtures = root.response.Select(f => f.fixture).ToList();
            List<LaLiga.APIs.Match.Team> home = root.response.Select(t => t.teams.home).ToList();
            List<LaLiga.APIs.Match.Team> away = root.response.Select(t => t.teams.away).ToList();
            if (fixtures.Count() >= home.Count())
            {
                for (int i = 0; i < fixtures.Count(); i++)
                {
                    string dateStr = fixtures[i].date.Substring(0, fixtures[i].date.IndexOf("T"));
                    System.Console.WriteLine(dateStr);
                    DateTime date;
                    if (DateTime.TryParse(dateStr, out date))
                    {
                        Mecz mecz = new Mecz(fixtures[i].id, home[i].id, away[i].id, date, fixtures[i].referee);
                        mecze.Add(mecz);
                    }
                }
            }
        }
        return mecze;
    }

    public List<Strzelec> getShootersData(string filePath, int IdMeczu)
    {
        string json = File.ReadAllText(filePath);
        List<Strzelec> strzelcy = new List<Strzelec>();

        ShooterRoot? root = JsonSerializer.Deserialize<ShooterRoot>(json);
        if (root != null)
        {
            List<Event> events = root.response[0].events;
            foreach (var ev in events)
            {
                Strzelec? oldStrzelec = strzelcy.FirstOrDefault(s => s.id_meczu == IdMeczu && s.APIid == ev.player.id);
                if (ev.detail.Equals("Own Goal"))
                {
                    if (oldStrzelec == null)
                    {
                        Strzelec strzelec = new Strzelec(ev.team.id, IdMeczu, ev.player.id);
                        strzelec.AddOwnGoal();
                        strzelcy.Add(strzelec);
                    }
                    else
                    {
                        oldStrzelec.AddOwnGoal();
                    }
                }
                else if (ev.detail.Equals("Normal Goal") || (ev.type.Equals("Goal") && ev.detail.Equals("Penalty")))
                {
                    if (oldStrzelec == null)
                    {
                        Strzelec strzelec = new Strzelec(ev.team.id, IdMeczu, ev.player.id);
                        strzelec.AddGoal();
                        strzelcy.Add(strzelec);
                    }
                    else
                    {
                        oldStrzelec.AddGoal();
                    }
                    if (ev.assist.id != null)
                    {
                        Strzelec? oldAssister = strzelcy.FirstOrDefault(s => s.id_meczu == IdMeczu && s.APIid == ev.assist.id);
                        if (oldAssister == null)
                        {
                            Strzelec assister = new Strzelec(ev.team.id, IdMeczu, ev.assist.id ?? 0);
                            assister.AddAssist();
                            strzelcy.Add(assister);
                        }
                        else
                        {
                            oldAssister.AddAssist();
                        }
                    }
                }
            }
        }
        return strzelcy;
    }
}