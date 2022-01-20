namespace ProductsArchive.Domain.Entities.ProductsSection;

public class Product : AuditableEntityWithId<Guid>
{
    public Guid GroupId { get; set; }
    public virtual ProductGroup? Group { get; set; }

    public string? ProductId { get; set; }

    public Guid SizeId { get; set; }
    public virtual ProductSize? Size { get; set; }

    public string? NetWeight { get; set; }
}
