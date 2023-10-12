using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repository;
using DataAccess.Repository.LanguageRepository;
using Infrastructure.Configurations.Commands;

namespace Application.Commands.LanguageCommands.SetLanguageKeyValue
{
    public class SetLanguageKeyValueCommandHandler : ICommandHandler<SetLanguageKeyValueCommand, SetLanguageKeyValueResponse>
    {
        private readonly ILanguageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SetLanguageKeyValueCommandHandler(ILanguageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<SetLanguageKeyValueResponse> Handle(SetLanguageKeyValueCommand command, CancellationToken cancellationToken)
        {
            await _repository.SetValueAsync(command.Request.LanguageId, command.Request.KeyId, command.Request.Value).ConfigureAwait(false);
            await _unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);
            return new SetLanguageKeyValueResponse();
        }
    }
}
