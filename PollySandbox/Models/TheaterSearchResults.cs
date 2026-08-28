namespace PollySandbox.Models;


public class TheaterSearchResults
{
    public TheaterSearchStatus Status { get; set; }
    public List<BasicTheater> Theaters { get; set; }
    public List<CityStateLocation> PartialMatches { get; set; }
    public bool TruncatedFlag { get; set; }

    public TheaterSearchResults()
    {
        Theaters = new List<BasicTheater>();
        PartialMatches = new List<CityStateLocation>();
    }
}


public enum TheaterSearchStatus
{
    None = 0,
    Partial,
    Complete
}

public class CityStateLocation
{
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}