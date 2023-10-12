using System;
using System.Threading.Tasks;
using DataAccess.Repository;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class UserSignInResult
    {
        public bool Succeeded { get; set; }
        public bool IsLockOut { get; set; }
        public Exception Exception { get; set; }
    }
    public class SignInService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public SignInService(SignInManager<User> signInManager, IUnitOfWork unitOfWork, UserManager<User> userManager, IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userRepository = userRepository;

        }

        public async Task<UserSignInResult> SignInAsync(User user, string password)
        {
            var dateTimeNow = DateTime.Now.AddHours(-1);
            if (user.LastAccessFailedAttempt < dateTimeNow)//unlock user if 1 hour has passed since last failed access attempt
            {
                await _userRepository.SetUnLockoutAsync(user);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut) user.LastAccessFailedAttempt = null;
                else user.LastAccessFailedAttempt = DateTime.Now;
                await _unitOfWork.CompleteAsync();
            }
            return new UserSignInResult()
            {
                Succeeded = result.Succeeded,
                IsLockOut = result.IsLockedOut
            };
        }
    }
}
