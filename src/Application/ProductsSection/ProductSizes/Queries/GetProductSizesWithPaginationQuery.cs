using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.Common.Validators;

namespace ProductsArchive.Application.ProductsSection.ProductSizes.Queries;

public class GetProductSizesWithPaginationQuery : IRequest<PaginatedList<ProductSizeDto>>
{
    public PaginationDetails PaginationDetails { get; set; } = new PaginationDetails();
    public SortDetails SortDetails { get; set; } = new SortDetails();
    public List<FilterDetails> FilterDetails { get; set; } = new List<FilterDetails>();
}

public class GetProductSizesWithPaginationQueryValidator : AbstractValidator<GetProductSizesWithPaginationQuery>
{
    public GetProductSizesWithPaginationQueryValidator(
        PaginationDetailsValidator paginationValidator,
        SortDetailsValidator sortValidator)
    {
        RuleFor(x => x.PaginationDetails)
            .SetValidator(paginationValidator);

        RuleFor(x => x.SortDetails)
            .SetValidator(sortValidator.AsType<ProductSizeDto>());
    }
}

public class GetProductSizesWithPaginationQueryHandler : IRequestHandler<GetProductSizesWithPaginationQuery, PaginatedList<ProductSizeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentCultureService _culture;

    public GetProductSizesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentCultureService culture)
    {
        _context = context;
        _mapper = mapper;
        _culture = culture;
    }

    public async Task<PaginatedList<ProductSizeDto>> Handle(GetProductSizesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductSizes
            .AsNoTracking()
            .ProjectToLocalization<ProductSizeDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
            .Filter(request.FilterDetails.ToArray())
            .Sort(request.SortDetails)
            .ToPaginatedListAsync(request.PaginationDetails, cancellationToken);
    }
}