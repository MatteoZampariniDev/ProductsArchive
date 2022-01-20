using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ProductsArchive.Application.ProductsSection.Products.Queries;

public record GetProductQuery(Guid Id) : IRequest<ProductDto?>;

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    private readonly IApplicationDbContext _context;

    public GetProductQueryValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(Exists);
    }

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentCultureService _culture;

    public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentCultureService culture)
    {
        _context = context;
        _mapper = mapper;
        _culture = culture;
    }

    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectToLocalization<ProductDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
            .SingleOrDefaultAsync(cancellationToken);
    }
}