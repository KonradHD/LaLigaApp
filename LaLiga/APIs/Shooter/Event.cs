namespace LaLiga.APIs.Shooter
{

    public class Event
    {
        public EventTime time { get; set; }
        public EventTeam team { get; set; }
        public EventPlayer player { get; set; }
        public EventAssist assist { get; set; }
        public string type { get; set; }
        public string detail { get; set; }
        public string? comments { get; set; }
    }

    public class EventTime
    {
        public int elapsed { get; set; }
        public int? extra { get; set; }
    }

    public class EventTeam
    {
        public int id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
    }

    public class EventPlayer
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class EventAssist
    {
        public int? id { get; set; }
        public string? name { get; set; }
    }
}