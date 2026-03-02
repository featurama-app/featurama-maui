using Featurama.Maui.UI.Strings;
using Featurama.Maui.UI.Theme;

namespace Featurama.Maui.UI.Views;

internal sealed class HeaderView : ContentView
{
    public HeaderView(FeaturamaTheme theme, FeaturamaStrings strings, Action? onClose, Action onAdd)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(40),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(40),
            },
            Padding = new Thickness(8, 8, 8, 16),
        };

        if (onClose != null)
        {
            var closeBtn = new Button
            {
                Text = "\u2715",
                TextColor = theme.Text,
                BackgroundColor = Colors.Transparent,
                FontSize = 18,
                WidthRequest = 40,
                HeightRequest = 40,
            };
            closeBtn.Clicked += (_, _) => onClose();
            grid.Add(closeBtn, 0);
        }

        var title = new Label
        {
            Text = strings.Title,
            TextColor = theme.Text,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
        };
        grid.Add(title, 1);

        var addBtn = new Button
        {
            Text = "+",
            TextColor = theme.Accent,
            BackgroundColor = Colors.Transparent,
            FontSize = 22,
            WidthRequest = 40,
            HeightRequest = 40,
        };
        addBtn.Clicked += (_, _) => onAdd();
        grid.Add(addBtn, 2);

        Content = grid;
    }
}
