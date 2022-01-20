using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ProductsArchive.Application.ProductsSection.ProductGroups.Queries;

public record GetProductGroupQuery(Guid Id) : IRequest<ProductGroupDto?>;

public class GetProductGroupQueryValidator : AbstractValidator<GetProductGroupQuery>
{
    private readonly IApplicationDbContext _context;

    public GetProductGroupQueryValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(Exists);
    }

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ProductGroups
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}

public class GetProductGroupQueryHandler : IRequestHandler<GetProductGroupQuery, ProductGroupDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentCultureService _culture;

    public GetProductGroupQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentCultureService culture)
    {
        _context = context;
        _mapper = mapper;
        _culture = culture;
    }

    public async Task<ProductGroupDto?> Handle(GetProductGroupQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductGroups
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectToLocalization<ProductGroupDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
            .SingleOrDefaultAsync(cancellationToken);
    }
}