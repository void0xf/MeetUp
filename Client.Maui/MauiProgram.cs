using Client.Maui.Api.Auth;
using Client.Maui.Api.Search;
using Client.Maui.Api.Users;
using Client.Maui.Pages;
using Client.Maui.Store;
using Client.Maui.ViewModels;
using Microsoft.Extensions.Logging;
using Refit;

namespace Client.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<UserStore>();

            builder
                .Services.AddRefitClient<IAuth>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://10.0.2.2:6001"));

            builder
                .Services.AddRefitClient<IUserApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://10.0.2.2:6001"));
            builder
                .Services.AddRefitClient<ISearchApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://10.0.2.2:6001"));

            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<SignInPageViewModel>();
            builder.Services.AddTransient<SearchViewModel>();
            builder.Services.AddTransient<BrowseEventsViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddTransient<BrowseEventsPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<SignInPage>();
            builder.Services.AddTransient<SearchPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
