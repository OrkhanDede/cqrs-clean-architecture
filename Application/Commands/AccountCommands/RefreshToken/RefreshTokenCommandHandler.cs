using System.Threading;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Core.Extensions;
using DataAccess.Repository.RefreshTokenRepository;
using DataAccess.Repository.UserRepository;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.AccountCommands.RefreshToken
{
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly AuthService _authService;
        private readonly ExceptionService _exceptionService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenService _tokenService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenCommandHandler(AuthService authService, ExceptionService exceptionService, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, TokenService tokenService, IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _exceptionService = exceptionService;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.GetRefreshToken();
            var refreshToken = await _refreshTokenRepository.GetFirstAsync(c => c.Token == token, "User", "Impersonator");

            if (refreshToken is not { IsActive: true })
                throw _exceptionService.InvalidToken();

            var isRevoked = refreshToken.IsRevoked;
            if (isRevoked)
            {
                await _tokenService.RevokeRefreshTokensAsync(token);
            }
            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = await _tokenService.RotateRefreshTokenAsync(refreshToken.Token);

            var newJwtToken = _tokenService.GenerateToken(refreshToken.User, refreshToken.ToTokenClaim().ToClaimList().ToArray());

            // remove old refresh tokens from user
            await _tokenService.RemoveOldRefreshTokensAsync(refreshToken.UserId, true, c => c.Token != newRefreshToken.Token);



            return new RefreshTokenResponse()
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}