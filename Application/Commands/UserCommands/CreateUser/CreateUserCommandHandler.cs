using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.UserCommands.SetUserPermission;
using Application.Commands.UserCommands.SetUserRole;
using AutoMapper;
using AutoWrapper.Wrappers;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.UserCommands.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;

        public CreateUserCommandHandler(AuthService authService, IMapper mapper, IUserRepository userRepository , IMediator mediator , ExceptionService exceptionService)
        {
            _authService = authService;
            _mapper = mapper;
            _userRepository = userRepository;
            _mediator = mediator;
            _exceptionService = exceptionService;
        }
        public async Task<CreateUserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {


            var isExistAsync = await _userRepository
                .IsExistAsync(c => c.UserName.ToLower() == command.Request.UserName.ToLower() || c.Email.ToLower() == command.Request.Email.ToLower())
                .ConfigureAwait(false);

            if (isExistAsync)
                throw _exceptionService.RecordAlreadyExistException();

            var user = _mapper.Map<CreateUserRequest, User>(command.Request);
            user.Id = Guid.NewGuid().ToString();
            user.EmailConfirmed = true;

            var result = await _userRepository.CreateAsync(user, command.Request.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.FirstOrDefault();
                throw new ApiException(errorMessage, StatusCodes.Status409Conflict);
            }

            if (result.Succeeded)
            {
                await _mediator.Send(new SetUserRoleCommand(new SetUserRoleRequest()
                {
                    RoleIds = command.Request.Roles,
                    UserId = user.Id
                }), cancellationToken);
                await _mediator.Send(new SetUserPermissionCommand(new SetUserPermissionRequest()
                {
                    PermissionIds = command.Request.DirectivePermissions,
                    UserId = user.Id
                }), cancellationToken);
            }
            return new CreateUserResponse()
            {
                Response = user
            };



        }
    }
}
