namespace Featurama.Maui.Exceptions;

public class FeaturamaNetworkException : FeaturamaException
{
    public FeaturamaNetworkException(string message, Exception innerException)
        : base(message, innerException) { }
}
