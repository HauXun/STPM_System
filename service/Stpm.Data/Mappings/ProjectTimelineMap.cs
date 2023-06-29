using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class ProjectTimelineMap : IEntityTypeConfiguration<ProjectTimeline>
{
    public void Configure(EntityTypeBuilder<ProjectTimeline> builder)
    {
        builder.ToTable("ProjectTimeline");

        builder.Property(p => p.ShowOn)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(e => e.ShortDescription)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.Title)
               .HasMaxLength(200)
               .IsRequired();
    }
}
