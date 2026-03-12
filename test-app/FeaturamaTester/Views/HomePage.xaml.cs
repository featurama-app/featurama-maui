using Featurama.Maui.UI;
using Featurama.Maui.UI.Theme;

namespace FeaturamaTester.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnOpenFeaturama(object sender, EventArgs e)
    {
        var page = new FeaturamaPage(new FeaturamaPageOptions
        {
            AccentColor = Color.FromArgb("#6366F1"),
            ColorScheme = FeaturamaColorScheme.Light,
            OnClose = async () => await Navigation.PopModalAsync(),
        });
        await Navigation.PushModalAsync(new NavigationPage(page));
    }

    private async void OnOpenSettings(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//settings");
    }
}
