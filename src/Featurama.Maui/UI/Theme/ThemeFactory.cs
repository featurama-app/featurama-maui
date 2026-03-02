namespace Featurama.Maui.UI.Theme;

internal static class ThemeFactory
{
    public static FeaturamaTheme Create(Color accentColor, FeaturamaColorScheme colorScheme)
    {
        var (h, s, _) = ColorToHsl(accentColor);
        var isDark = colorScheme == FeaturamaColorScheme.Dark;

        var accentLight = isDark
            ? HslToColor(h, Math.Min(s, 30), 20)
            : HslToColor(h, Math.Min(s, 40), 92);

        var accentForeground = RelativeLuminance(accentColor) > 0.4 ? Colors.Black : Colors.White;

        if (isDark)
        {
            return new FeaturamaTheme
            {
                Background = Color.FromArgb("#000000"),
                Card = Color.FromArgb("#1C1C1E"),
                Secondary = Color.FromArgb("#2C2C2E"),
                Text = Colors.White,
                TextSecondary = Color.FromArgb("#8E8E93"),
                Accent = accentColor,
                AccentLight = accentLight,
                AccentForeground = accentForeground,
                Border = Color.FromArgb("#38383A"),
                BorderAccent = accentColor,
                Gray100 = Color.FromArgb("#1C1C1E"),
            };
        }

        return new FeaturamaTheme
        {
            Background = Color.FromArgb("#F2F2F7"),
            Card = Colors.White,
            Secondary = Color.FromArgb("#F2F2F7"),
            Text = Colors.Black,
            TextSecondary = Color.FromArgb("#8E8E93"),
            Accent = accentColor,
            AccentLight = accentLight,
            AccentForeground = accentForeground,
            Border = Color.FromArgb("#E5E5EA"),
            BorderAccent = accentColor,
            Gray100 = Color.FromArgb("#E5E5EA"),
        };
    }

    private static (double H, double S, double L) ColorToHsl(Color color)
    {
        var r = (double)color.Red;
        var g = (double)color.Green;
        var b = (double)color.Blue;
        var cMax = Math.Max(r, Math.Max(g, b));
        var cMin = Math.Min(r, Math.Min(g, b));
        double h = 0, s = 0;
        var l = (cMax + cMin) / 2;

        if (Math.Abs(cMax - cMin) > 0.001)
        {
            var d = cMax - cMin;
            s = l > 0.5 ? d / (2 - cMax - cMin) : d / (cMax + cMin);
            if (Math.Abs(cMax - r) < 0.001)
                h = ((g - b) / d + (g < b ? 6 : 0)) / 6;
            else if (Math.Abs(cMax - g) < 0.001)
                h = ((b - r) / d + 2) / 6;
            else
                h = ((r - g) / d + 4) / 6;
        }

        return (h * 360, s * 100, l * 100);
    }

    private static Color HslToColor(double h, double s, double l)
    {
        s /= 100;
        l /= 100;
        var a = s * Math.Min(l, 1 - l);
        double F(double n)
        {
            var k = (n + h / 30) % 12;
            return l - a * Math.Max(Math.Min(k - 3, Math.Min(9 - k, 1)), -1);
        }
        return Color.FromRgba(F(0), F(8), F(4), 1.0);
    }

    private static double RelativeLuminance(Color c)
    {
        static double Channel(double v) =>
            v <= 0.03928 ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);
        return 0.2126 * Channel(c.Red) + 0.7152 * Channel(c.Green) + 0.0722 * Channel(c.Blue);
    }
}
