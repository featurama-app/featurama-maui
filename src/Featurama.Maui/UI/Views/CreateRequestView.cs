using Featurama.Maui.UI.Strings;
using Featurama.Maui.UI.Theme;

namespace Featurama.Maui.UI.Views;

internal sealed class CreateRequestView : ContentView
{
    private readonly Entry _titleEntry;
    private readonly Editor _descEditor;
    private readonly Button _submitBtn;
    private bool _isSubmitting;

    public event Func<string, string, Task>? SubmitRequested;
    public event EventHandler? CancelRequested;

    public CreateRequestView(FeaturamaTheme theme, FeaturamaStrings strings)
    {
        _titleEntry = new Entry
        {
            Placeholder = strings.TitlePlaceholder,
            PlaceholderColor = theme.TextSecondary,
            TextColor = theme.Text,
            BackgroundColor = theme.Secondary,
            FontSize = 16,
        };

        _descEditor = new Editor
        {
            Placeholder = strings.DescriptionPlaceholder,
            PlaceholderColor = theme.TextSecondary,
            TextColor = theme.Text,
            BackgroundColor = theme.Secondary,
            FontSize = 16,
            HeightRequest = 80,
        };

        var cancelBtn = new Button
        {
            Text = strings.Cancel,
            TextColor = theme.Text,
            BackgroundColor = theme.Secondary,
            CornerRadius = 8,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Fill,
        };
        cancelBtn.Clicked += (_, _) => CancelRequested?.Invoke(this, EventArgs.Empty);

        _submitBtn = new Button
        {
            Text = strings.Submit,
            TextColor = theme.AccentForeground,
            BackgroundColor = theme.Accent,
            CornerRadius = 8,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Fill,
        };
        _submitBtn.Clicked += async (_, _) => await HandleSubmit();

        var buttonsGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
            },
            ColumnSpacing = 12,
        };
        buttonsGrid.Add(cancelBtn, 0);
        buttonsGrid.Add(_submitBtn, 1);

        Content = new Border
        {
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
            Stroke = new SolidColorBrush(theme.BorderAccent),
            BackgroundColor = theme.Card,
            StrokeThickness = 1,
            Padding = new Thickness(16),
            Margin = new Thickness(16, 0, 16, 16),
            Content = new VerticalStackLayout
            {
                Spacing = 12,
                Children = { _titleEntry, _descEditor, buttonsGrid },
            },
        };
    }

    private async Task HandleSubmit()
    {
        var title = _titleEntry.Text?.Trim();
        if (string.IsNullOrEmpty(title) || _isSubmitting) return;
        _isSubmitting = true;
        _submitBtn.Opacity = 0.5;
        try
        {
            if (SubmitRequested != null)
                await SubmitRequested(title, _descEditor.Text?.Trim() ?? "");
            _titleEntry.Text = "";
            _descEditor.Text = "";
        }
        finally
        {
            _isSubmitting = false;
            _submitBtn.Opacity = 1;
        }
    }
}
