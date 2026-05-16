using Warehouse.Domain.Exception;

namespace Warehouse.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            WmsDomainException domainEx => domainEx.StatusCode,
            _ => StatusCodes.Status500InternalServerError
        };

        var correlationId = context.Response.Headers["CorrelationId"].ToString();

        logger.LogError(exception, "An error occurred: {Message}. Status Code: {StatusCode}. CorrelationId: {CorrelationId}",
            exception.Message, statusCode, correlationId);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new { error = exception.Message };
        return context.Response.WriteAsJsonAsync(response);
    }
}