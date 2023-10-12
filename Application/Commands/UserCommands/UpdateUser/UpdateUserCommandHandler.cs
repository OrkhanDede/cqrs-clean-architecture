using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.UserCommands.SetUserPermission;
using Application.Commands.UserCommands.SetUserRole;
using AutoMapper;
using Core.Constants;
using Core.Enums;
using DataAccess.Repository.UserRepository;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;
using MediatR;

namespace Application.Commands.UserCommands.UpdateUser
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;

        public UpdateUserCommandHandler(AuthService authService, IMapper mapper, IUserRepository userRepository , IMediator mediator , ExceptionService exceptionService)
        {
            _authService = authService;
            _mapper = mapper;
            _userRepository = userRepository;
            _mediator = mediator;
            _exceptionService = exceptionService;
        }
        public async Task<UpdateUserResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var userId = _authService.GetAuthorizedUserId();
            var haveRoleSetPermission = await _authService.UserIsInPermissionAsync(userId, $"role_{PermissionEnum.Set}");
            var haveRoleListPermission = await _authService.UserIsInPermissionAsync(userId, $"role_{PermissionEnum.List}");
            var isAdmin = await _authService.IsAdminAsync();
            var isAuthUserAdmin = await _authService.IsAdminAsync();

            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            var user = await _userRepository.GetUserByIdAsync(command.Request.Id, includeParams.ToArray()).ConfigureAwait(false);
            if (user == null)
                throw _exceptionService.RecordNotFoundException();

            if (!((haveRoleSetPermission && haveRoleListPermission) | isAdmin))
                command.Request.Roles = new List<string>();

            if (!isAuthUserAdmin)
                command.Request.DirectivePermissions = user.DirectivePermissions.Select(c => c.PermissionId).ToList();

            //update
            _mapper.Map(command.Request, user);

            await _mediator.Send(new SetUserRoleCommand(new SetUserRoleRequest()
            {
                UserId = user.Id,
                RoleIds = command.Request.Roles
            }), cancellationToken);
            await _mediator.Send(new SetUserPermissionCommand(new SetUserPermissionRequest()
            {
                PermissionIds = command.Request.DirectivePermissions,
                UserId = user.Id
            }), cancellationToken);

            await _userRepository.UpdateAsync(user).ConfigureAwait(false);

            return new UpdateUserResponse()
            {
                Response = user
            };

        }
    }
}
