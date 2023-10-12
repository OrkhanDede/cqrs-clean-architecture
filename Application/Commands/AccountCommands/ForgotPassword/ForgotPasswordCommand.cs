using Infrastructure.Configurations.Commands;

namespace Application.Commands.AccountCommands.ForgotPassword
{
    public class ForgotPasswordCommand : CommandBase<ForgotPasswordResponse>
    {
        public ForgotPasswordCommand(ForgotPasswordRequest request)
        {
            Request = request;
        }
        public ForgotPasswordRequest Request { get; set; }
    }
}
