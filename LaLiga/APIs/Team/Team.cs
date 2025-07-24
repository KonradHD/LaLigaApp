public class Team
{
    public int id { get; set; }
    public string name { get; set; }
    public string code { get; set; }
    public string country { get; set; }
    public int founded { get; set; }
    public bool national { get; set; }
    public string logo { get; set; }

    public override string ToString()
    {
        return "Nazwa: " + name;
    }
}