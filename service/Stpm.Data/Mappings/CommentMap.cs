using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class CommentMap : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comment");

        builder.Property(e => e.Content)
               .IsRequired();

        builder.Property(e => e.Date)
               .IsRequired()
               .HasColumnType("smalldatetime");

        builder.Property(e => e.ModifiedDate)
               .HasColumnType("smalldatetime");

        builder.Property(e => e.UserId)
               .HasMaxLength(450)
               .IsRequired();

        builder.HasOne(d => d.User)
               .WithMany(p => p.Comments)
               .HasForeignKey(d => d.UserId)
               .HasConstraintName("FK_Comment_Users")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
