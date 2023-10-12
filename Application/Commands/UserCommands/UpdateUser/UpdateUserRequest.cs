using System.Collections.Generic;

namespace Application.Commands.UserCommands.UpdateUser
{
    public class UpdateUserRequest
    {


        public string Id { get; set; }
        public List<string> Roles { get; set; }
        public List<string> DirectivePermissions { get; set; }
    }
}
