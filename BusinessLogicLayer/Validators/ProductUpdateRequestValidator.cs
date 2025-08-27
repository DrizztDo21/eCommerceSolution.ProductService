using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;
public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(x => x.ProductID)
            .NotEmpty().WithMessage("ProductID is required.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid category.");

        RuleFor(x => x.UnitPrice)
            .InclusiveBetween(0, double.MaxValue).WithMessage($"Price should be between 0 to{double.MaxValue}");

        RuleFor(x => x.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity in Stock should be between 0 to{int.MaxValue}");
    }
}
