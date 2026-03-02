namespace Featurama.Maui.Exceptions;

public class FeaturamaApiException : FeaturamaException
{
    public int StatusCode { get; }
    public string? ResponseBody { get; }

    public FeaturamaApiException(int statusCode, string message, string? responseBody = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
