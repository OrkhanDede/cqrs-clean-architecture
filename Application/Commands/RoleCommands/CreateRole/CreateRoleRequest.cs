using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.RoleCommands.CreateRole
{
    public class CreateRoleRequest
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public List<int> PermissionIds { get; set; }
    }
}
