using Infrastructure.Configurations.Commands;

namespace Application.Commands.UserCommands.CreateUser
{
    public class CreateUserCommand : CommandBase<CreateUserResponse>
    {
        public CreateUserCommand(CreateUserRequest request)
        {
            Request = request;
        }

        public CreateUserRequest Request { get; set; }
    }
}
