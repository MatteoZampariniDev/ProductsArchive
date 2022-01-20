using ProductsArchive.Application.Common.Models;

namespace ProductsArchive.Application.Common.Validators;

public class SortDetailsValidator : AbstractValidator<SortDetails>
{
    private Type? Type { get; set; }

    public SortDetailsValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage($"SortDetails model not valid.");

        RuleFor(x => x.Order)
            .Must(order => order.ToUpper() == "ASC" || order.ToUpper() == "DESC")
            .WithMessage($"SortOrder not valid: must be either 'ASC' or 'DESC'.");

        RuleFor(x => x.Property)
            .Must(BeValidProperty)
            .When(x => !string.IsNullOrEmpty(x.Property))
            .WithMessage($"SortProperty not valid.");
    }

    public SortDetailsValidator AsType<T>() => AsType(typeof(T));
    public SortDetailsValidator AsType(Type type)
    {
        Type = type;
        return this;
    }

    private bool BeValidProperty(string? propertyPath)
    {
        if (Type == null)
        {
            throw new Exception("A Type is required.");
        }

        return !string.IsNullOrWhiteSpace(propertyPath)
            && TypeHelpers.IsValidProperty(Type, propertyPath, false);
    }
}
