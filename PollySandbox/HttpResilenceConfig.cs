using System.Text.Json.Serialization;
using Polly;

namespace PollySandbox;


public class HttpResilienceOptions
{
    public RetryOptions Retry { get; set; }
    
    public AttemptTimeout AttemptTimeout { get; set; }
    public int TotalRequestTimeoutSeconds { get; set; }
    public CircuitBreakerOptions CircuitBreaker { get; set; }
}

public class RetryOptions
{
    public int MaxRetryAttempts { get; set; }
    public DelayBackoffType BackoffType { get; set; }
    public bool UseJitter { get; set; }
    public int DelaySeconds { get; set; }
}

public class CircuitBreakerOptions
{
    public int SamplingDurationSeconds { get; set; }
}

public class AttemptTimeout
{
    public int TimeoutSeconds { get; set; }
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false,
    PropertyNameCaseInsensitive = true,
    GenerationMode = JsonSourceGenerationMode.Serialization | JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(HttpResilienceOptions))]
public partial class ResilienceJsonSerializerContext : JsonSerializerContext
{
}