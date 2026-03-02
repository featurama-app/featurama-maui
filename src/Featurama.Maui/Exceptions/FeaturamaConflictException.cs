namespace Featurama.Maui.Exceptions;

public class FeaturamaConflictException : FeaturamaApiException
{
    public FeaturamaConflictException(string? responseBody = null)
        : base(409, "Conflict: resource already exists.", responseBody) { }
}
