namespace Warehouse.Api.Middleware;

public class CorrelationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        context.Response.Headers["X-Correlation-ID"] = correlationId;

        await next(context);
    }
}