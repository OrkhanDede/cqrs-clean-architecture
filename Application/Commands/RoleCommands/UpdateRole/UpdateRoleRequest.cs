using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.RoleCommands.UpdateRole
{
    public class UpdateRoleRequest
    {


        public string Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> PermissionCategories { get; set; }
    }
}
