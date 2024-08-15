using Client.Maui.Api.MeetEvent;
using Client.Maui.ViewModels;

namespace Client.Maui.Pages;

public partial class EventDetailPage : ContentPage
{
    public EventDetailPage(IMeetEventApi meetEventApi)
    {
        InitializeComponent();
        BindingContext = new EventDetailViewModel(meetEventApi);
    }
}
