using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Warehouse.Api;
using Warehouse.Api.Extensions;
using Warehouse.Api.Interfaces;
using Warehouse.Api.Middleware;
using Warehouse.Api.Services;
using Warehouse.Api.Validators;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var databaseConnectionSection = builder.Configuration.GetSection("DatabaseConnection");
var databaseConnection = databaseConnectionSection.Get<DatabaseConnection>() ??
                         throw new Exception("Failed to get db settings");
builder.Services.AddDbContext<WmsContext>(options => options.UseNpgsql(
    $"Server={databaseConnection.Server};Database={databaseConnection.Database};" +
    $"User Id={databaseConnection.User};Password={databaseConnection.Password};",
    x => x.MigrationsAssembly(typeof(WmsContext).Assembly.FullName)));

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