using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Constants;
using Core.Enums;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;

namespace Application.Commands.UserCommands.SetUserPermission
{
    public class SetUserPermissionCommandHandler : ICommandHandler<SetUserPermissionCommand, SetUserPermissionResponse>
    {
        private readonly IUserRepository _repository;
        private readonly AuthService _authService;
        private readonly ExceptionService _exceptionService;
        public SetUserPermissionCommandHandler(IUserRepository repository, AuthService authService, ExceptionService exceptionService)
        {
            _repository = repository;
            _authService = authService;
            _exceptionService = exceptionService;
        }
        public async Task<SetUserPermissionResponse> Handle(SetUserPermissionCommand command, CancellationToken cancellationToken)
        {

            var isAdmin = await _authService.IsAdminAsync();
            if (!isAdmin)
                command.Request.PermissionIds = new List<string>();

            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            _repository.SetGlobalQueryFilterStatus(false);
            var user = await _repository.GetUserByIdAsync(command.Request.UserId, includeParams.ToArray());
            if (user == null)
                throw _exceptionService.RecordNotFoundException();


            var removedPermissions = user.DirectivePermissions.Where(f => !command.Request.PermissionIds.Contains(f.PermissionId)).ToList();
            foreach (var item in removedPermissions)
            {
                user.DirectivePermissions.Remove(item);
            }

            if (command.Request.PermissionIds != null && command.Request.PermissionIds.Any())
            {
                var alreadyExistPermissions = user.DirectivePermissions.Where(c => command.Request.PermissionIds.Contains(c.PermissionId)).ToList();
                foreach (var permission in alreadyExistPermissions)
                {
                    permission.Status = RecordStatusEnum.Active;
                }
                var addedPermissions = command.Request.PermissionIds.Where(id => !alreadyExistPermissions.Select(e => e.PermissionId).Contains(id) && user.DirectivePermissions.All(f => f.PermissionId != id)).Select(c => new UserPermission() { PermissionId = c, DateCreated = DateTime.Now }).ToList();
                foreach (var item in addedPermissions)
                {
                    user.DirectivePermissions.Add(item);
                }
            }
            await _repository.UpdateAsync(user).ConfigureAwait(false);
            return new SetUserPermissionResponse();
        }
    }
}
