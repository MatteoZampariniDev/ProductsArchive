using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsArchive.Domain.Common.Localization;

namespace ProductsArchive.Infrastructure.Persistence.Configurations.Localization;

public class LocalizedStringValueConfig : IEntityTypeConfiguration<LocalizedStringValue>
{
    public void Configure(EntityTypeBuilder<LocalizedStringValue> builder)
    {
        builder.HasKey(x => new { x.LocalizedStringId, x.TwoLetterISOLanguageName } );

        builder.HasOne(x => x.LocalizedString)
            .WithMany(x => x.LocalizedValues)
            .HasForeignKey(x => x.LocalizedStringId);
    }
}