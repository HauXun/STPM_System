using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class TimelineMap : IEntityTypeConfiguration<Timeline>
{
    public void Configure(EntityTypeBuilder<Timeline> builder)
    {
        builder.ToTable("Timeline");

        builder.Property(e => e.DueDate)
               .HasColumnType("smalldatetime")
               .IsRequired();

        builder.Property(e => e.ShortDescription)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.Title)
               .HasMaxLength(200)
               .IsRequired();

        builder.HasOne(d => d.Project)
               .WithMany(p => p.Timelines)
               .HasForeignKey(d => d.ProjectId)
               .HasConstraintName("FK_Timeline_ProjectTimeline")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Notifies)
               .WithMany(p => p.Timelines)
               .UsingEntity<Dictionary<string, object>>(
                   "TimelineNotification",
                   r => r.HasOne<Notification>()
                         .WithMany()
                         .HasForeignKey("NotifyId")
                         .HasConstraintName("FK_TimelineNotifications_Notification")
                         .OnDelete(DeleteBehavior.Cascade),
                   l => l.HasOne<Timeline>()
                         .WithMany()
                         .HasForeignKey("TimelineId")
                         .HasConstraintName("FK_TimelineNotifications_Timeline")
                         .OnDelete(DeleteBehavior.Cascade),
                   j =>
                   {
                       j.HasKey("TimelineId", "NotifyId");
                       j.ToTable("TimelineNotifications");
                   });
    }
}
