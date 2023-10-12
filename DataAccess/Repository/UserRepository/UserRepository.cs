using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Core.Enums;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.UserRepository
{
    public class UserRepository : IUserRepository, IRepositoryIdentifier
    {
        private readonly UserManager<User> _userManager;
        private bool _isQueryFilterApplied = true;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public void SetGlobalQueryFilterStatus(bool status)
        {
            this._isQueryFilterApplied = status;
        }
        public Task<User> GetUserByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return Task.FromResult<User>(null);
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).FirstOrDefaultAsync(c => c.UserName.ToLower() == userName.ToLower());
        }
        public Task<User> GetUserByNameAsync(string userName, params Expression<Func<User, object>>[] includeProperties)
        {
            if (includeProperties == null) throw new ArgumentNullException(nameof(includeProperties));
            if (string.IsNullOrEmpty(userName))
                return Task.FromResult<User>(null);
            var query = _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
            return query.FirstOrDefaultAsync(c => c.UserName.ToLower() == userName.ToLower());
        }
        public Task<User> GetUserByNameAsync(string userName, params string[] includeProperties)
        {
            if (string.IsNullOrEmpty(userName))
                return Task.FromResult<User>(null);
            var query = _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
            return query.FirstOrDefaultAsync(c => c.UserName.ToLower() == userName.ToLower());
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Task.FromResult<User>(null);
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public Task<User> GetUserByIdAsync(string userId)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).FirstOrDefaultAsync(c => c.Id == userId);
        }

        public Task<User> GetUserByIdAsync(string userId, params Expression<Func<User, object>>[] includeProperties)
        {
            var query = _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
            return query.FirstOrDefaultAsync(c => c.Id == userId);
        }


        public Task<User> GetUserByIdAsync(string userId, params string[] includeProperties)
        {
            var query = _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);

            return query.FirstOrDefaultAsync(c => c.Id == userId);
        }

        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).Where(predicate);
        }
        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate, params string[] includeProperties)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties).Where(predicate);
        }

        public IQueryable<Domain.Entities.Identity.User> FindBy(params string[] includeProperties)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
        }

        public IQueryable<User> FindBy(params Expression<Func<User, object>>[] includeProperties)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
        }

        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate, params Expression<Func<User, object>>[] includeProperties)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties).Where(predicate);
        }
        public IQueryable<User> GetAll()
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied);
        }
        public IQueryable<User> GetAll(params string[] includeProperties)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
        }
        public IQueryable<User> GetAll(params Expression<Func<User, object>>[] includeProperties)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).IncludeAll(includeProperties);
        }



        public Task<bool> IsExistAsync(Expression<Func<User, bool>> predicate)
        {
            return _userManager.Users.ApplyQueryFilter(this._isQueryFilterApplied).AnyAsync(predicate);
        }
        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }
        public Task<IdentityResult> UpdateAsync(User user)
        {
            return _userManager.UpdateAsync(user);
        }
        public async Task Delete(User user)
        {

            await _userManager.DeleteAsync(user);
        }

        public Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> ResetPasswordAsync(Domain.Entities.Identity.User user, string token, string newPassword)
        {
            // var user = await GetUserByIdAsync(userId, c => c.EmailConfirmationRequests);
            return await _userManager.ResetPasswordAsync(user, token, newPassword).ConfigureAwait(false);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await GetUserByIdAsync(userId, c => c.EmailConfirmationRequests);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;

        }
        public async Task<string> CreateEmailConfirmationTokenAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId, c => c.EmailConfirmationRequests);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
            var encodedToken = HttpUtility.UrlEncode(token);
            await SetLastEmailRequestDeleted(userId).ConfigureAwait(false);
            user.EmailConfirmationRequests.Add(new EmailConfirmationRequest
            {
                Token = encodedToken,
                ExpireDate = DateTime.Now.AddDays(1),

            });
            await UpdateAsync(user).ConfigureAwait(false);

            return encodedToken;
        }

        public async Task<string> CreatePasswordResetTokenAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId, c => c.PasswordResetRequests);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            var encodedToken = HttpUtility.UrlEncode(token);

            await SetLastPasswordRequestDeleted(userId).ConfigureAwait(false);

            user.PasswordResetRequests.Add(new PasswordResetRequest()
            {
                Token = encodedToken,
                ExpireDate = DateTime.Now.AddDays(1),

            });
            await UpdateAsync(user).ConfigureAwait(false);

            return encodedToken;
        }
        public async Task SetLastEmailRequestDeleted(string userId)
        {
            var user = await GetUserByIdAsync(userId, c => c.EmailConfirmationRequests);
            var lastRequest = user.EmailConfirmationRequests.LastOrDefault();
            if (lastRequest != null)
                lastRequest.Status = RecordStatusEnum.Deleted;
            await UpdateAsync(user).ConfigureAwait(false);
        }
        public async Task SetLastPasswordRequestDeleted(string userId)
        {
            var user = await GetUserByIdAsync(userId, c => c.PasswordResetRequests);
            var lastRequest = user.PasswordResetRequests.LastOrDefault();
            if (lastRequest != null)
                lastRequest.Status = RecordStatusEnum.Deleted;
            await UpdateAsync(user).ConfigureAwait(false);
        }
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task SetLockoutEnabledAsync(User user, DateTime expireDate)
        {
            await _userManager.SetLockoutEndDateAsync(user, expireDate);
        }
        public async Task SetUnLockoutAsync(User user)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);
            user.LastAccessFailedAttempt = null;
        }


    }
}
