using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductSizes.Commands;

[Authorize(Roles = "Administrator")]
public record DeleteProductSizeCommand(Guid Id) : IRequest;

public class DeleteProductSizeCommandValidator : AbstractValidator<DeleteProductSizeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductSizeCommandValidator(IApplicationDbContext context)
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

public class DeleteProductSizeCommandHandler : IRequestHandler<DeleteProductSizeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductSizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductSizeCommand request, CancellationToken cancellationToken)
    {
        var size = await _context.ProductSizes
            .Where(x => x.Id == request.Id)
            .IncludeLocalizedProperty(x => x.Name)
            .SingleOrDefaultAsync();

        if (size == null)
        {
            throw new NotFoundException(nameof(ProductSize), request.Id);
        }

        _context.LocalizedStrings.Remove(size.Name);
        _context.ProductSizes.Remove(size);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
