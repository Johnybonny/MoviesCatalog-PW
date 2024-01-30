//using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Pasternak.MoviesCatalog.MAUI.ViewModels;

namespace Pasternak.MoviesCatalog.MAUI
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
            builder.Services.AddSingleton<DirectorsCollectionViewModel>();
            builder.Services.AddSingleton<DirectorsPage>();
            builder.Services.AddSingleton<MoviesCollectionViewModel>();
            builder.Services.AddSingleton<MoviesPage>();
            builder.Services.AddSingleton<BLC.BLC>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
