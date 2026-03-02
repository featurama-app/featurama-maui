using System.Text.Json.Serialization;

namespace Featurama.Maui.Models;

[JsonConverter(typeof(JsonStringEnumConverter<FeatureRequestStatus>))]
public enum FeatureRequestStatus
{
    Requested,
    Roadmap,
    InProgress,
    Done,
    Declined
}
