using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductSizes.Commands;

[Authorize(Roles = "Administrator")]
public record UpdateProductSizeCommand : IRequest
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
}

public class UpdateProductSizeCommandValidator : AbstractValidator<UpdateProductSizeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductSizeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .MustAsync(Exists)
            .WithMessage("Id not valid.");
    }

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ProductSizes
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}

public class UpdateProductSizeCommandHandler : IRequestHandler<UpdateProductSizeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentCultureService _culture;

    public UpdateProductSizeCommandHandler(IApplicationDbContext context, ICurrentCultureService culture)
    {
        _context = context;
        _culture = culture;
    }

    public async Task<Unit> Handle(UpdateProductSizeCommand request, CancellationToken cancellationToken)
    {
        var size = await _context.ProductSizes
            .IncludeLocalizedProperty(x => x.Name)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (size == null)
        {
            throw new NotFoundException(nameof(ProductSize), request.Id);
        }

        size.Name[_culture.CurrentCulture] = request.Name;
        _context.SetEntityState(size, EntityState.Modified);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
