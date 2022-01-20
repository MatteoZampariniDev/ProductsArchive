namespace ProductsArchive.Domain.Entities.ProductsSection;

public class ProductGroup : AuditableEntityWithId<Guid>
{
    public Guid CategoryId { get; set; }
    public virtual ProductCategory? Category { get; set; }

    public string? GroupId { get; set; }

    public LocalizedString Name { get; set; } = new LocalizedString();
    public LocalizedString Description { get; set; } = new LocalizedString();

    public virtual IList<Product> Products { get; set; } = new List<Product>();
}