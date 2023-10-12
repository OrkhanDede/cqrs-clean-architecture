using Data;
using Domain.Entities.Identity;

namespace DataAccess.Repository.PermissionRepository
{
    public class PermissionCategoryPermissionRepository : Repository<PermissionCategoryPermission>, IPermissionCategoryPermissionRepository, IRepositoryIdentifier
    {
        public PermissionCategoryPermissionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
