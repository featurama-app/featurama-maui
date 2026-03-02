namespace Featurama.Maui.Models;

public sealed class UpdateFeatureRequestInput
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}
