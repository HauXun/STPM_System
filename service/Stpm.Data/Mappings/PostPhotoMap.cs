using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class PostPhotoMap : IEntityTypeConfiguration<PostPhoto>
{
    public void Configure(EntityTypeBuilder<PostPhoto> builder)
    {
        builder.ToTable("PostPhoto");

        builder.Property(e => e.ImageUrl)
               .HasMaxLength(1000)
               .IsRequired();

        builder.HasOne(d => d.Post).WithMany(p => p.PostPhotos)
               .HasForeignKey(d => d.PostId)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK_PostPhoto_Post");
    }
}
