using Featurama.Maui.Models;
using Featurama.Maui.UI.Strings;
using Featurama.Maui.UI.Theme;

namespace Featurama.Maui.UI.Views;

internal sealed class RequestCardView : ContentView
{
    public RequestCardView(FeaturamaTheme theme, FeaturamaStrings strings, FeatureRequest request, bool isVoting, Action onToggleVote)
    {
        var voteBtn = new Button
        {
            Text = $"\u25B2\n{request.VoteCount}",
            TextColor = theme.Accent,
            BackgroundColor = theme.AccentLight,
            CornerRadius = 8,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            WidthRequest = 52,
            HeightRequest = 56,
            IsEnabled = !isVoting,
            Padding = new Thickness(4),
        };
        voteBtn.Clicked += (_, _) => onToggleVote();

        var titleLabel = new Label
        {
            Text = request.Title,
            TextColor = theme.Text,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
        };

        var titleRow = new HorizontalStackLayout
        {
            Spacing = 8,
            Children = { titleLabel },
        };

        if (request.Status == FeatureRequestStatus.Roadmap)
        {
            var badge = new Border
            {
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 4 },
                BackgroundColor = theme.AccentLight,
                StrokeThickness = 0,
                Padding = new Thickness(8, 2),
                VerticalOptions = LayoutOptions.Center,
                Content = new Label
                {
                    Text = strings.BadgePlanned,
                    TextColor = theme.Accent,
                    FontSize = 11,
                    FontAttributes = FontAttributes.Bold,
                },
            };
            titleRow.Children.Add(badge);
        }

        var contentLayout = new VerticalStackLayout
        {
            Spacing = 4,
            Children = { titleRow },
        };

        if (!string.IsNullOrEmpty(request.Description))
        {
            contentLayout.Children.Add(new Label
            {
                Text = request.Description,
                TextColor = theme.TextSecondary,
                FontSize = 13,
                MaxLines = 2,
                LineBreakMode = LineBreakMode.TailTruncation,
            });
        }

        Content = new Border
        {
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
            Stroke = new SolidColorBrush(theme.Border),
            BackgroundColor = theme.Card,
            StrokeThickness = 1,
            Padding = new Thickness(14),
            Margin = new Thickness(0, 0, 0, 12),
            Content = new HorizontalStackLayout
            {
                Spacing = 12,
                Children = { voteBtn, contentLayout },
            },
        };
    }
}
