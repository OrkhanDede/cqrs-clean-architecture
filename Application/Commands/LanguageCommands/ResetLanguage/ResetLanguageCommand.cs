using Infrastructure.Configurations.Commands;

namespace Application.Commands.LanguageCommands.ResetLanguage
{
    public class ResetLanguageCommand : CommandBase<ResetLanguageResponse>
    {
        public ResetLanguageCommand(ResetLanguageRequest request)
        {
            Request = request;
        }

        public ResetLanguageRequest Request { get; set; }
    }
}
