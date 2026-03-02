using Microsoft.Extensions.DependencyInjection;

namespace Featurama.Maui.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFeaturama(
        this IServiceCollection services,
        Action<FeaturamaOptionsBuilder> configure)
    {
        var builder = new FeaturamaOptionsBuilder();
        configure(builder);
        var options = builder.Build();

        services.AddSingleton(options);

        services.AddHttpClient<FeaturamaClient>((sp, httpClient) =>
        {
            var opts = sp.GetRequiredService<FeaturamaOptions>();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", opts.ApiKey);
            httpClient.Timeout = opts.Timeout;
        });

        return services;
    }
}
