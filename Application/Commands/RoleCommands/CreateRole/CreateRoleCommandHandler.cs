using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Repository.RoleRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;

namespace Application.Commands.RoleCommands.CreateRole
{
    public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, CreateRoleResponse>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ExceptionService _exceptionService;

        public CreateRoleCommandHandler(IRoleRepository repository, IMapper mapper , ExceptionService exceptionService)
        {
            _repository = repository;
            _mapper = mapper;
            _exceptionService = exceptionService;
        }
        public async Task<CreateRoleResponse> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var isExistAsync =
                await _repository.IsExistAsync(c => c.Name == command.Request.Name).ConfigureAwait(false);
            if (isExistAsync)
                throw _exceptionService.RecordNotFoundException();

            var role = _mapper.Map<CreateRoleRequest, Role>(command.Request);
            role.Id = Guid.NewGuid().ToString();
            await _repository.CreateAsync(role).ConfigureAwait(false);
            return new CreateRoleResponse()
            {
                Response = role
            };
        }
    }
}
