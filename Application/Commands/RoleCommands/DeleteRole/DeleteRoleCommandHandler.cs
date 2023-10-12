using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repository.RoleRepository;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;

namespace Application.Commands.RoleCommands.DeleteRole
{
    public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, DeleteRoleResponse>
    {
        private readonly IRoleRepository _repository;
        private readonly ExceptionService _exceptionService;

        public DeleteRoleCommandHandler(IRoleRepository repository , ExceptionService exceptionService)
        {
            _repository = repository;
            _exceptionService = exceptionService;
        }
        public async Task<DeleteRoleResponse> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _repository.GetRoleByIdAsync(command.Request.Id).ConfigureAwait(false);
            if (role == null)
                throw _exceptionService.RecordNotFoundException();

            await _repository.DeleteAsync(role).ConfigureAwait(false);

            return new DeleteRoleResponse();

        }
    }
}
