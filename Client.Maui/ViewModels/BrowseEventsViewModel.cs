using System.Collections.ObjectModel;
using System.Diagnostics;
using Client.Maui.Api;
using Client.Maui.Api.MeetEvent;
using Client.Maui.Pages;
using Client.Maui.Store;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels
{
    public partial class BrowseEventsViewModel : ObservableObject
    {
        private readonly IMeetEventApi _meetEventApi;
        private readonly UserStore _userStore;

        [ObservableProperty]
        private ObservableCollection<Event> results;

        [ObservableProperty]
        private bool isRefreshing;

        public BrowseEventsViewModel(IMeetEventApi meetEventApi)
        {
            _userStore = App.Current.Handler.MauiContext.Services.GetService<UserStore>();
            _meetEventApi = meetEventApi;
            results = new ObservableCollection<Event>();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await LoadMyEventsAsync();
        }

        [RelayCommand]
        public async Task NavigateToSearchPageAsync()
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
        }

        [RelayCommand]
        public async Task NavigateToCreateEventPageAsync()
        {
            await Shell.Current.GoToAsync(nameof(CreateEventPage));
        }

        [RelayCommand]
        public async Task LoadMyEventsAsync()
        {
            if (IsRefreshing)
                return;

            try
            {
                IsRefreshing = true;
                var events = await _meetEventApi.GetMyEvents($"Bearer {_userStore.AccessToken}");
                Results.Clear();
                foreach (var e in events)
                {
                    Results.Add(e);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading events: {ex.Message}");
                await Shell.Current.DisplayAlert(
                    "Error",
                    "Unable to load events. Please try again later.",
                    "OK"
                );
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        public async Task NavigateToEventDetailAsync(Event selectedEvent)
        {
            if (selectedEvent != null)
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Event", selectedEvent }
                };
                await Shell.Current.GoToAsync(nameof(EventDetailPage), navigationParameter);
            }
        }
    }
}
