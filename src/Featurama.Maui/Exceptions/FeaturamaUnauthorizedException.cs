namespace Featurama.Maui.Exceptions;

public class FeaturamaUnauthorizedException : FeaturamaApiException
{
    public FeaturamaUnauthorizedException(string? responseBody = null)
        : base(401, "Invalid or missing API key.", responseBody) { }
}
