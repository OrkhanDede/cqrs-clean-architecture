using Data;
using Domain.Entities.Lang;

namespace DataAccess.Repository.LanguageRepository
{
    public class KeyRepository : Repository<Key>, IKeyRepository, IRepositoryIdentifier
    {
        public KeyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
