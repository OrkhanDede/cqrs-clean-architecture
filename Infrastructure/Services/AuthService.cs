using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Constants;
using Core.Enums;
using Core.Models;
using Core.Utilities;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IMediator mediator, IUserRepository userRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetAuthorizedUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<List<Role>> GetUserRolesAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, "Roles.Role");
            return user.Roles.Select(c => c.Role).ToList();
        }
        public List<Claim> GetClaims()
        {
            var claims = _httpContextAccessor.HttpContext?.User.Claims.ToList();
            return claims;
        }
        public List<Claim> GetCustomClaims()
        {
            var claims = GetClaims();
            var result = claims?.Where(c => CustomClaimTypeConstants.Types.Contains(c.Type)).ToList();
            return result;
        }
        public TokenClaim GetTokenClaim()
        {
            var claims = GetCustomClaims();
            return new TokenClaim()
            {
                ImpersonatorId = claims.FirstOrDefault(c => c.Type == CustomClaimTypeConstants.Impersonator),
                ImpersonatorName = claims.FirstOrDefault(c => c.Type == CustomClaimTypeConstants.ImpersonatorName),
                RememberMe =
                    claims.FirstOrDefault(c => c.Type == CustomClaimTypeConstants.RememberMe),
            };
        }

        public async Task<bool> IsAdminAsync()
        {
            var userId = GetAuthorizedUserId();
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            return UserIsInPermission(await _userRepository.GetUserByIdAsync(userId, includeParams.ToArray()), nameof(PermissionEnum.Admin));
        }
        public async Task<bool> IsAdminAsync(string userId)

        {
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            return UserIsInPermission(await _userRepository.GetUserByIdAsync(userId, includeParams.ToArray()), nameof(PermissionEnum.Admin));
        }
        public async Task<bool> UserIsInRoleAsync(User user, string roleName)
        {
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            return roles.Any(s => s.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public bool UserIsInPermission(User user, string permissionName)
        {
            var directivePermissions = user.DirectivePermissions.Select(c => c.Permission.Label).ToList();
            var userRole = user.Roles.Select(c => c.Role).ToList();
            var permissions = userRole.SelectMany(c => c.PermissionCategory.Select(e => e.PermissionCategoryPermission.Permission.Label)).ToList();

            return directivePermissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase)) ||
                   permissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase));


        }
        public async Task<bool> UserIsInPermissionAsync(string userId, string permissionName)
        {
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            var user = await _userRepository.GetUserByIdAsync(userId, includeParams.ToArray()).ConfigureAwait(false);
            var directivePermissions = user.DirectivePermissions.Select(c => c.Permission.Label).ToList();
            var userRole = user.Roles.Select(c => c.Role).ToList();
            var permissions = userRole.SelectMany(c => c.PermissionCategory.Select(e => $"{e.PermissionCategoryPermission.Category.Label.ToLower()}_{e.PermissionCategoryPermission.Permission.Label.ToLower()}")).ToList();

            return directivePermissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase)) ||
                   permissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
        }



    }
}
