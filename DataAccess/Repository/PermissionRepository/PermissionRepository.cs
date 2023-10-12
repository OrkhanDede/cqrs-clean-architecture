using Data;
using Domain.Entities.Identity;

namespace DataAccess.Repository.PermissionRepository
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository, IRepositoryIdentifier
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
