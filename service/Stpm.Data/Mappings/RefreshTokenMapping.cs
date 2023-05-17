using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Stpm.Core.Entities;

namespace Stpm.Data.Mappings;

public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Token)
               .IsRequired();

        builder.Property(e => e.JwtId)
               .IsRequired();

        builder.Property(p => p.IsUsed)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(p => p.IsRevoked)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(e => e.ExpiredAt)
               .HasColumnType("smalldatetime");

        builder.Property(e => e.IssuedAt)
               .HasColumnType("smalldatetime")
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.HasOne(d => d.User)
               .WithMany(p => p.RefreshTokens)
               .HasForeignKey(d => d.UserId)
               .HasConstraintName("FK_RefreshTokens_Users")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
