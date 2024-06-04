using Client.Maui.ViewModels;

namespace Client.Maui.Pages
{
    public partial class BrowseEventsPage : ContentPage
    {
        private readonly BrowseEventsViewModel _viewModel;

        public BrowseEventsPage(BrowseEventsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }
    }
}
