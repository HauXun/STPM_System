using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class TopicHistoryAwardMap : IEntityTypeConfiguration<TopicHistoryAward>
{
    public void Configure(EntityTypeBuilder<TopicHistoryAward> builder)
    {
        builder.ToTable("TopicHistoryAward");

        builder.Property(e => e.TopicRank)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(e => e.UrlSlug)
               .HasMaxLength(200)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(e => e.TopicAward)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(e => e.TopicName)
               .HasMaxLength(200)
               .IsRequired();
    }
}
