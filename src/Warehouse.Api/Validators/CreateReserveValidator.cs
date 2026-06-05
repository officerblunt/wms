using System.Text.RegularExpressions;
using FluentValidation;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Validators;

internal sealed partial class CreateReserveValidator : AbstractValidator<ReserveStockDto>
{
    public CreateReserveValidator()
    {
        RuleForEach(reserve => reserve.Sku)
            .NotEmpty().WithMessage("Sku is required")
            .Matches(SkuRegex()).WithMessage("Sku must match");
        
        RuleFor(reserve => reserve.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero");
        
        RuleFor(reserve => reserve.WarehouseId)
            .NotEqual(Guid.Empty).WithMessage("WarehouseId must be a valid GUID");
    }

    [GeneratedRegex(@"WMS-\d{5}")]
    private static partial Regex SkuRegex();
}