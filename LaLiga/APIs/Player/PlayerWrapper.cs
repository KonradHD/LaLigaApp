public class PlayerWrapper
{
    public TeamNon team { get; set; }
    public List<Player> players { get; set; }
}

public class TeamNon
{
    public int id { get; set; }
    public string name { get; set; }
    public string logo { get; set; }
}