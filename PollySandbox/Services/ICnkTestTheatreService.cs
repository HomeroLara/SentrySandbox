using PollySandbox.Models;

namespace PollySandbox.Services;

public interface ICnkTestTheatreService
{
    Task<TheaterSearchResults> SearchByGeoCoordinates(float lat, float lon, float radius, int maxResults);
}