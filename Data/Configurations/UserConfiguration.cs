using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.Property(x => x.Id).IsRequired().HasMaxLength(128);
            modelBuilder.Property(x => x.UserName).IsRequired().HasMaxLength(30);
        }
    }
}
