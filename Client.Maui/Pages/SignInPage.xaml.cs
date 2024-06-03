using Client.Maui.ViewModels;

namespace Client.Maui.Pages;

public partial class SignInPage : ContentPage
{
    public SignInPage(SignInPageViewModel signInViewModel)
    {
        InitializeComponent();
        BindingContext = signInViewModel;
    }
}
