using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class TopicMap : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("Topic");

        builder.Property(e => e.UrlSlug)
               .HasMaxLength(200)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(e => e.OutlineUrl)
               .HasMaxLength(1000)
               .IsUnicode(false);

        builder.Property(e => e.RegisDate)
               .HasColumnType("smalldatetime")
               .IsRequired();

        builder.Property(e => e.TopicName)
               .HasMaxLength(200)
               .IsRequired();

        builder.HasOne(d => d.SpecificAward)
               .WithMany(p => p.Topics)
               .HasForeignKey(d => d.SpecificAwardId)
               .HasConstraintName("FK_Topic_SpecificAward");

        builder.HasOne(d => d.TopicRank)
               .WithMany(p => p.Topics)
               .HasForeignKey(d => d.TopicRankId)
               .HasConstraintName("FK_Topic_TopicRank")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Comments)
               .WithMany(p => p.Topics)
               .UsingEntity<Dictionary<string, object>>(
               "TopicComment",
               r => r.HasOne<Comment>()
                     .WithMany()
                     .HasForeignKey("CommentId")
                     .HasConstraintName("FK_TopicComment_Comment")
                     .OnDelete(DeleteBehavior.Cascade),
               l => l.HasOne<Topic>()
                     .WithMany()
                     .HasForeignKey("TopicId")
                     .HasConstraintName("FK_TopicComment_Topic")
                     .OnDelete(DeleteBehavior.Cascade),
               j =>
               {
                   j.HasKey("TopicId", "CommentId");
                   j.ToTable("TopicComment");
               });
    }
}
