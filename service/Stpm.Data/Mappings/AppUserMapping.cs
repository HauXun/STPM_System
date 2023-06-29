using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

internal class AppUserMapping : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(e => e.FullName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.JoinedDate)
               .HasColumnType("smalldatetime")
               .IsRequired();

        builder.Property(e => e.UrlSlug)
               .HasMaxLength(200)
               .IsRequired()
               .IsUnicode(false);

        builder.Property(e => e.ImageUrl)
               .HasMaxLength(1000);

        builder.Property(e => e.MSSV)
               .HasMaxLength(50);

        builder.Property(e => e.GradeName)
               .HasMaxLength(50);

        builder.HasMany(e => e.UserNotifies)
               .WithOne(e => e.User)
               .HasForeignKey(e => e.UserId);

        builder.HasMany(e => e.UserTopicRatings)
               .WithOne(e => e.User)
               .HasForeignKey(e => e.UserId);

        builder.HasMany(d => d.Topics)
               .WithMany(p => p.Users)
               .UsingEntity<Dictionary<string, object>>(
                   "UserTopicRegis",
                   r => r.HasOne<Topic>()
                         .WithMany()
                         .HasForeignKey("TopicId")
                         .HasConstraintName("FK_UserTopicRegis_Topic")
                         .OnDelete(DeleteBehavior.Cascade),
                   l => l.HasOne<AppUser>()
                         .WithMany()
                         .HasForeignKey("UserId")
                         .HasConstraintName("FK_UserTopicRegis_Users")
                         .OnDelete(DeleteBehavior.Cascade),
                   j =>
                   {
                       j.HasKey("UserId", "TopicId")
                        .HasName("PK_UserTopic");
                       j.ToTable("UserTopicRegis");
                       j.HasIndex(new[] { "TopicId" }, "IX_UserTopicRegis_TopicId");
                   });
    }
}
