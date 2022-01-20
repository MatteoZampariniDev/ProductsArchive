using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductGroups.Commands;

[Authorize(Roles = "Administrator")]
public record CreateProductGroupCommand : IRequest<Guid>
{
    public Guid CategoryId { get; init; }
    public string? GroupId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}

public class CreateProductGroupCommandValidator : AbstractValidator<CreateProductGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProductGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.CategoryId).MustAsync(Exists);
        RuleFor(x => x.GroupId).MustAsync(NotEmptyAndUniqueGroupId);
    }

    public async Task<bool> Exists(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .AnyAsync(x => x.Id == categoryId, cancellationToken);
    }

    public async Task<bool> NotEmptyAndUniqueGroupId(string? groupId, CancellationToken cancellationToken)
    {
        return !string.IsNullOrWhiteSpace(groupId) && !await _context.ProductGroups
            .AnyAsync(x => x.GroupId == groupId, cancellationToken);
    }
}

public class CreateProductGroupCommandHandler : IRequestHandler<CreateProductGroupCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentCultureService _culture;

    public CreateProductGroupCommandHandler(IApplicationDbContext context, ICurrentCultureService culture)
    {
        _context = context;
        _culture = culture;
    }

    public async Task<Guid> Handle(CreateProductGroupCommand request, CancellationToken cancellationToken)
    {
        var group = new ProductGroup
        {
            CategoryId = request.CategoryId,
            GroupId = request.GroupId,
            Name = LocalizedString.Create(_culture.CurrentCulture, request.Name),
            Description = LocalizedString.Create(_culture.CurrentCulture, request.Description)
        };

        _context.ProductGroups.Add(group);

        await _context.SaveChangesAsync(cancellationToken);

        return group.Id;
    }
}