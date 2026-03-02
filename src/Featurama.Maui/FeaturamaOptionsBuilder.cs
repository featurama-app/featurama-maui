namespace Featurama.Maui;

public sealed class FeaturamaOptionsBuilder
{
    private readonly FeaturamaOptions _options = new();

    public FeaturamaOptionsBuilder ApiKey(string apiKey)
    {
        _options.ApiKey = apiKey;
        return this;
    }

    public FeaturamaOptionsBuilder BaseUrl(string baseUrl)
    {
        _options.BaseUrl = baseUrl.TrimEnd('/');
        return this;
    }

    public FeaturamaOptionsBuilder Timeout(TimeSpan timeout)
    {
        _options.Timeout = timeout;
        return this;
    }

    internal FeaturamaOptions Build()
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            throw new ArgumentException("API key must not be empty.");

        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            throw new ArgumentException(
                "Base URL must not be empty. Set it to your Convex deployment URL, e.g. \"https://your-deployment.convex.site\".");

        return _options;
    }
}
