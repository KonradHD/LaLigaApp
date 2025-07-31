
namespace LaLiga.APIs.Match
{
    public class Team
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? logo { get; set; }
        public bool? winner { get; set; }
    }
}