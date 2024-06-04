using Client.Maui.Api.Search;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.Maui.ViewModels;

[QueryProperty(nameof(Event), "Event")]
public partial class EventDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private SearchResponse.Event _event;
}
