using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Repository.RoleRepository
{
    public interface IRoleRepository
    {

        void SetGlobalQueryFilterStatus(bool status);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<Role> GetRoleByIdAsync(string roleId,
            params Expression<Func<Role, object>>[] includeProperties);
        Task<Role> GetRoleByIdAsync(string roleId, params string[] includeProperties);
        IQueryable<Role> FindBy(
            Expression<Func<Role, bool>> predicate);

        IQueryable<Role> FindBy(
            Expression<Func<Role, bool>> predicate, params string[] includeProperties);
        IQueryable<Role> FindBy(params string[] includeProperties);
        IQueryable<Role> FindBy(
            Expression<Func<Role, bool>> predicate,
            params Expression<Func<Role, object>>[] includeProperties);
        IQueryable<Role> GetAll();
        IQueryable<Role> GetAll(params string[] includeProperties);
        IQueryable<Role> GetAll(
            params Expression<Func<Role, object>>[] includeProperties);
        Task<bool> IsExistAsync(Expression<Func<Role, bool>> predicate);
        Task<IdentityResult> CreateAsync(Role role);
        Task<IdentityResult> UpdateAsync(Role role);
        Task<IdentityResult> DeleteAsync(Role role);

    }
}
