using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

internal class NotificationMap : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notification");

        builder.Property(e => e.Content)
               .IsRequired();

        builder.Property(e => e.LevelId)
               .HasMaxLength(450)
               .IsRequired();

        builder.Property(e => e.DueDate)
               .HasColumnType("smalldatetime");

        builder.Property(e => e.Title)
               .HasMaxLength(200)
               .IsRequired();

        builder.HasOne(d => d.Level)
               .WithMany(p => p.Notifications)
               .HasForeignKey(d => d.LevelId)
               .HasConstraintName("FK_Notification_NotiLevel")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
