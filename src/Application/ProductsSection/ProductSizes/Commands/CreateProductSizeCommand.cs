using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductSizes.Commands;

[Authorize(Roles = "Administrator")]
public record CreateProductSizeCommand() : IRequest<Guid>
{
    public string? Name { get; init; }
}

public class CreateProductSizeCommandValidator : AbstractValidator<CreateProductSizeCommand>
{
    public CreateProductSizeCommandValidator()
    {

    }
}

public class CreateProductSizeCommandHandler : IRequestHandler<CreateProductSizeCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentCultureService _culture;

    public CreateProductSizeCommandHandler(IApplicationDbContext context, ICurrentCultureService culture)
    {
        _context = context;
        _culture = culture;
    }

    public async Task<Guid> Handle(CreateProductSizeCommand request, CancellationToken cancellationToken)
    {
        var size = new ProductSize
        {
            Name = LocalizedString.Create(_culture.CurrentCulture, request.Name)
        };

        _context.ProductSizes.Add(size);

        await _context.SaveChangesAsync(cancellationToken);

        return size.Id;
    }
}
