using Infrastructure.Configurations.Commands;

namespace Application.Commands.UserCommands.DeleteUser
{
    public class DeleteUserCommand : CommandBase<DeleteUserResponse>
    {
        public DeleteUserCommand(DeleteUserRequest request)
        {
            Request = request;
        }

        public DeleteUserRequest Request { get; set; }

    }
}
