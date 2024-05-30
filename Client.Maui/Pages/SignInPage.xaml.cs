using Client.Maui.ViewModels;

namespace Client.Maui.Pages;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
        BindingContext = new SignInPageViewModel();
    }
}
