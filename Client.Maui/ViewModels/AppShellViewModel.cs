using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.Maui.ViewModels;

public partial class AppShellViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBottomBarVisible = false;

    public AppShellViewModel() { }
}
