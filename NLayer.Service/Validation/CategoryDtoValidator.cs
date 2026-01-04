using FluentValidation;
using Nlayer.Core.DTOs;

namespace NLayer.Service.Validation;

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required");
    }
}