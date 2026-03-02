namespace Featurama.Maui.UI.Utils;

internal static class VoterIdProvider
{
    private const string Key = "featurama_voter_id";

    public static string GetOrCreate()
    {
        var id = Preferences.Get(Key, string.Empty);
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
            Preferences.Set(Key, id);
        }
        return id;
    }
}
