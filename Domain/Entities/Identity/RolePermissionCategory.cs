using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Configurations;

namespace Domain.Entities.Identity
{
    public class RolePermissionCategory : Entity
    {
        [ForeignKey("PermissionCategoryPermission")]
        public int PermissionCategoryPermissionId { get; set; }
        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public PermissionCategoryPermission PermissionCategoryPermission { get; set; }
        public Role Role { get; set; }
    }
}
