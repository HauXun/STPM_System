using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class PostVideoMap : IEntityTypeConfiguration<PostVideo>
{
    public void Configure(EntityTypeBuilder<PostVideo> builder)
    {
        builder.ToTable("PostVideo");

        builder.Property(e => e.VideoUrl)
               .HasMaxLength(1000)
               .IsRequired();

        builder.HasOne(d => d.Post)
               .WithMany(p => p.PostVideos)
               .HasForeignKey(d => d.PostId)
               .HasConstraintName("FK_PostVideo_Post")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
