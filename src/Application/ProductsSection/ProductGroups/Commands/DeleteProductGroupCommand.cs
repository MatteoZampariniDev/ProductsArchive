using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Security;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection.ProductGroups.Commands;

[Authorize(Roles = "Administrator")]
public record DeleteProductGroupCommand(Guid Id) : IRequest;

public class DeleteProductGroupCommandValidator : AbstractValidator<DeleteProductGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductGroupCommandValidator(IApplicationDbContext context)
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

public class DeleteProductGroupCommandHandler : IRequestHandler<DeleteProductGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _context.ProductGroups
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (group == null)
        {
            throw new NotFoundException(nameof(ProductGroup), request.Id);
        }

        _context.ProductGroups.Remove(group);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}