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
            CounterLabel.Text = "Fetching Theaters...";
            var cnkTestTheatreService = ServiceHelper.GetService<ICnkTestTheatreService>();
            var theatres = await cnkTestTheatreService.SearchByGeoCoordinates(32.909946f, -96.87241f, 50f, 5);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            CounterLabel.Text = "Error: " + exception.Message;
        }
    }
    
    static int counter = 0;

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        counter++;
        CounterLabel.Text = $"You clicked {counter} times";
    }
}