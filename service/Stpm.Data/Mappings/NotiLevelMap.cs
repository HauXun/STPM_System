using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class NotiLevelMap : IEntityTypeConfiguration<NotiLevel>
{
    public void Configure(EntityTypeBuilder<NotiLevel> builder)
    {
        builder.ToTable("NotiLevel");

        builder.Property(e => e.LevelName)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(e => e.Description)
               .HasMaxLength(1000);
    }
}
