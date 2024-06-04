using Client.Maui.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels;

public partial class BrowseEventsViewModel : ObservableObject
{
    [RelayCommand]
    async void NavigateToSearchPage()
    {
        await Shell.Current.GoToAsync(nameof(SearchPage));
    }
}
