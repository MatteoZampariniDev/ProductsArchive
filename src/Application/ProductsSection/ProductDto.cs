using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection;

public class ProductDto : IMapFrom<Product>
{
    public Guid Id { get; set; }
    public string? ProductId { get; set; }
    public ProductGroupDto? Group { get; set; }
    public ProductSizeDto? Size { get; set; }
    public string? NetWeight { get; set; }
}
