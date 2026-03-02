using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Web;
using Featurama.Maui.Exceptions;
using Featurama.Maui.Models;
using Featurama.Maui.Serialization;

namespace Featurama.Maui;

public sealed class FeaturamaClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public FeaturamaClient(HttpClient httpClient, FeaturamaOptions options)
    {
        _httpClient = httpClient;
        _baseUrl = options.BaseUrl.TrimEnd('/');

        _httpClient.DefaultRequestHeaders.Remove("X-Api-Key");
        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", options.ApiKey);
        _httpClient.Timeout = options.Timeout;
    }

    public async Task<PaginatedResponse<FeatureRequest>> GetFeatureRequestsAsync(
        int page = 1,
        int pageSize = 20,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl}/api/public/requests?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(filter))
            url += $"&filter={HttpUtility.UrlEncode(filter)}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await SendAsync(request, FeaturamaJsonContext.Default.PaginatedResponseFeatureRequest, cancellationToken);
    }

    public async Task<FeatureRequest> CreateFeatureRequestAsync(
        string title,
        string? description = null,
        string? submitterIdentifier = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl}/api/public/requests";
        var body = new CreateFeatureRequestInput
        {
            Title = title,
            Description = description,
            SubmitterIdentifier = submitterIdentifier
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(body, FeaturamaJsonContext.Default.CreateFeatureRequestInput)
        };
        return await SendAsync(request, FeaturamaJsonContext.Default.FeatureRequest, cancellationToken);
    }

    public async Task<FeatureRequest> UpdateFeatureRequestAsync(
        Guid id,
        string title,
        string? description = null,
        string submitterIdentifier = "",
        CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl}/api/public/requests/{id}?submitterIdentifier={HttpUtility.UrlEncode(submitterIdentifier)}";
        var body = new UpdateFeatureRequestInput
        {
            Title = title,
            Description = description
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(body, FeaturamaJsonContext.Default.UpdateFeatureRequestInput)
        };
        return await SendAsync(request, FeaturamaJsonContext.Default.FeatureRequest, cancellationToken);
    }

    public async Task<FeatureRequest> VoteAsync(
        Guid featureRequestId,
        string voterIdentifier,
        CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl}/api/public/requests/{featureRequestId}/vote";
        var body = new VoteRequestBody { VoterIdentifier = voterIdentifier };

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(body, FeaturamaJsonContext.Default.VoteRequestBody)
        };
        return await SendAsync(request, FeaturamaJsonContext.Default.FeatureRequest, cancellationToken);
    }

    public async Task<FeatureRequest> RemoveVoteAsync(
        Guid featureRequestId,
        string voterIdentifier,
        CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl}/api/public/requests/{featureRequestId}/vote";
        var body = new VoteRequestBody { VoterIdentifier = voterIdentifier };

        using var request = new HttpRequestMessage(HttpMethod.Delete, url)
        {
            Content = JsonContent.Create(body, FeaturamaJsonContext.Default.VoteRequestBody)
        };
        return await SendAsync(request, FeaturamaJsonContext.Default.FeatureRequest, cancellationToken);
    }

    public async Task<FeatureRequest> ToggleVoteAsync(
        Guid featureRequestId,
        string voterIdentifier,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await VoteAsync(featureRequestId, voterIdentifier, cancellationToken);
        }
        catch (FeaturamaConflictException)
        {
            return await RemoveVoteAsync(featureRequestId, voterIdentifier, cancellationToken);
        }
    }

    public async Task<ProjectConfig> GetProjectConfigAsync(
        CancellationToken cancellationToken = default)
    {
        var url = $"{_baseUrl}/api/public/config";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await SendAsync(request, FeaturamaJsonContext.Default.ProjectConfig, cancellationToken);
    }

    private async Task<T> SendAsync<T>(HttpRequestMessage request, JsonTypeInfo<T> typeInfo, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            throw new FeaturamaNetworkException("Request timed out.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new FeaturamaNetworkException($"Network error: {ex.Message}", ex);
        }

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw (int)response.StatusCode switch
            {
                401 => new FeaturamaUnauthorizedException(responseBody),
                404 => new FeaturamaNotFoundException(responseBody),
                409 => new FeaturamaConflictException(responseBody),
                _ => new FeaturamaApiException((int)response.StatusCode,
                    $"API error: {(int)response.StatusCode}", responseBody)
            };
        }

        return JsonSerializer.Deserialize(responseBody, typeInfo)
               ?? throw new FeaturamaException("Failed to deserialize response.");
    }
}
