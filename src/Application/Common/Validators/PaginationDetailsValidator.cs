using FluentValidation;
using ProductsArchive.Application.Common.Models;

namespace ProductsArchive.Application.Common.Validators;

public class PaginationDetailsValidator : AbstractValidator<PaginationDetails>
{
    public PaginationDetailsValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage($"Invalid PaginationDetails model.");

        RuleFor(x => x.PageIndex)
            .Must(pageNumber => pageNumber >= 0)
            .WithMessage($"Invalid PageIndex model.");

        RuleFor(x => x.PageSize)
            .Must(pageSize => pageSize > 0)
            .WithMessage($"Invalid PageSize model.");
    }
}