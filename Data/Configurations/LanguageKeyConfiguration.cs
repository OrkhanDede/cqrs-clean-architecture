using Domain.Entities.Lang;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class LanguageKeyConfiguration : IEntityTypeConfiguration<LanguageKey>
    {
        public void Configure(EntityTypeBuilder<LanguageKey> modelBuilder)
        {
            modelBuilder.HasKey(ul => new { ul.LanguageId, ul.KeyId });

        }
    }
}
