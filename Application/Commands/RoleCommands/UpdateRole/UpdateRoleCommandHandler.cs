using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Constants;
using DataAccess.Repository.RoleRepository;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;

namespace Application.Commands.RoleCommands.UpdateRole
{
    public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, UpdateRoleResponse>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ExceptionService _exceptionService;

        public UpdateRoleCommandHandler(IRoleRepository repository, IMapper mapper , ExceptionService exceptionService)
        {
            _repository = repository;
            _mapper = mapper;
            _exceptionService = exceptionService;
        }
        public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _repository.GetRoleByIdAsync(command.Request.Id, new IncludeStringConstants().RolePermissionIncludeList.ToArray()).ConfigureAwait(false);
            if (role == null)
                throw _exceptionService.RecordNotFoundException();

            var isExistAsync = await _repository.IsExistAsync(c => c.Name == command.Request.Name).ConfigureAwait(false) && command.Request.Name != role.Name;
            if (isExistAsync)
                throw _exceptionService.RecordAlreadyExistException();

            //update
            _mapper.Map(command.Request, role);
            await _repository.UpdateAsync(role).ConfigureAwait(false);

            return new UpdateRoleResponse()
            {
                Response = role
            };
        }
    }
}
