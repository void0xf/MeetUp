using System.Collections.ObjectModel;
using System.Diagnostics;
using Client.Maui.Api;
using Client.Maui.Api.Search;
using Client.Maui.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly ISearchApi _searchApi;
        private List<Event> _allEvents;

        public SearchViewModel(ISearchApi searchApi)
        {
            _searchApi = searchApi;
            Results = new ObservableCollection<Event>();
            _allEvents = new List<Event>();

            // Load all events when the ViewModel is created
            LoadAllEventsAsync().ConfigureAwait(false);
        }

        [ObservableProperty]
        private string searchTerm;

        [ObservableProperty]
        private ObservableCollection<Event> results;

        [ObservableProperty]
        private bool isLoading;

        private async Task LoadAllEventsAsync()
        {
            try
            {
                IsLoading = true;
                _allEvents = await _searchApi.SearchMeetAsync();
                await FilterEventsAsync();
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
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task FilterEventsAsync()
        {
            if (_allEvents == null)
                return;

            Results.Clear();
            var filteredEvents = string.IsNullOrWhiteSpace(SearchTerm)
                ? _allEvents
                : _allEvents.Where(e =>
                    e.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                    || e.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                    || e.Location.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                );

            foreach (var ev in filteredEvents)
            {
                Results.Add(ev);
            }
        }

        [RelayCommand]
        private async Task NavigateToEventDetail(Event selectedEvent)
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

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadAllEventsAsync();
        }

        partial void OnSearchTermChanged(string value)
        {
            FilterEventsAsync().ConfigureAwait(false);
        }
    }
}
