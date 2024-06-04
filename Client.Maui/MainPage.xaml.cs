using Client.Maui.ViewModels;

namespace Client.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(viewModel);
        }
    }
}
