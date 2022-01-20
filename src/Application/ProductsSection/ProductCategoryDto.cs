using AutoMapper;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection;

public class ProductCategoryDto : IMapFrom<ProductCategory>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public void Mapping(Profile profile)
    {
        string twoLetterISOLanguageName = "";
        profile.CreateMap<ProductCategory, ProductCategoryDto>()
            .ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name.LocalizedValues
                .Where(x => x.TwoLetterISOLanguageName == twoLetterISOLanguageName)
                .Select(x => x.Value).SingleOrDefault()));
    }
}
