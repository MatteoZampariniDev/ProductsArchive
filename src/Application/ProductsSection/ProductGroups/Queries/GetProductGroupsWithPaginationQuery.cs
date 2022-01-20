using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Extensions;
using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.Common.Validators;

namespace ProductsArchive.Application.ProductsSection.ProductGroups.Queries;

public record GetProductGroupsWithPaginationQuery : IRequest<PaginatedList<ProductGroupDto>>
{
    public PaginationDetails PaginationDetails { get; set; } = new PaginationDetails();
    public SortDetails SortDetails { get; set; } = new SortDetails();
    public List<FilterDetails> FilterDetails { get; set; } = new List<FilterDetails>();
}

public class GetProductGroupsWithPaginationQueryValidator : AbstractValidator<GetProductGroupsWithPaginationQuery>
{
    public GetProductGroupsWithPaginationQueryValidator(
        PaginationDetailsValidator paginationValidator,
        SortDetailsValidator sortValidator)
    {
        RuleFor(x => x.PaginationDetails)
            .SetValidator(paginationValidator);

        RuleFor(x => x.SortDetails)
            .SetValidator(sortValidator.AsType<ProductGroupDto>());
    }
}

public class GetProductGroupsWithPaginationQueryHandler : IRequestHandler<GetProductGroupsWithPaginationQuery, PaginatedList<ProductGroupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentCultureService _culture;

    public GetProductGroupsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentCultureService culture)
    {
        _context = context;
        _mapper = mapper;
        _culture = culture;
    }

    public async Task<PaginatedList<ProductGroupDto>> Handle(GetProductGroupsWithPaginationQuery request, CancellationToken cancellationToken)
    {
       // await _context.ProductGroups.AsNoTracking()
       //.ProjectToLocalization<ProductGroupDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
       //.Where(x => x.Name.ToLo.Contains("query") || x.Description.Contains("query"))
       //.ToPaginatedListAsync(request.PaginationDetails, cancellationToken);

        return await _context.ProductGroups
            .AsNoTracking()
            .ProjectToLocalization<ProductGroupDto>(_mapper.ConfigurationProvider, _culture.CurrentCulture)
            .Filter(request.FilterDetails.ToArray())
            .Sort(request.SortDetails)
            .ToPaginatedListAsync(request.PaginationDetails, cancellationToken);
    }
}