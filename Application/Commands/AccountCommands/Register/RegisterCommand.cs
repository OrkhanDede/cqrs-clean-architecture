using Infrastructure.Configurations;
using Infrastructure.Configurations.Commands;

namespace Application.Commands.AccountCommands.Register
{
    public class RegisterCommand : CommandBase<RegisterResponse>, ITransactionalRequest<RegisterResponse>
    {
        public RegisterCommand(RegisterRequest request)
        {
            Request = request;
        }

        public RegisterRequest Request { get; set; }
    }
}
