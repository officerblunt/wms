using FluentValidation;
using MediatR;

namespace Warehouse.Api.Validators;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationContext = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(validationContext, cancellationToken))
        );

        var errors = validationFailures
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .Select(error =>
                {
                    var message = string.Format("{0}: {1}", error.PropertyName, error.ErrorMessage);
                    return new Exception(
                        message
                    );
                }
            )
            .ToList();

        if (errors.Count != 0) throw new Domain.Exception.ValidationException(errors);

        var response = await next(cancellationToken);

        return response;
    }
}