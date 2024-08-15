using Client.Maui.Api;
using Client.Maui.Api.MeetEvent;
using Client.Maui.Store;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels;

[QueryProperty(nameof(Event), "Event")]
public partial class EventDetailViewModel : ObservableObject
{
    private readonly IMeetEventApi _meetEventApi;
    private readonly UserStore _userStore;

    public EventDetailViewModel(IMeetEventApi meetEventApi)
    {
        _meetEventApi = meetEventApi;
        _userStore = App.Current.Handler.MauiContext.Services.GetService<UserStore>();
    }

    [ObservableProperty]
    private Event _event;

    [RelayCommand]
    async void JoinEvent()
    {
        var res = await _meetEventApi.JoinToEvent(Event.Id.ToString(), _userStore.AccessToken);
        if (res.StatusCode == System.Net.HttpStatusCode.OK) { }
    }
}
