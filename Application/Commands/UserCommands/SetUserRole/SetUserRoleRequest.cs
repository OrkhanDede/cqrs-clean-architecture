using System.Collections.Generic;

namespace Application.Commands.UserCommands.SetUserRole
{
    public class SetUserRoleRequest
    {
        public string UserId { get; set; }
        public List<string> RoleIds { get; set; }
    }
}
