using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries.AccountQueries.GetAuthUser;
using AutoWrapper.Wrappers;
using Core.Constants;
using DataAccess.Repository.UserRepository;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;
using MediatR;

namespace Application.Commands.AccountCommands.StartImpersonate
{
    public class StartImpersonateCommandHandler : ICommandHandler<StartImpersonateCommand, StartImpersonateResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;
        private readonly ExceptionService _exceptionService;

        public StartImpersonateCommandHandler(IMediator mediator, IUserRepository userRepository, AuthService authService, TokenService tokenService, ExceptionService exceptionService)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _authService = authService;
            _tokenService = tokenService;
            _exceptionService = exceptionService;
        }
        public async Task<StartImpersonateResponse> Handle(StartImpersonateCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(command.Request.UserId);

            var authUser = (await _mediator.Send(new GetAuthUserQuery(new GetAuthUserRequest()), cancellationToken)).Response;

            if (user.Id == authUser.Id)
                throw _exceptionService.CantImpersonateYourself();

            var tokenClaim = _authService.GetTokenClaim();
            var impersonatorIdClaim = new Claim(CustomClaimTypeConstants.Impersonator, authUser.Id);
            var impersonatorNameClaim = new Claim(CustomClaimTypeConstants.ImpersonatorName, authUser.UserName);
            tokenClaim.ImpersonatorId = impersonatorIdClaim;
            tokenClaim.ImpersonatorName = impersonatorNameClaim;
            var claims = tokenClaim.ToClaimList();

            var accessToken = _tokenService.GenerateToken(user, claims.ToArray());
            await _tokenService.RemoveOldRefreshTokensAsync(user.Id, removeAll: false);
            var refreshToken = await _tokenService.AddRefreshTokenAsync(user.Id, tokenClaim);


            return new StartImpersonateResponse()
            {
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };

        }
    }
}
