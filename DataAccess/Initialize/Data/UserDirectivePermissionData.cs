using System.Collections.Generic;
using System.Linq;
using Data;
using Domain.Entities.Identity;

namespace DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {
        public static List<UserPermission> BuildUserDirectivePermissionsList(ApplicationDbContext context)
        {
            var list = context.Permissions.Where(c => c.IsDirective);
            return list.Select(c => new UserPermission()
            {
                PermissionId = c.Label
            }).ToList();

        }

    }
}
