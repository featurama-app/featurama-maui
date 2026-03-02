namespace Featurama.Maui.Models;

public sealed class FeatureRequest
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public FeatureRequestStatus Status { get; set; }
    public FeatureRequestSource Source { get; set; }
    public int VoteCount { get; set; }
    public string? SubmitterIdentifier { get; set; }
    public DateTime CreatedAt { get; set; }
}
