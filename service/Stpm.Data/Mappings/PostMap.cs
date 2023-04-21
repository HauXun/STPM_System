using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");

        builder.Property(p => p.Published)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(e => e.ShortDescription)
               .IsRequired();

        builder.Property(e => e.Description)
               .IsRequired();

        builder.Property(e => e.Meta)
               .HasMaxLength(1000)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(e => e.ModifiedDate)
               .HasColumnType("smalldatetime");

        builder.Property(e => e.PostedDate)
               .HasColumnType("smalldatetime")
               .IsRequired();

        builder.Property(e => e.Title)
               .HasMaxLength(1000)
               .IsRequired();

        builder.Property(e => e.UrlSlug)
               .HasMaxLength(1000)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(e => e.UserId)
               .HasMaxLength(450)
               .IsRequired();

        builder.HasOne(d => d.Topic)
               .WithMany(p => p.Posts)
               .HasForeignKey(d => d.TopicId)
               .HasConstraintName("FK_Post_Topic")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.User)
               .WithMany(p => p.Posts)
               .HasForeignKey(d => d.UserId)
               .HasConstraintName("FK_Post_Users")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Comments)
               .WithMany(p => p.Posts)
               .UsingEntity<Dictionary<string, object>>(
                   "PostComment",
                   r => r.HasOne<Comment>()
                         .WithMany()
                         .HasForeignKey("CommentId")
                         .HasConstraintName("FK_PostComment_Comment")
                         .OnDelete(DeleteBehavior.Cascade),
                   l => l.HasOne<Post>()
                         .WithMany()
                         .HasForeignKey("PostId")
                         .OnDelete(DeleteBehavior.NoAction)
                         .HasConstraintName("FK_PostComment_Post"),
                   j =>
                   {
                       j.HasKey("PostId", "CommentId");
                       j.ToTable("PostComment");
                   });

        builder.HasMany(d => d.Tags)
               .WithMany(p => p.Posts)
               .UsingEntity<Dictionary<string, object>>(
                   "PostTag",
                   r => r.HasOne<Tag>()
                         .WithMany()
                         .HasForeignKey("TagId")
                         .HasConstraintName("FK_PostTag_Tags")
                         .OnDelete(DeleteBehavior.Cascade),
                   l => l.HasOne<Post>()
                         .WithMany()
                         .HasForeignKey("PostId")
                         .HasConstraintName("FK_PostTag_Post")
                         .OnDelete(DeleteBehavior.Cascade),
                   j =>
                   {
                       j.HasKey("PostId", "TagId");
                       j.ToTable("PostTag");
                   });
    }
}
