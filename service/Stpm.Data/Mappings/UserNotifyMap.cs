using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class UserNotifyMap : IEntityTypeConfiguration<UserNotify>
{
    public void Configure(EntityTypeBuilder<UserNotify> builder)
    {
        builder.HasKey(e => new { e.UserId, e.NotifyId })
               .HasName("PK_UserNoti");

        builder.ToTable("UserNotify");

        builder.Property(p => p.Viewed)
               .IsRequired()
               .HasDefaultValue(false);

        builder.HasOne(d => d.Notify)
               .WithMany(p => p.UserNotifies)
               .HasForeignKey(d => d.NotifyId)
               .HasConstraintName("FK_UserNotify_Notification")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.User)
               .WithMany(p => p.UserNotifies)
               .HasForeignKey(d => d.UserId)
               .HasConstraintName("FK_UserNotify_Users")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
