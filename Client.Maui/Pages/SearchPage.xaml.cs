using Client.Maui.ViewModels;

namespace Client.Maui.Pages;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel searchViewModel)
    {
        InitializeComponent();
        BindingContext = searchViewModel;
    }
}
