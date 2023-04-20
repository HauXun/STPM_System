using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class NotifyAttachmentMap : IEntityTypeConfiguration<NotifyAttachment>
{
    public void Configure(EntityTypeBuilder<NotifyAttachment> builder)
    {
        builder.ToTable("NotifyAttachment");

        builder.Property(e => e.AttachmentUrl)
               .HasMaxLength(1000)
               .IsRequired();

        builder.HasOne(d => d.Notify)
               .WithMany(p => p.NotifyAttachments)
               .HasForeignKey(d => d.NotifyId)
               .HasConstraintName("FK_NotifyAttachment_Notification")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
