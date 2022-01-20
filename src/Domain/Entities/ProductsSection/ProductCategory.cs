namespace ProductsArchive.Domain.Entities.ProductsSection;

public class ProductCategory : AuditableEntityWithId<Guid>
{
    public LocalizedString Name { get; set; } = new LocalizedString();

    public virtual IList<ProductGroup> Groups { get; set; } = new List<ProductGroup>();
}
