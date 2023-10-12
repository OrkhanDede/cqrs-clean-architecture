using System.Threading.Tasks;
using Data;

namespace DataAccess.Repository.AccessLogRepository
{
    public class AccessLogRepository : IAccessLogRepository, IRepositoryIdentifier
    {
        private readonly ApplicationDbContext _context;

        public AccessLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.Entities.Identity.AccessLog entity)
        {
            await _context.AddAsync(entity).ConfigureAwait(false);
        }
    }
}
