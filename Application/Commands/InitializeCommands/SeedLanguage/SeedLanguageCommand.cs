using Infrastructure.Configurations.Commands;

namespace Application.Commands.InitializeCommands.SeedLanguage
{
    public class SeedLanguageCommand : CommandBase<SeedLanguageResponse>
    {
        public SeedLanguageCommand(SeedLanguageRequest request)
        {
            Request = request;
        }

        public SeedLanguageRequest Request { get; set; }
    }
}
