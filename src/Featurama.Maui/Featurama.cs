using Featurama.Maui.Models;

namespace Featurama.Maui;

public static class Featurama
{
    private static FeaturamaClient? _client;

    public static bool IsInitialized => _client is not null;

    public static void Init(Action<FeaturamaOptionsBuilder> configure)
    {
        var builder = new FeaturamaOptionsBuilder();
        configure(builder);
        var options = builder.Build();

        _client = new FeaturamaClient(new HttpClient(), options);
    }

    public static void Init(FeaturamaOptions options)
    {
        _client = new FeaturamaClient(new HttpClient(), options);
    }

    private static FeaturamaClient Client =>
        _client ?? throw new InvalidOperationException(
            "Featurama SDK has not been initialized. Call Featurama.Init() first.");

    public static Task<PaginatedResponse<FeatureRequest>> GetFeatureRequestsAsync(
        int page = 1, int pageSize = 20, string? filter = null,
        CancellationToken cancellationToken = default)
        => Client.GetFeatureRequestsAsync(page, pageSize, filter, cancellationToken);

    public static Task<FeatureRequest> CreateFeatureRequestAsync(
        string title, string? description = null, string? submitterIdentifier = null,
        CancellationToken cancellationToken = default)
        => Client.CreateFeatureRequestAsync(title, description, submitterIdentifier, cancellationToken);

    public static Task<FeatureRequest> UpdateFeatureRequestAsync(
        Guid id, string title, string? description = null, string submitterIdentifier = "",
        CancellationToken cancellationToken = default)
        => Client.UpdateFeatureRequestAsync(id, title, description, submitterIdentifier, cancellationToken);

    public static Task<FeatureRequest> VoteAsync(
        Guid featureRequestId, string voterIdentifier,
        CancellationToken cancellationToken = default)
        => Client.VoteAsync(featureRequestId, voterIdentifier, cancellationToken);

    public static Task<FeatureRequest> RemoveVoteAsync(
        Guid featureRequestId, string voterIdentifier,
        CancellationToken cancellationToken = default)
        => Client.RemoveVoteAsync(featureRequestId, voterIdentifier, cancellationToken);

    public static Task<FeatureRequest> ToggleVoteAsync(
        Guid featureRequestId, string voterIdentifier,
        CancellationToken cancellationToken = default)
        => Client.ToggleVoteAsync(featureRequestId, voterIdentifier, cancellationToken);

    public static Task<ProjectConfig> GetProjectConfigAsync(
        CancellationToken cancellationToken = default)
        => Client.GetProjectConfigAsync(cancellationToken);

    internal static void Reset() => _client = null;
}
