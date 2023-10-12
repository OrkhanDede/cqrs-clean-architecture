using Infrastructure.Configurations.Commands;

namespace Application.Commands.AccountCommands.SignOutUser
{
    public class SignOutUserCommand : CommandBase<SignOutUserResponse>
    {
        public SignOutUserCommand(SignOutUserRequest request)
        {
            Request = request;
        }
        public SignOutUserRequest Request { get; set; }
    }
}
