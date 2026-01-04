using FluentValidation;
using Nlayer.Core.DTOs;

namespace NLayer.Service.Validation;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required!").NotEmpty().WithMessage("{PropertyName} is required!");
        RuleFor(x => x.Price).InclusiveBetween(1,decimal.MaxValue).WithMessage("{PropertyName} must be between {MinValue} and {MaxValue}");
        RuleFor(x => x.Stock).InclusiveBetween(1,int.MaxValue).WithMessage("{PropertyName} must be between {MinValue} and {MaxValue}");
        RuleFor(x => x.CategoryId).InclusiveBetween(1,int.MaxValue).WithMessage("{PropertyName} must be between {MinValue} and {MaxValue}");
    }
}