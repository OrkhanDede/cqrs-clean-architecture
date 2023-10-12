using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repository;
using DataAccess.Repository.RefreshTokenRepository;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.AccountCommands.SignOutUser
{
    public class SignOutUserCommandHandler : ICommandHandler<SignOutUserCommand, SignOutUserResponse>
    {
        private readonly IRefreshTokenRepository _userJwtRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public SignOutUserCommandHandler(IRefreshTokenRepository userJwtRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor, AuthService authService, TokenService tokenService)
        {
            _userJwtRepository = userJwtRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _tokenService = tokenService;
        }

        public async Task<SignOutUserResponse> Handle(SignOutUserCommand request, CancellationToken cancellationToken)
        {

            var authUserId = _authService.GetAuthorizedUserId();

            await _tokenService.RemoveOldRefreshTokensAsync(authUserId, true);
            return new SignOutUserResponse();
        }
    }
}
