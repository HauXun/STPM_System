using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class TopicRankMap : IEntityTypeConfiguration<TopicRank>
{
    public void Configure(EntityTypeBuilder<TopicRank> builder)
    {
        builder.ToTable("TopicRank");

        builder.Property(e => e.RankName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.UrlSlug)
               .HasMaxLength(100)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(e => e.ShortDescription)
               .HasMaxLength(5000)
               .IsRequired();
    }
}
