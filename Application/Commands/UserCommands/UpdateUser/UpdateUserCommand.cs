using Infrastructure.Configurations.Commands;

namespace Application.Commands.UserCommands.UpdateUser
{
    public class UpdateUserCommand : CommandBase<UpdateUserResponse>

    {
        public UpdateUserCommand(UpdateUserRequest request)
        {
            Request = request;
        }

        public UpdateUserRequest Request { get; set; }
    }
}
