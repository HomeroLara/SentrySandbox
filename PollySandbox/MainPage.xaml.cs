using PollySandbox.Services;

namespace PollySandbox;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object? sender, EventArgs e)
    {
        try
        {
            var cnkTestTheatreService = ServiceHelper.GetService<ICnkTestTheatreService>();
            var theatres = await cnkTestTheatreService.SearchByGeoCoordinates(32.909946f, -96.87241f, 50f, 5);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}