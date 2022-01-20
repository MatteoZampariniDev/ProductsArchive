using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.Products.Commands;

[Authorize(Roles = "Administrator")]
public record CreateProductCommand : IRequest<Guid>
{
    public Guid GroupId { get; set; }
    public Guid SizeId { get; set; }
    public string? ProductId { get; set; }
    public string? NetWeight { get; set; }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.GroupId)
            .MustAsync(GroupExists)
            .WithMessage("GroupId not valid.");

        RuleFor(x => x.SizeId)
            .MustAsync(SizeExists)
            .WithMessage("SizeId not valid.");

        RuleFor(x => x.ProductId)
            .MustAsync(NotEmptyAndUniqueProductId)
            .WithMessage("ProductId not valid.");
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

    public async Task<bool> NotEmptyAndUniqueProductId(string? productId, CancellationToken cancellationToken)
    {
        return !string.IsNullOrWhiteSpace(productId) && !await _context.Products
            .AnyAsync(x => x.ProductId == productId, cancellationToken);
    }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            ProductId = request.ProductId,
            GroupId = request.GroupId,
            SizeId = request.SizeId,
            NetWeight = request.NetWeight
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
