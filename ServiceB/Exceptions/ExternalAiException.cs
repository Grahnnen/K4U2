using System.Net;

namespace LLMProxy.Exceptions;

public class ExternalAiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string Title { get; }
    public string SafeDetail { get; }

    public ExternalAiException(HttpStatusCode statusCode, string title, string safeDetail) : base(safeDetail)
    {
        StatusCode = statusCode;
        Title = title;
        SafeDetail = safeDetail;
    }
}