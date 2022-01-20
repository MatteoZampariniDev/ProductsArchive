using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ProductsArchive.Application.ProductsSection.ProductCategories.Queries;

public record GetProductCategoryQuery(Guid Id) : IRequest<ProductCategoryDto?>;

public class GetProductCategoryQueryValidator : AbstractValidator<GetProductCategoryQuery>
{
    private readonly IApplicationDbContext _context;

    public GetProductCategoryQueryValidator(IApplicationDbContext context)
    {
        _context = context;

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

public class GetProductCategoryQueryHandler : IRequestHandler<GetProductCategoryQuery, ProductCategoryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentCultureService _culture;

    public GetProductCategoryQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentCultureService culture)
    {
        _context = context;
        _mapper = mapper;
        _culture = culture;
    }

    public async Task<ProductCategoryDto?> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectToLocalization<ProductCategoryDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
            .SingleOrDefaultAsync();
    }
}
