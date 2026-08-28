namespace PollySandbox.Services;

public static class ServiceHelper
{
    // Private field to hold a custom service provider, primarily for testing purposes
    private static IServiceProvider? _customServiceProvider;

    /// <summary>
    /// Gets an instance of the specified service type.
    /// </summary>
    /// <typeparam name="TService">The type of service to retrieve.</typeparam>
    /// <returns>An instance of the requested service.</returns>
    public static TService GetService<TService>() => (_customServiceProvider ?? Current).GetService<TService>();

    /// <summary>
    /// Gets the current platform-specific IServiceProvider.
    /// </summary>
    public static IServiceProvider Current =>
#if ANDROID
        IPlatformApplication.Current.Services;
#elif IOS || MACCATALYST
        IPlatformApplication.Current.Services;
#else
		null;
#endif
        
    /// <summary>
    /// Sets a custom service provider for testing purposes.
    /// </summary>
    /// <param name="serviceProvider">The custom service provider to set.</param>
    public static void SetCustomServiceProvider(IServiceProvider serviceProvider)
    {
        _customServiceProvider = serviceProvider;
    }
        
    /// <summary>
    /// Commented this out to prevent issues with the unit tests
    /// since this class and it's properties are static, setting _customServiceProvider = null
    /// will affect all unit tests that are being executed.
    /// </summary>
    // public static void ClearCustomServiceProvider()
    // {
    //     // _customServiceProvider = null;
    // }
}