using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Constants;
using Core.Models;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Middlewares
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public readonly PermissionRequirementModel[] Permission;

        public PermissionRequirement(params PermissionRequirementModel[] permission)
        {
            Permission = permission;
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;

        public PermissionHandler(AuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //var userId = _userService.GetAuthorizedUserId(context.User);
            var isAdmin = await _authService.IsAdminAsync().ConfigureAwait(false);
            if (!isAdmin)
            {
                var userId = _authService.GetAuthorizedUserId();
                var user = await _userRepository.GetUserByIdAsync(userId, new IncludeStringConstants().UserRolePermissionIncludeArray);
                if (user != null)
                {
                    var userRole = new List<Role>();
                    foreach (var c in user.Roles) userRole.Add(c.Role);

                    var rolePermissions = userRole.SelectMany(c => c.PermissionCategory.Select(m => m.PermissionCategoryPermission)).ToList();

                    var directivePermissions = user.DirectivePermissions.Select(c => c.Permission);


                    if (
                        directivePermissions.Any(dp => requirement.Permission.Any(req => req.IsEqual(dp.Label)))
                        ||
                        rolePermissions.Any(rp =>
                            requirement.Permission.Any(req => req.IsEqual(rp.PermissionId, rp.CategoryId)))
                    )
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            else
            {
                context.Succeed(requirement);
            }


            await Task.CompletedTask.ConfigureAwait(false);

        }
    }

}
