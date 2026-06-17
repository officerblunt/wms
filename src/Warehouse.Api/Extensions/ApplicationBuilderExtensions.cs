using FluentValidation;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Warehouse.Api.Interfaces;
using Warehouse.Api.Middleware;
using Warehouse.Api.Services;
using Warehouse.Api.Services.Background;
using Warehouse.Api.Services.Communication;
using Warehouse.Api.Validators;
using Warehouse.Application.Interfaces.Communication;
using Warehouse.Domain.Communication;

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

        internal IServiceCollection AddBackgroundServices()
        {
            services.AddHostedService<ExpiredReservationsCleaner>();
            services.AddHostedService<OutboxBackgroundWorker>();
            return services;
        }

        internal IServiceCollection AddRabbitMq()
        {
            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>();

                return new ConnectionFactory
                {
                    HostName = options.Value.HostName,
                    Port = options.Value.Port,
                    UserName = options.Value.UserName,
                    Password = options.Value.Password,
                    VirtualHost = options.Value.VirtualHost,
                };
            });

            services.AddSingleton<IConnection>(sp =>
            {
                var factory = sp.GetRequiredService<ConnectionFactory>();
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });

            services.AddSingleton<IChannel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateChannelAsync().GetAwaiter().GetResult();
            });

            services.AddScoped<IRabbitMqProducer, RabbitMqProducer>();

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