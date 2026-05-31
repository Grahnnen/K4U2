using System.Net;

namespace WebApplication1.Exceptions;

public class DownstreamServiceException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string Title { get; }
    public string SafeDetail { get; }

    public DownstreamServiceException(HttpStatusCode statusCode, string title, string safeDetail) : base(safeDetail)
    {
        StatusCode = statusCode;
        Title = title;
        SafeDetail = safeDetail;
    }
}