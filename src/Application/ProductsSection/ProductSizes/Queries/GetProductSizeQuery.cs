using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ProductsArchive.Application.ProductsSection.ProductSizes.Queries;

public record GetProductSizeQuery(Guid Id) : IRequest<ProductSizeDto>;

public class GetProductSizeQueryValidator : AbstractValidator<GetProductSizeQuery>
{
    private readonly IApplicationDbContext _context;

    public GetProductSizeQueryValidator(IApplicationDbContext context)
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

public class GetProductSizeQueryHandler : IRequestHandler<GetProductSizeQuery, ProductSizeDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentCultureService _culture;

    public GetProductSizeQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentCultureService culture)
    {
        _context = context;
        _mapper = mapper;
        _culture = culture;
    }

    public async Task<ProductSizeDto?> Handle(GetProductSizeQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductSizes
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectToLocalization<ProductSizeDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
            .SingleOrDefaultAsync();
    }
}
