using Infrastructure.Configurations.Commands;

namespace Application.Commands.RoleCommands.UpdateRole
{
    public class UpdateRoleCommand : CommandBase<UpdateRoleResponse>
    {
        public UpdateRoleCommand(UpdateRoleRequest request)
        {
            Request = request;
        }

        public UpdateRoleRequest Request { get; set; }
    }
}
