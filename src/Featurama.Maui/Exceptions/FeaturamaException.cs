namespace Featurama.Maui.Exceptions;

public class FeaturamaException : Exception
{
    public FeaturamaException(string message) : base(message) { }
    public FeaturamaException(string message, Exception innerException) : base(message, innerException) { }
}
