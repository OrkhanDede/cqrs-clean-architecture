using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repository.LanguageRepository;
using Infrastructure.Configurations.Commands;

namespace Application.Commands.InitializeCommands.SeedLanguage
{
    public class SeedLanguageCommandHandler : ICommandHandler<SeedLanguageCommand, SeedLanguageResponse>
    {
        private readonly ILanguageRepository _languageRepository;

        public SeedLanguageCommandHandler(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }
        public async Task<SeedLanguageResponse> Handle(SeedLanguageCommand command, CancellationToken cancellationToken)
        {
            await _languageRepository.ResetAsync().ConfigureAwait(false);
            return new SeedLanguageResponse();
        }
    }
}
