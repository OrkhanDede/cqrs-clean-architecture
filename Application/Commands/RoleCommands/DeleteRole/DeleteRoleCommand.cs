using Infrastructure.Configurations.Commands;

namespace Application.Commands.RoleCommands.DeleteRole
{
    public class DeleteRoleCommand : CommandBase<DeleteRoleResponse>
    {
        public DeleteRoleCommand(DeleteRoleRequest request)
        {
            Request = request;
        }

        public DeleteRoleRequest Request { get; set; }
    }
}
