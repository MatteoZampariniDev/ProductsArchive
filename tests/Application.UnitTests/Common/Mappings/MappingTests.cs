using System.Runtime.Serialization;
using AutoMapper;
using ProductsArchive.Application.Common.Mappings;
using NUnit.Framework;
using ProductsArchive.Domain.Entities.ProductsSection;
using ProductsArchive.Application.ProductsSection.ProductCategories.Queries;
using ProductsArchive.Application.ProductsSection;

namespace ProductsArchive.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(ProductCategory), typeof(ProductCategoryDto))]
    [TestCase(typeof(ProductSize), typeof(ProductSizeDto))]
    [TestCase(typeof(ProductGroup), typeof(ProductGroupDto))]
    [TestCase(typeof(Product), typeof(ProductDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}
