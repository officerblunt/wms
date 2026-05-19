using System.Collections.Concurrent;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Warehouse.Api.Validators;

public class ValidationFilter : IAsyncActionFilter
{
    private static readonly ConcurrentDictionary<Type, Type> _validatorTypeCache = new();

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var argumentType = argument.GetType();

            var validatorInterfaceType = _validatorTypeCache.GetOrAdd(argumentType, type =>
                typeof(IValidator<>).MakeGenericType(type));

            if (context.HttpContext.RequestServices.GetService(validatorInterfaceType) is not IValidator validator)
                continue;

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext);

            if (result.IsValid) continue;

            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            context.Result = new BadRequestObjectResult(new { Errors = errors });
            return;
        }

        await next();
    }
}