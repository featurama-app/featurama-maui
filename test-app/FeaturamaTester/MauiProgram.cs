using Featurama.Maui.DependencyInjection;
using FeaturamaTester.ViewModels;
using FeaturamaTester.Views;

namespace FeaturamaTester;

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

        // Register Featurama SDK via DI
        builder.Services.AddFeaturama(opts =>
        {
            opts.ApiKey(Config.ApiKey)
                .BaseUrl(Config.BaseUrl);
        });

        // Register ViewModels
        builder.Services.AddTransient<SettingsViewModel>();

        // Register Pages
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
