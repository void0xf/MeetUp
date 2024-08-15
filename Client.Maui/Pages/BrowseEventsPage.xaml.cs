using Client.Maui.ViewModels;

namespace Client.Maui.Pages
{
    public partial class BrowseEventsPage : ContentPage
    {
        public BrowseEventsPage(BrowseEventsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
