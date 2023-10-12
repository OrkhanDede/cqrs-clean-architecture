using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using DataAccess.Repository.PermissionRepository;
using DataAccess.Repository.RoleRepository;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.InitializeCommands.SetUuidToAllRecord
{
    public class SetUuidToAllRecordsCommandHandler : ICommandHandler<SetUuidToAllRecordCommand, SetUuidToAllRecordResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public SetUuidToAllRecordsCommandHandler(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<SetUuidToAllRecordResponse> Handle(SetUuidToAllRecordCommand command, CancellationToken cancellationToken)
        {




            var permissionRepository = new PermissionRepository(_context);
            var permissionQuery = permissionRepository.FindBy(c => string.IsNullOrEmpty(c.Uuid)).IgnoreQueryFilters();
            var permissionItems = await permissionQuery.ToListAsync(cancellationToken: cancellationToken);
            permissionItems.ForEach(statement => statement.Uuid = Guid.NewGuid().ToString());

            var permissionCategoryPermissionRepository = new PermissionCategoryPermissionRepository(_context);
            var permissionCategoryPermissionQuery = permissionCategoryPermissionRepository.FindBy(c => string.IsNullOrEmpty(c.Uuid)).IgnoreQueryFilters();
            var permissionCategoryPermissionItems = await permissionCategoryPermissionQuery.ToListAsync(cancellationToken: cancellationToken);
            permissionCategoryPermissionItems.ForEach(statement => statement.Uuid = Guid.NewGuid().ToString());

            var permissionCategoryRepository = _context.PermissionCategories;
            var permissionCategoryQuery = permissionCategoryRepository.Where(c => string.IsNullOrEmpty(c.Uuid)).IgnoreQueryFilters();
            var permissionCategoryItems = await permissionCategoryQuery.ToListAsync(cancellationToken: cancellationToken);
            permissionCategoryItems.ForEach(statement => statement.Uuid = Guid.NewGuid().ToString());

            var userRepository = new UserRepository(_userManager);
            var userQuery = userRepository.FindBy(c => string.IsNullOrWhiteSpace(c.Uuid)).IgnoreQueryFilters();
            var userItems = await userQuery.ToListAsync(cancellationToken: cancellationToken);
            userItems.ForEach(user => user.Uuid = Guid.NewGuid().ToString());

            var roleRepository = new RoleRepository(_roleManager);
            var roleQuery = roleRepository.FindBy(c => string.IsNullOrWhiteSpace(c.Uuid)).IgnoreQueryFilters();
            var roleItems = await roleQuery.ToListAsync(cancellationToken: cancellationToken);
            roleItems.ForEach(user => user.Uuid = Guid.NewGuid().ToString());


            var userRoleRepository = _context.UserRoles;
            var userRoleQuery = userRoleRepository.Where(c => string.IsNullOrWhiteSpace(c.Uuid)).IgnoreQueryFilters();
            var userRoleItems = await userRoleQuery.ToListAsync(cancellationToken: cancellationToken);
            userRoleItems.ForEach(user => user.Uuid = Guid.NewGuid().ToString());


            var userPermissionRepository = _context.UserPermissions;
            var userPermissionQuery = userPermissionRepository.Where(c => string.IsNullOrWhiteSpace(c.Uuid)).IgnoreQueryFilters();
            var userPermissionItems = await userPermissionQuery.ToListAsync(cancellationToken: cancellationToken);
            userPermissionItems.ForEach(user => user.Uuid = Guid.NewGuid().ToString());

            var rolePermissionCategoryRepository = _context.RolePermissionCategories;
            var rolePermissionCategoryQuery = rolePermissionCategoryRepository.Where(c => string.IsNullOrWhiteSpace(c.Uuid)).IgnoreQueryFilters();
            var rolePermissionCategoryItems = await rolePermissionCategoryQuery.ToListAsync(cancellationToken: cancellationToken);
            rolePermissionCategoryItems.ForEach(user => user.Uuid = Guid.NewGuid().ToString());


            await _context.SaveChangesAsync(cancellationToken);







            return new SetUuidToAllRecordResponse();

        }
    }
}
