using Featurama.Maui.UI.Strings;
using Featurama.Maui.UI.Theme;

namespace Featurama.Maui.UI;

public sealed class FeaturamaPageOptions
{
    public FeaturamaColorScheme ColorScheme { get; set; } = FeaturamaColorScheme.Light;
    public Color? AccentColor { get; set; }
    public Action? OnClose { get; set; }
    public FeaturamaStrings? Strings { get; set; }
}
