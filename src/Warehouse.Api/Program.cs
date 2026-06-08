using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using Warehouse.Api;
using Warehouse.Api.Extensions;
using Warehouse.Api.Interfaces;
using Warehouse.Api.Middleware;
using Warehouse.Api.Services;
using Warehouse.Api.Validators;
using Warehouse.Infrastructure;
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

builder.Services.AddDbContext<WmsContext>(options => options.UseNpgsql(
        dataSource,
        x => x.MigrationsAssembly(typeof(WmsContext).Assembly.FullName)
    )
);

builder.Services.AddApiAndValidation()
    .RegisterServices()
    .AddBackgroundServices();

var app = builder.Build();

app.UseMiddleware()
    .AddApiAndValidation();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
    .CreateLogger();

app.Run();