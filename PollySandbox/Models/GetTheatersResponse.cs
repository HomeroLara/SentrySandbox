using System.Text.Json.Serialization;

namespace PollySandbox.Models;

public class GetTheatersResponse
{
    [JsonPropertyName("theaters")]
    public List<BasicTheater> Theaters { get; set; } = new();
}