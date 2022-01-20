using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductGroups.Commands;

[Authorize(Roles = "Administrator")]
public record UpdateProductGroupCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }

    public string? GroupId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class UpdateProductGroupCommandValidator : AbstractValidator<UpdateProductGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(GroupExists);
        RuleFor(x => x).MustAsync(NotEmptyAndUniqueGroupId);
        RuleFor(x => x.CategoryId).MustAsync(CategoryExists);
    }

    public async Task<bool> GroupExists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ProductGroups
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> CategoryExists(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .AnyAsync(x => x.Id == categoryId, cancellationToken);
    }

    public async Task<bool> NotEmptyAndUniqueGroupId(UpdateProductGroupCommand command, CancellationToken cancellationToken)
    {
        return !string.IsNullOrWhiteSpace(command.GroupId) && !await _context.ProductGroups
            .AnyAsync(x => x.Id != command.Id && x.GroupId == command.GroupId, cancellationToken);
    }
}

public class UpdateProductGroupCommandHandler : IRequestHandler<UpdateProductGroupCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentCultureService _culture;

    public UpdateProductGroupCommandHandler(IApplicationDbContext context, ICurrentCultureService culture)
    {
        _context = context;
        _culture = culture;
    }

    public async Task<Unit> Handle(UpdateProductGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _context.ProductGroups
            .IncludeLocalizedProperty(x => x.Name)
            .IncludeLocalizedProperty(x => x.Description)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (group == null)
        {
            throw new NotFoundException(nameof(ProductGroup), request.Id);
        }

        group.CategoryId = request.CategoryId;
        group.GroupId = request.GroupId;
        group.Name[_culture.CurrentCulture] = request.Name;
        group.Description[_culture.CurrentCulture] = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}