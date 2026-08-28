using System;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using PollySandbox.Services;
using Refit;
using Sentry;
using Polly;
using PollySandbox.Services;

namespace PollySandbox;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            
            .UseSentry(options =>
            {
                options.Dsn = "[YOUR DNS ENTRY HERE]";
                options.StackTraceMode = StackTraceMode.Enhanced;
                options.IsGlobalModeEnabled = true;
             
#if IOS
                //var appInfo = new iOSAppInfo();
                options.Release = $"iOS 1.0";
                // options.Native.EnableSwizzling = false;
                
                // options.Native.EnableNetworkBreadcrumbs = false;
                // options.Native.EnableNetworkTracking = false;
                // options.Native.EnableTracing = false;
#endif
                
#if ANDROID
                var appInfo = new DroidAppInfo();
                options.Release = $"Android {appInfo.AppVersion}";
                options.Android.SuppressSegfaults = true; 
#endif
                
#if DEBUG
                options.Environment = "Debug";
                
                // Use debug mode if you want to see what the SDK is doing.
                // Debug messages are written to stdout with Console.Writeline,
                // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                // options.Debug = true;
#endif
                
#if RELEASE_TEST
                options.Environment = "Release-Test";
#endif
                
#if RELEASE_UAT
                options.Environment = "Release-UAT";
#endif                
                
#if RELEASE
                options.Environment = "Production";
                options.SampleRate = .25f;
#endif

            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        RegisterRefitClients(builder);
        RegisterServices(builder);
        return builder.Build();
    }

    private static void RegisterServices(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ICnkTestTheatreService, CnkTestTheatreService>(serviceProvider =>
        {
            var mobilePapi = serviceProvider.GetRequiredService<ICnkTestMobilePapi>();
            var cnkTheaterService = new CnkTestTheatreService(mobilePapi, true);
            return cnkTheaterService;
        });
    }

    private static void RegisterRefitClients(MauiAppBuilder builder)
    {
        builder.Services
            .AddRefitClient<ICnkTestMobilePapi>(_ =>
            {
                var typeInfoResolver = TestMpapiSourceGeneratorJsonSerializerContext.Default;
                var options = CnkApiHelpers.GetCnkStandardJsonSerializerSettings(typeInfoResolver);

                return new RefitSettings()
                {
                    ContentSerializer = new SystemTextJsonContentSerializer(options)
                };
            })
            .ConfigureHttpClient(httpClient =>
            {
                
                httpClient.BaseAddress = new Uri("https://cnk.wiremockapi.cloud/");
                CnkApiHelpers.AddCnkStandardHeaders(httpClient.DefaultRequestHeaders);
            })
            .ConfigurePrimaryHttpMessageHandler(_ =>
            {
                var platformHttpMessageHandler = CnkApiHelpers.GetPlatformHttpMessageHandler();
                return platformHttpMessageHandler;
            })
            .AddStandardResilienceHandler(options =>
            {

                options.Retry.MaxRetryAttempts = 3;
                options.Retry.BackoffType = 0;
                options.Retry.UseJitter = true;
                options.Retry.Delay = TimeSpan.FromSeconds(0);
            
                // Per-attempt timeout
                options.AttemptTimeout.Timeout = new TimeSpan(0, 0, 30); //TimeSpan.FromSeconds(resilienceConfig.AttemptTimeout.TimeoutSeconds);
            
                // Overall timeout budget
                options.TotalRequestTimeout = new HttpTimeoutStrategyOptions()
                {
                    Timeout = TimeSpan.FromSeconds(30)
                };
            
                // Adjust circuit breaker so sampling >= 2 * attempt timeout (>= 60s)
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(70); // or 60+
                // (Optional) tune other CB settings if desired:
                // options.CircuitBreaker.MinimumThroughput = 10;
                // options.CircuitBreaker.FailureRatio = 0.2;
            
                // If you prefer to disable instead:
                // options.CircuitBreaker.Enabled = false;
            });
    }
}