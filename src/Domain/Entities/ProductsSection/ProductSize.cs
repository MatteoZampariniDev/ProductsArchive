namespace ProductsArchive.Domain.Entities.ProductsSection;

public class ProductSize : AuditableEntityWithId<Guid>
{
    public LocalizedString Name { get; set; } = new LocalizedString();

    public virtual IList<Product> Products { get; private set; } = new List<Product>();
}