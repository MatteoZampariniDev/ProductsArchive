using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductCategories.Commands;

/// <summary>
/// Delete an existing <see cref="ProductCategory"/>
/// </summary>
/// <param name="Id"></param>
[Authorize(Roles = "Administrator")]
public record DeleteProductCategoryCommand(Guid Id) : IRequest;

public class DeleteProductCategoryCommandValidator : AbstractValidator<DeleteProductCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // check if a ProductCategory with provided id exists
        RuleFor(x => x.Id)
            .MustAsync(Exists)
            .WithMessage("Id not valid.");
    }

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}

public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        // get the ProductCategory with localized properties
        var category = await _context.ProductCategories
            .Where(x => x.Id == request.Id)
            .IncludeLocalizedProperty(x => x.Name!)
            .SingleOrDefaultAsync(cancellationToken);

        if (category == null)
        {
            throw new NotFoundException(nameof(ProductCategory), request.Id);
        }

        // remove category and localized properties
        _context.LocalizedStrings.Remove(category.Name);
        _context.ProductCategories.Remove(category);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
