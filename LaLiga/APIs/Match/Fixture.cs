
namespace LaLiga.APIs.Match
{
    public class Fixture
    {
        public int id { get; set; }
        public string referee { get; set; }
        public string timezone { get; set; }
        public string date { get; set; }
        public long timestamp { get; set; }
        public Periods periods { get; set; }
        public Venue venue { get; set; }
        public Status status { get; set; }
    }

    public class Periods
    {
        public long first { get; set; }
        public long second { get; set; }
    }

    public class Venue
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
    }

    public class Status
    {
        public string @long { get; set; }
        public string @short { get; set; }
        public int elapsed { get; set; }
    }

}