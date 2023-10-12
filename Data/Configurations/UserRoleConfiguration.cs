using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> modelBuilder)
        {
            modelBuilder.HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.HasOne(ur => ur.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            modelBuilder.HasOne(ur => ur.User)
                .WithMany(r => r.Roles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Property(x => x.RoleId).IsRequired().HasMaxLength(128);
            modelBuilder.Property(x => x.UserId).IsRequired().HasMaxLength(128);

        }
    }
}
