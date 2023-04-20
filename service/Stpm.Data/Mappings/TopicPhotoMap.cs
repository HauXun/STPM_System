using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class TopicPhotoMap : IEntityTypeConfiguration<TopicPhoto>
{
    public void Configure(EntityTypeBuilder<TopicPhoto> builder)
    {
        builder.ToTable("TopicPhoto");

        builder.Property(e => e.ImageUrl)
               .HasMaxLength(1000)
               .IsRequired();

        builder.HasOne(d => d.Topic)
               .WithMany(p => p.TopicPhotos)
               .HasForeignKey(d => d.TopicId)
               .HasConstraintName("FK_TopicPhoto_Topic")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
