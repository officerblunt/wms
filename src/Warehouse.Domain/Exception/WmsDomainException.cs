namespace Warehouse.Domain.Exception;

public class WmsDomainException : System.Exception
{
    public int StatusCode { get; }

    protected WmsDomainException(string message, int statusCode = 400)
        : base(message)
    {
        StatusCode = statusCode;
    }
}