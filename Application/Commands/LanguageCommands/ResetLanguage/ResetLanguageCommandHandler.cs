using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repository.LanguageRepository;
using Infrastructure.Configurations.Commands;

namespace Application.Commands.LanguageCommands.ResetLanguage
{
    public class ResetLanguageCommandHandler : ICommandHandler<ResetLanguageCommand, ResetLanguageResponse>
    {
        private readonly ILanguageRepository _repository;

        public ResetLanguageCommandHandler(ILanguageRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResetLanguageResponse> Handle(ResetLanguageCommand command, CancellationToken cancellationToken)
        {
            await _repository.ResetAsync();
            return new ResetLanguageResponse();
        }
    }
}
