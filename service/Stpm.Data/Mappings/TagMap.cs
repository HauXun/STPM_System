using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class TagMap : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(e => e.Description)
               .HasMaxLength(500);

        builder.Property(e => e.Name)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(e => e.UrlSlug)
               .HasMaxLength(50)
               .IsRequired();
    }
}
