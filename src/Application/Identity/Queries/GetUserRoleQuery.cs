namespace ProductsArchive.Application.Identity.Queries;
public record GetUserRoleQuery(Guid Id) : IRequest<string?>;

public class GetUserRoleQueryValidator : AbstractValidator<GetUserRoleQuery>
{
    private readonly IIdentityService _identityService;

    public GetUserRoleQueryValidator(IIdentityService identityService)
    {
        _identityService = identityService;

        RuleFor(x => x.Id)
            .MustAsync(Exists)
            .WithMessage("Id not valid.");
    }

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await _identityService.UserExistsAsync(id.ToString());
    }
}

public class GetProductSizeQueryHandler : IRequestHandler<GetUserRoleQuery, string?>
{
    private readonly IIdentityService _identityService;

    public GetProductSizeQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string?> Handle(GetUserRoleQuery request, CancellationToken cancellationToken)
    {
        return await _identityService.GetUserRoleAsync(request.Id.ToString());
    }
}
