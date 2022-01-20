using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.Products.Commands;

[Authorize(Roles = "Administrator")]
public record UpdateProductCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public Guid SizeId { get; set; }
    public string? ProductId { get; set; }
    public string? NetWeight { get; set; }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(ProductExists);
        RuleFor(x => x).MustAsync(NotEmptyAndUniqueProductId);
        RuleFor(x => x.SizeId).MustAsync(SizeExists);
        RuleFor(x => x.GroupId).MustAsync(GroupExists);
    }

    public async Task<bool> ProductExists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> GroupExists(Guid groupId, CancellationToken cancellationToken)
    {
        return await _context.ProductGroups
            .AnyAsync(x => x.Id == groupId, cancellationToken);
    }

    public async Task<bool> SizeExists(Guid sizeId, CancellationToken cancellationToken)
    {
        return await _context.ProductSizes
            .AnyAsync(x => x.Id == sizeId, cancellationToken);
    }

    public async Task<bool> NotEmptyAndUniqueProductId(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        return !string.IsNullOrWhiteSpace(command.ProductId) && !await _context.Products
            .AnyAsync(x => x.Id != command.Id && x.ProductId == command.ProductId, cancellationToken);
    }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(x => x.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        product.SizeId = request.SizeId;
        product.GroupId = request.GroupId;
        product.ProductId = request.ProductId;
        product.NetWeight = request.NetWeight;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}