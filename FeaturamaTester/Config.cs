namespace FeaturamaTester;

public static class Config
{
    public static string ApiKey { get; set; } = "fm_live_REPLACE_WITH_YOUR_KEY";
    public static string BaseUrl { get; set; } = "http://localhost:5001";
    public static string UserId { get; } = $"maui_test_user_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
}
