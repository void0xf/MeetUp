using Client.Maui.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels
{
    public partial class BrowseEventsViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task NavigateToSearchPageAsync()
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
        }
    }
}
