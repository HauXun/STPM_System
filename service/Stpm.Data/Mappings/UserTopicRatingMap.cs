using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class UserTopicRatingMap : IEntityTypeConfiguration<UserTopicRating>
{
    public void Configure(EntityTypeBuilder<UserTopicRating> builder)
    {
        builder.HasKey(e => new { e.UserId, e.TopicId })
               .HasName("PK_UserTopicRating");

        builder.ToTable("UserTopicRating");

        builder.HasOne(d => d.Topic)
               .WithMany(p => p.UserTopicRatings)
               .HasForeignKey(d => d.TopicId)
               .HasConstraintName("FK_UserTopicRating_Topic")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.User)
               .WithMany(p => p.UserTopicRatings)
               .HasForeignKey(d => d.UserId)
               .HasConstraintName("FK_UserTopicRating_Users")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
