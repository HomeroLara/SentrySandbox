using Microsoft.Extensions.Options;
using PollySandbox.Services;
using Sentry.Maui;

namespace PollySandbox;

public partial class MainPage : ContentPage
{
    public MainPage(IOptions<SentryMauiOptions> sentryOptions)
    {
        InitializeComponent();
        
#if IOS
        var isSwizzlingEnabled = sentryOptions.Value.Native.EnableSwizzling;
        SwizzlingLabel.Text = isSwizzlingEnabled ? "Yes" : "No";
#endif
        
#if DEBUG
            BuildLabel.Text = "Debug Mode";
#endif
#if RELEASE
        BuildLabel.Text = "Release Mode";
#endif
        
        DeviceModelLabel.Text = DeviceInfo.Model;
        OSVersionLabel.Text = DeviceInfo.VersionString;
    }

    private async void OnCounterClicked(object? sender, EventArgs e)
    {
        try
        {
            CounterLabel.Text = "Fetching Theaters...";
            var cnkTestTheatreService = ServiceHelper.GetService<ICnkTestTheatreService>();
            var theatres = await cnkTestTheatreService.SearchByGeoCoordinates(32.909946f, -96.87241f, 50f, 5);
            CounterLabel.Text = $"Found {theatres.Theaters.Count} theaters.";
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            CounterLabel.Text = "Error: " + exception.Message;
        }
    }
}