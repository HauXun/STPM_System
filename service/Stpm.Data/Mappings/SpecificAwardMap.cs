using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class SpecificAwardMap : IEntityTypeConfiguration<SpecificAward>
{
    public void Configure(EntityTypeBuilder<SpecificAward> builder)
    {
        builder.ToTable("SpecificAward");

        builder.HasIndex(e => new { e.BonusPrize, e.Year, e.RankAwardId }, "UQ_SpecificAward")
               .IsUnique();

        builder.Property(p => p.Passed)
               .IsRequired()
               .HasDefaultValue(false);

        builder.HasOne(d => d.RankAward)
               .WithMany(p => p.SpecificAwards)
               .HasForeignKey(d => d.RankAwardId)
               .HasConstraintName("FK_SpecificAward_RankAward")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
