using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CarPlay;
using Microsoft.Extensions.Logging;

namespace PollySandbox;

public static class CnkApiHelpers
{
    public static JsonSerializerOptions GetCnkStandardJsonSerializerSettings(
        IJsonTypeInfoResolver jsonTypeInfoResolver = null)
    {
        JsonSerializerOptions options;
        if (jsonTypeInfoResolver is not null)
        {
            return new JsonSerializerOptions()
            {
                TypeInfoResolver = JsonTypeInfoResolver.Combine(jsonTypeInfoResolver),
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                Converters = { new TimeZoneInfoJsonConverter() }
            };
        }
        
        return new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            Converters = { new TimeZoneInfoJsonConverter() }
        };
    }

    public static void AddCnkStandardHeaders(HttpRequestHeaders headers)
    {
        headers.Add("Accept", "application/json");
        headers.Add("User-Agent", "CnkApp");

        headers.Add("APP_VERSION", "1.0");
        headers.Add("APP_PLATFORM","Apple");
        headers.Add("DEVICE_ID", Guid.NewGuid().ToString());
        headers.Add("OS_VERSION", "14.0");
        
    }

#if ANDROID
    public static HttpMessageHandler GetPlatformHttpMessageHandler()
    {
        return new Xamarin.Android.Net.AndroidMessageHandler();
    }
#elif IOS
    public static HttpMessageHandler GetPlatformHttpMessageHandler()
    {
        return new NSUrlSessionHandler();
    }
#endif
}