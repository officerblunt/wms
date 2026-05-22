using FluentValidation;
using Warehouse.Api.Interfaces;
using Warehouse.Api.Middleware;
using Warehouse.Api.Services;
using Warehouse.Api.Validators;

namespace Warehouse.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddApiAndValidation()
        {
            services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
            services.AddOpenApi();
            services.AddControllers(options => { options.Filters.Add<ValidationFilter>(); });
            return services;
        }

        internal IServiceCollection RegisterServices()
        {
            services.AddSingleton<IOrderService, OrdersService>();
            return services;
        }
    }

    extension(WebApplication app)
    {
        internal WebApplication UseMiddleware()
        {
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }

        internal WebApplication AddApiAndValidation()
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.MapControllers();
            return app;
        }
    }
}