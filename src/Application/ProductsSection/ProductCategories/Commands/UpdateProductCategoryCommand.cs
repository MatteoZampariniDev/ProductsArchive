using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductCategories.Commands;

/// <summary>
/// Update a <see cref="ProductCategory"/>
/// </summary>
[Authorize(Roles = "Administrator")]
public record UpdateProductCategoryCommand : IRequest
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
}

public class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // check if a category with provided Id exists
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

public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentCultureService _culture;

    public UpdateProductCategoryCommandHandler(IApplicationDbContext context, ICurrentCultureService culture)
    {
        _context = context;
        _culture = culture;
    }

    public async Task<Unit> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        // get category with localized properties
        var category = await _context.ProductCategories
            .Where(x => x.Id == request.Id)
            .IncludeLocalizedProperty(x => x.Name!)
            .SingleOrDefaultAsync(cancellationToken);

        if (category == null)
        {
            throw new NotFoundException(nameof(ProductCategory), request.Id);
        }

        // update localized property
        category.Name![_culture.CurrentCulture] = request.Name;

        // set EntityState.Modified otherwise the change in the localized property
        // is not seen as a change to the entity
        _context.SetEntityState(category, EntityState.Modified);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
