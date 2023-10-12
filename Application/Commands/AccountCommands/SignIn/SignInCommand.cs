using Infrastructure.Configurations.Queries;

namespace Application.Commands.AccountCommands.SignIn
{
    public class SignInCommand : IQuery<SignInResponse>
    {
        public SignInCommand(SignInRequest request)
        {
            Request = request;
        }

        public SignInRequest Request { get; set; }
    }
}
