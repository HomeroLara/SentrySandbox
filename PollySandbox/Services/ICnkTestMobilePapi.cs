using System.Text.Json.Serialization;
using PollySandbox.Models;
using Refit;

namespace PollySandbox.Services;

/// <summary>
/// Test-only Refit contract used by test scenarios.
/// Do not use this interface for production mobile API calls.
/// </summary>
public interface ICnkTestMobilePapi
{
    [Get("/theater/SearchByGeoCoordinates/{lat}/{lon}/{radius}/{maxResults}")]
    Task<ApiResponse<GetTheatersResponse>> SearchTheatersByGeoCoordinates(
        float lat, float lon, float radius, int maxResults);
}

// This is a System.Text.Json source generator context for the types used in the ICnkTestMobilePapi interface.
// It improves serialization performance by generating code at compile time.
// It also allows Android to avoid Reflection-based serialization, which is not supported for AOT compiled apps.
// IMPORTANT!!!!!
// When adding calls to the ICnkTestMobilePapi interface, ensure the types used in the method signatures are added here.
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false,
    PropertyNameCaseInsensitive = true,
    GenerationMode = JsonSourceGenerationMode.Serialization | JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(GetTheatersResponse))]
[JsonSerializable(typeof(List<DateTime>))]
public partial class TestMpapiSourceGeneratorJsonSerializerContext : JsonSerializerContext
{
}