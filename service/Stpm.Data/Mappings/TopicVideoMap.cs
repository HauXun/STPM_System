using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class TopicVideoMap : IEntityTypeConfiguration<TopicVideo>
{
    public void Configure(EntityTypeBuilder<TopicVideo> builder)
    {
        builder.ToTable("TopicVideo");

        builder.Property(e => e.VideoUrl)
               .HasMaxLength(1000)
               .IsRequired();

        builder.HasOne(d => d.Topic)
               .WithMany(p => p.TopicVideos)
               .HasForeignKey(d => d.TopicId)
               .HasConstraintName("FK_TopicVideo_Topic")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
