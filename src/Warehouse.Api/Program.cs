using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using Serilog.Formatting.Json;
using Warehouse.Api.Extensions;
using Warehouse.Domain.Communication;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Enum;

var builder = WebApplication.CreateBuilder(args);

var databaseConnectionSection = builder.Configuration.GetSection("DatabaseConnection");
var databaseConnection = databaseConnectionSection.Get<DatabaseConnection>() ??
                         throw new Exception("Failed to get db settings");

var connectionString = $"Server={databaseConnection.Server};Database={databaseConnection.Database};" +
                       $"User Id={databaseConnection.User};Password={databaseConnection.Password};";

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<OrderStatus>("order_status");
dataSourceBuilder.MapEnum<WarehouseStatus>("warehouse_status");
dataSourceBuilder.MapEnum<WarehouseLocationStatus>("warehouse_location_status");
dataSourceBuilder.MapEnum<WarehouseLocationType>("warehouse_location_type");
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<WmsContext>((_, options) =>
{
    options.UseNpgsql(dataSource, optionsBuilder =>
    {
        optionsBuilder.MapEnum<OrderStatus>("order_status");
        optionsBuilder.MapEnum<WarehouseStatus>("warehouse_status");
        optionsBuilder.MapEnum<WarehouseLocationStatus>("warehouse_location_status");
        optionsBuilder.MapEnum<WarehouseLocationType>("warehouse_location_type");
        optionsBuilder.MigrationsAssembly(typeof(WmsContext).Assembly.FullName);
    });
});

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.SectionName));

builder.Services.AddApiAndValidation()
    .RegisterServices()
    .AddBackgroundServices()
    .AddRabbitMq();

var app = builder.Build();

app.UseMiddleware()
    .AddApiAndValidation();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new JsonFormatter())
    .CreateLogger();

app.Run();