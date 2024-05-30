using Client.Maui.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [RelayCommand]
        async void NaviageToSignUp()
        {
            await Shell.Current.GoToAsync(nameof(SignUpPage));
        }
    }
}
