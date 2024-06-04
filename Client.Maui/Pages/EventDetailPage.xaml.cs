using Client.Maui.ViewModels;

namespace Client.Maui.Pages;

public partial class EventDetailPage : ContentPage
{
    public EventDetailPage()
    {
        InitializeComponent();
        BindingContext = new EventDetailViewModel();
    }
}
