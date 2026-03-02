namespace Featurama.Maui.UI.Theme;

public sealed class FeaturamaTheme
{
    public Color Background { get; init; } = Colors.White;
    public Color Card { get; init; } = Colors.White;
    public Color Secondary { get; init; } = Colors.White;
    public Color Text { get; init; } = Colors.Black;
    public Color TextSecondary { get; init; } = Colors.Gray;
    public Color Accent { get; init; } = Color.FromArgb("#007AFF");
    public Color AccentLight { get; init; } = Colors.LightBlue;
    public Color AccentForeground { get; init; } = Colors.White;
    public Color Border { get; init; } = Colors.LightGray;
    public Color BorderAccent { get; init; } = Color.FromArgb("#007AFF");
    public Color Gray100 { get; init; } = Colors.LightGray;
}
