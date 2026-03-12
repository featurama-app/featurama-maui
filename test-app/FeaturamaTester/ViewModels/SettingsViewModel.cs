using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FeaturamaTester.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _apiKey = Config.ApiKey;

    [ObservableProperty]
    private string _baseUrl = Config.BaseUrl;

    [ObservableProperty]
    private string _userId = Config.UserId;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            StatusMessage = "API key cannot be empty.";
            return;
        }

        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            StatusMessage = "Base URL cannot be empty.";
            return;
        }

        Config.ApiKey = ApiKey.Trim();
        Config.BaseUrl = BaseUrl.Trim();

        StatusMessage = "Settings saved. Restart the app for changes to take effect.";

        await Shell.Current.DisplayAlert(
            "Settings Saved",
            "Configuration updated. Please restart the app to apply changes to the SDK client.",
            "OK");
    }
}
