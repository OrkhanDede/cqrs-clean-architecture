using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.RoleRepository
{
    public class RoleRepository : IRoleRepository, IRepositoryIdentifier
    {
        private readonly RoleManager<Role> _roleManager;
        private bool _isQueryFilterApplied = true;
        public RoleRepository(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public void SetGlobalQueryFilterStatus(bool status)
        {
            this._isQueryFilterApplied = status;
        }
        public Task<Role> GetRoleByIdAsync(string roleId)
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).FirstOrDefaultAsync(c => c.Id == roleId);
        }

        public Task<Role> GetRoleByIdAsync(string roleId, params Expression<Func<Role, object>>[] includeProperties)
        {
            var query = _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
            return query.FirstOrDefaultAsync(c => c.Id == roleId);
        }


        public Task<Role> GetRoleByIdAsync(string roleId, params string[] includeProperties)
        {
            var query = _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);

            return query.FirstOrDefaultAsync(c => c.Id == roleId);
        }
        public IQueryable<Role> FindBy(Expression<Func<Role, bool>> predicate)
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).Where(predicate);
        }
        public IQueryable<Role> FindBy(Expression<Func<Role, bool>> predicate, params string[] includeProperties)
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties).Where(predicate);
        }

        public IQueryable<Role> FindBy(params string[] includeProperties)
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
        }

        public IQueryable<Role> FindBy(Expression<Func<Role, bool>> predicate, params Expression<Func<Role, object>>[] includeProperties)
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties).Where(predicate);
        }

        public IQueryable<Role> GetAll()
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied);
        }
        public IQueryable<Role> GetAll(params string[] includeProperties)
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
        }
        public IQueryable<Role> GetAll(params Expression<Func<Role, object>>[] includeProperties)
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
        }

        public Task<bool> IsExistAsync(Expression<Func<Role, bool>> predicate)
        {
            return _roleManager.Roles.ApplyQueryFilter(this._isQueryFilterApplied).AnyAsync(predicate);
        }
        public Task<IdentityResult> CreateAsync(Role role)
        {
            return _roleManager.CreateAsync(role);
        }
        public Task<IdentityResult> UpdateAsync(Role role)
        {
            return _roleManager.UpdateAsync(role);
        }

        public Task<IdentityResult> DeleteAsync(Role role)
        {
            return _roleManager.DeleteAsync(role);
        }


    }
}
