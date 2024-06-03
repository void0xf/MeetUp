using Client.Maui.Pages;

namespace Client.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
        }
    }
}
