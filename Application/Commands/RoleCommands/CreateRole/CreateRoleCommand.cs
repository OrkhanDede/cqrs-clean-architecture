using Infrastructure.Configurations.Commands;

namespace Application.Commands.RoleCommands.CreateRole
{
    public class CreateRoleCommand : CommandBase<CreateRoleResponse>

    {
        public CreateRoleCommand(CreateRoleRequest request)
        {
            Request = request;
        }

        public CreateRoleRequest Request { get; set; }
    }
}
