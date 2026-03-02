namespace Featurama.Maui.Models;

public sealed class ProjectConfig
{
    public BrandingConfig Branding { get; set; } = new();
    public string EmailCollection { get; set; } = "none";
}

public sealed class BrandingConfig
{
    public bool ShowBranding { get; set; } = true;
}
