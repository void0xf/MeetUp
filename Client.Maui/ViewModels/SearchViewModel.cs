using System.Collections.ObjectModel;
using System.Net;
using Client.Maui.Api.Search;
using Client.Maui.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly ISearchApi _searchApi;

        public SearchViewModel(ISearchApi searchApi)
        {
            _searchApi = searchApi;
            Results = new ObservableCollection<SearchResponse.Event>();
            SearchEventsAsync();
        }

        [ObservableProperty]
        private string searchTerm;

        [ObservableProperty]
        private ObservableCollection<SearchResponse.Event> results;

        [ObservableProperty]
        SearchResponse.Event selectedEvent;

        [RelayCommand]
        private async Task SearchEventsAsync()
        {
            var apiResponse = await _searchApi.SearchAsync(SearchTerm, 100);

            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                Results.Clear();
                foreach (var result in apiResponse.Content.Results)
                {
                    Results.Add(result);
                }
            }
        }

        [RelayCommand]
        private async Task SelectEventAsync()
        {
            if (SelectedEvent != null)
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Event", SelectedEvent }
                };
                await Shell.Current.GoToAsync(nameof(EventDetailPage), navigationParameter);
            }
        }

        partial void OnSearchTermChanged(string value)
        {
            SearchEventsAsync();
        }
    }
}
