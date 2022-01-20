using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsArchive.Domain.Common.Localization;

namespace ProductsArchive.Infrastructure.Persistence.Configurations.Localization;

public class LocalizedStringConfig : IEntityTypeConfiguration<LocalizedString>
{
    public void Configure(EntityTypeBuilder<LocalizedString> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
    }
}