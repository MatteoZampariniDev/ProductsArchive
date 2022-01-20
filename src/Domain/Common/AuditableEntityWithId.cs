namespace ProductsArchive.Domain.Common;

public abstract class AuditableEntityWithId<T> : AuditableEntity
{
    public T? Id { get; private set; }

    public void ForceSet_Id(T id) => Id = id;
}
