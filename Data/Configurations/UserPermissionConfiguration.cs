using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> modelBuilder)
        {
            modelBuilder.HasKey(table => new
            {
                table.UserId,
                table.PermissionId
            });

        }
    }
}
