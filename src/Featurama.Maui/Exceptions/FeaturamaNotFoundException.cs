namespace Featurama.Maui.Exceptions;

public class FeaturamaNotFoundException : FeaturamaApiException
{
    public FeaturamaNotFoundException(string? responseBody = null)
        : base(404, "Resource not found.", responseBody) { }
}
