using Client.Maui.Store;
using Microsoft.Extensions.Logging;

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
            /*builder
                .Services.AddRefitClient<IAuth>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5000"));*/
            builder.Services.AddSingleton<UserStore>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
