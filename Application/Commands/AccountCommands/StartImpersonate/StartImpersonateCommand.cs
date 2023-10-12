using Infrastructure.Configurations.Commands;

namespace Application.Commands.AccountCommands.StartImpersonate
{
    public class StartImpersonateCommand : CommandBase<StartImpersonateResponse>
    {
        public StartImpersonateCommand(StartImpersonateRequest request)
        {
            Request = request;
        }

        public StartImpersonateRequest Request { get; set; }
    }
}
