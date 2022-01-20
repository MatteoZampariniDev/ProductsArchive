using AutoMapper;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.ProductsSection;

public class ProductGroupDto : IMapFrom<ProductGroup>
{
    public Guid Id { get; set; }
    public ProductCategoryDto? Category { get; set; }
    public string? GroupId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public void Mapping(Profile profile)
    {
        string twoLetterISOLanguageName = "";
        profile.CreateMap<ProductGroup, ProductGroupDto>()
            .ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name.LocalizedValues
                .Where(x => x.TwoLetterISOLanguageName == twoLetterISOLanguageName)
                .Select(x => x.Value).SingleOrDefault()))

            .ForMember(dto => dto.Description, opt => opt.MapFrom(src => src.Description.LocalizedValues
                .Where(x => x.TwoLetterISOLanguageName == twoLetterISOLanguageName)
                .Select(x => x.Value).SingleOrDefault()));
    }
}
