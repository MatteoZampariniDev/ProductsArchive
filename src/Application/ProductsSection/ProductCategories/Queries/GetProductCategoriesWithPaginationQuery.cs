using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.Common.Validators;

namespace ProductsArchive.Application.ProductsSection.ProductCategories.Queries;

public record GetProductCategoriesWithPaginationQuery : IRequest<PaginatedList<ProductCategoryDto>>
{
    public PaginationDetails PaginationDetails { get; set; } = new PaginationDetails();
    public SortDetails SortDetails { get; set; } = new SortDetails();
    public List<FilterDetails> FilterDetails { get; set; } = new List<FilterDetails>();
}

public class GetProductCategoriesWithPaginationQueryValidator : AbstractValidator<GetProductCategoriesWithPaginationQuery>
{
    public GetProductCategoriesWithPaginationQueryValidator(
        PaginationDetailsValidator paginationValidator,
        SortDetailsValidator sortValidator)
    {
        RuleFor(x => x.PaginationDetails).SetValidator(paginationValidator);
        RuleFor(x => x.SortDetails).SetValidator(sortValidator.AsType<ProductCategoryDto>());
    }
}

public class GetProductCategoriesWithPaginationQueryHandler : IRequestHandler<GetProductCategoriesWithPaginationQuery, PaginatedList<ProductCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentCultureService _culture;

    public GetProductCategoriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentCultureService culture)
    {
        _context = context;
        _mapper = mapper;
        _culture = culture;
    }

    public async Task<PaginatedList<ProductCategoryDto>> Handle(GetProductCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .AsNoTracking()
            .ProjectToLocalization<ProductCategoryDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
            .Filter(request.FilterDetails.ToArray())
            .Sort(request.SortDetails)
            .ToPaginatedListAsync(request.PaginationDetails, cancellationToken);
    }
}
