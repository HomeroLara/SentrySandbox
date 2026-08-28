using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Refit;

namespace PollySandbox.Services;

public abstract class CnkServiceBase
{
    
    protected bool UseCaching { get; set; } = false;
    
    JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        TypeInfoResolverChain = { TestMpapiSourceGeneratorJsonSerializerContext.Default },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    public CnkServiceBase(bool useCaching)
    {
        UseCaching = useCaching;
    }

    private void TraceApiError(string methodName, IApiResponse apiResponse)
    {
        if (apiResponse?.Error is null)
        {
            return;
        }

        var requestUrl = apiResponse.RequestMessage?.RequestUri?.AbsoluteUri ?? string.Empty;
        var errorDetails = apiResponse.Error.Content;
        if (!string.IsNullOrWhiteSpace(errorDetails))
        {
            Console.WriteLine($"Error: Request to {requestUrl} failed with details: {errorDetails}");
        }
        else
        {
            var errorMessage = apiResponse.Error.Message;
            Console.WriteLine($"Error: Request to {requestUrl} failed with message: {errorMessage}");
        }
    }

    [DoesNotReturn]
    protected void TraceAndThrowApiError(string methodName, IApiResponse response)
    {
        TraceApiError(methodName, response);

        if (response.Error is not null)
        {
            throw response.Error;
        }

        var requestUrl = response.RequestMessage?.RequestUri?.AbsoluteUri ?? string.Empty;
        Console.WriteLine($"Error: Request to {requestUrl} failed with status code {(int)response.StatusCode} ({response.StatusCode}).");
    }

    protected void EnsureUserIsSignedIn()
    {
    }

    protected bool ShouldCacheResponse<T>(IApiResponse<T> apiResponse)
    {
        if (apiResponse is null || 
            apiResponse.Content is null || 
            apiResponse.Headers.CacheControl is null ||
            apiResponse.RequestMessage?.Method != HttpMethod.Get)
        {
            return false;
        }
        
        return apiResponse.Headers.CacheControl.Private &&
               apiResponse.Headers.CacheControl.MaxAge.HasValue &&
               apiResponse.Headers.CacheControl.MaxAge.Value > TimeSpan.Zero;
    }
    
    protected void SaveCacheResponse<T>(string cacheKey, IApiResponse<T> response)
    {
        try
        {
            if (!ShouldCacheResponse(response))
            {
                return;
            }
            
            // ShouldCacheResponse(response) will guarantee MaxAge is not null
            var maxAge = (response.Headers.CacheControl?.MaxAge!).Value;

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error: {exception}");
        }
    }
    
}