using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;
using MediatR;

namespace Application.Commands.AccountCommands.StopImpersonate
{
    public class StopImpersonateCommandHandler : ICommandHandler<StopImpersonateCommand, StopImpersonateResponse>
    {
        private readonly IMediator _mediator;
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public StopImpersonateCommandHandler(IMediator mediator, AuthService authService, TokenService tokenService)
        {
            _mediator = mediator;
            _authService = authService;
            _tokenService = tokenService;
        }
        public async Task<StopImpersonateResponse> Handle(StopImpersonateCommand command, CancellationToken cancellationToken)
        {

            var authUserId = _authService.GetAuthorizedUserId();
            var tokenClaim = _authService.GetTokenClaim();

            if (tokenClaim.ImpersonatorId != null)
            {
                var impersonatorIsAdmin = await _authService.IsAdminAsync(tokenClaim.ImpersonatorId.Value);
                if (impersonatorIsAdmin)
                {
                    await _tokenService.RemoveOldRefreshTokensAsync(authUserId, removeAll: true);
                }

            }

            return new StopImpersonateResponse()
            {

            };

        }
    }
}
