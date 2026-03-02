using System.Text.Json.Serialization;

namespace Featurama.Maui.Models;

[JsonConverter(typeof(JsonStringEnumConverter<FeatureRequestSource>))]
public enum FeatureRequestSource
{
    SDK,
    Dashboard
}
