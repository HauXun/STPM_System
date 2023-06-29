using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class RankAwardMap : IEntityTypeConfiguration<RankAward>
{
    public void Configure(EntityTypeBuilder<RankAward> builder)
    {
        builder.ToTable("RankAward");

        builder.Property(e => e.AwardName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.UrlSlug)
               .HasMaxLength(100)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(e => e.ShortDescription)
               .HasMaxLength(5000)
               .IsRequired();

        builder.HasOne(d => d.TopicRank)
               .WithMany(p => p.RankAwards)
               .HasForeignKey(d => d.TopicRankId)
               .HasConstraintName("FK_RankAward_TopicRank")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
