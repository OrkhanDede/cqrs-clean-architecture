using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class RolePermissionCategoryConfiguration : IEntityTypeConfiguration<RolePermissionCategory>
    {
        public void Configure(EntityTypeBuilder<RolePermissionCategory> modelBuilder)
        {
            modelBuilder.HasKey(table => new
            {
                table.RoleId,
                table.PermissionCategoryPermissionId
            });
            modelBuilder.Property(x => x.RoleId).IsRequired().HasMaxLength(128);

        }
    }
}
