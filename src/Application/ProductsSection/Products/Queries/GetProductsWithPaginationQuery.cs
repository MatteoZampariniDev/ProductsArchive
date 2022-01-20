using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.Common.Validators;

namespace ProductsArchive.Application.ProductsSection.Products.Queries;

public record GetProductsWithPaginationQuery : IRequest<PaginatedList<ProductDto>>
{
    public PaginationDetails PaginationDetails { get; set; } = new PaginationDetails();
    public SortDetails SortDetails { get; set; } = new SortDetails();
    public List<FilterDetails> FilterDetails { get; set; } = new List<FilterDetails>();
}

public class GetProductsWithPaginationQueryValidator : AbstractValidator<GetProductsWithPaginationQuery>
{
    public GetProductsWithPaginationQueryValidator(
        PaginationDetailsValidator paginationValidator,
        SortDetailsValidator sortValidator)
    {
        RuleFor(x => x.PaginationDetails)
            .SetValidator(paginationValidator);

        RuleFor(x => x.SortDetails)
            .SetValidator(sortValidator.AsType<ProductDto>());
    }
}

public class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentCultureService _culture;

    public GetProductsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentCultureService culture)
    {
        _context = context;
        _mapper = mapper;
        _culture = culture;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .ProjectToLocalization<ProductDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
            .Filter(request.FilterDetails.ToArray())
            .Sort(request.SortDetails)
            .ToPaginatedListAsync(request.PaginationDetails, cancellationToken);
    }
}