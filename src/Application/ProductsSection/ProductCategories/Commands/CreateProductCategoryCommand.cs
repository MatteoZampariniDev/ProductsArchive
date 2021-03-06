using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductCategories.Commands;

/// <summary>
/// Create a new <see cref="ProductCategory"/>
/// </summary>
[Authorize(Roles = "Administrator")]
public record CreateProductCategoryCommand : IRequest<Guid>
{
    public string? Name { get; init; }
}

public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator() { }
}

public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentCultureService _culture;

    public CreateProductCategoryCommandHandler(IApplicationDbContext context, ICurrentCultureService culture)
    {
        _context = context;
        _culture = culture;
    }

    public async Task<Guid> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        // create category
        var category = new ProductCategory
        {
            Name = LocalizedString.Create(_culture.CurrentCulture, request.Name)
        };

        // add to db
        _context.ProductCategories.Add(category);
        
        await _context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
