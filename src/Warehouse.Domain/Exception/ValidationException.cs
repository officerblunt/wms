using System.Text;

namespace Warehouse.Domain.Exception;

public class ValidationException : WmsDomainException
{
    public ValidationException(List<System.Exception> exceptions) : this(string.Join('\n',
        exceptions.Select(e => e.Message).ToList()))
    {
    }

    protected ValidationException(string message) : base(message)
    {
    }
}