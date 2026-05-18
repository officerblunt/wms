using FluentValidation;
using Serilog;
using Warehouse.Api.Interfaces;
using Warehouse.Api.Middleware;
using Warehouse.Api.Services;
using Warehouse.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();

    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});

builder.Services.AddSingleton<IOrderService, OrdersService>();

var app = builder.Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
    .CreateLogger();

app.UseMiddleware<CorrelationMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();