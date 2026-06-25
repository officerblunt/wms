using FluentValidation;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Validators;

public class CreateProductValidator : AbstractValidator<ProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().WithMessage("Product name is required");
        RuleFor(dto => dto.Sku).NotEmpty().WithMessage("Sku is required");
        RuleFor(dto => dto.Weight).GreaterThan(0).WithMessage("Weight must be greater than 0");
        RuleFor(dto => dto.Length).GreaterThan(0).WithMessage("Length must be greater than 0");
        RuleFor(dto => dto.Width).GreaterThan(0).WithMessage("Width must be greater than 0");
        RuleFor(dto => dto.Height).GreaterThan(0).WithMessage("Height must be greater than 0");
    }
}