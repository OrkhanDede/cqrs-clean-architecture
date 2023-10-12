using System.Threading;
using System.Threading.Tasks;
using Core.Extensions;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Queries;
using Infrastructure.Services;

namespace Application.Commands.AccountCommands.SignIn
{
    public class SignInCommandHandler : IQueryHandler<SignInCommand, SignInResponse>
    {
        private readonly TokenService _tokenService;
        private readonly AccessLogService _accessLogService;
        private readonly SignInService _signInService;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;

        public SignInCommandHandler(TokenService tokenService, AccessLogService accessLogService, SignInService signInService, IUserRepository userRepository, ExceptionService exceptionService)
        {
            _tokenService = tokenService;
            _accessLogService = accessLogService;
            _signInService = signInService;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
        }
        public async Task<SignInResponse> Handle(SignInCommand query, CancellationToken cancellationToken)
        {
            var emailOrUsername = query.Request.EmailOrUsername;
            var password = query.Request.Password;
            var isEmail = emailOrUsername.IsEmail();

            User user;
            if (isEmail)
                user = await _userRepository.GetUserByEmailAsync(emailOrUsername)
                    .ConfigureAwait(false);
            else
                user = await _userRepository.GetUserByNameAsync(emailOrUsername)
                    .ConfigureAwait(false);

            if (user == null)
                throw _exceptionService.InvalidSignInException();

            var isSignableUser = (await _signInService.SignInAsync(user, password)).Succeeded;
            if (!isSignableUser)
                throw _exceptionService.InvalidSignInException();

            var accessToken = _tokenService.GenerateToken(user);
            await _tokenService.RemoveOldRefreshTokensAsync(user.Id, removeAll: true);
            var refreshToken = await _tokenService.AddRefreshTokenAsync(user.Id);
            await _accessLogService.AddAccessLogAsync(user.Id);

            return new SignInResponse()
            {
                IsSigned = true,
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
