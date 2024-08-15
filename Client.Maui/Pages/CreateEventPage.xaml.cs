using Client.Maui.Api.MeetEvent;
using Client.Maui.ViewModels;

namespace Client.Maui.Pages;

public partial class CreateEventPage : ContentPage
{
    public CreateEventPage(IMeetEventApi meetEventApi)
    {
        InitializeComponent();
        BindingContext = new CreateEventViewModel(meetEventApi);
    }
}
