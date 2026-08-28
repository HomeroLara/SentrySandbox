using System.Net;
using PollySandbox.Models;

namespace PollySandbox.Services;

public class CnkTestTheatreService : CnkServiceBase, ICnkTestTheatreService
{
    private readonly ICnkTestMobilePapi _testMobilePapi;
    
    private const string TheaterSearchByGeoCoordinatesBaseKey = "TheaterSearchByGeoCoordinates";
    
    public CnkTestTheatreService(ICnkTestMobilePapi testMobilePapi, bool useCaching) 
        : base(false)
    {
        _testMobilePapi = testMobilePapi;
    }
    
    public async Task<TheaterSearchResults> SearchByGeoCoordinates(float lat, float lon, float radius, int maxResults)
    {
        var response = await _testMobilePapi.SearchTheatersByGeoCoordinates(lat, lon, radius, maxResults);

        if (response.IsSuccessStatusCode)
        {
            return new TheaterSearchResults()
            {
                Theaters = response.Content?.Theaters ?? []
            };
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new TheaterSearchResults()
            {
                Theaters = new List<BasicTheater>()
            };
        }
        
        TraceAndThrowApiError(nameof(SearchByGeoCoordinates), response);
        
        // Required to satisfy compiler flow analysis for non-void methods; TraceAndThrowApiError always throws.
        throw new InvalidOperationException($"{nameof(SearchByGeoCoordinates)}: Unreachable code path after TraceAndThrowApiError.");
    }
}