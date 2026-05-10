using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noname.Domain.Entities;
using Noname.Infrastructure.Identity;

namespace Noname.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(t => t.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(t => t.LastName).HasMaxLength(100).IsRequired();

        builder.HasOne<AppUser>()
            .WithOne(u => u.Member)
            .HasForeignKey<Member>(m => m.IdentityId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
