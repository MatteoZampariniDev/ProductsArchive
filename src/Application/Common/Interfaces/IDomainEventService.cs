using ProductsArchive.Domain.Common;

namespace ProductsArchive.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
