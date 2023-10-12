using System.Threading;
using System.Threading.Tasks;
using Application.Queries.UserQueries.GetUser;
using Infrastructure.Configurations.Queries;
using Infrastructure.Services;
using MediatR;

namespace Application.Queries.AccountQueries.GetAuthUser
{
    public class GetAuthUserQueryHandler : IQueryHandler<GetAuthUserQuery, GetAuthUserResponse>
    {
        private readonly IMediator _mediator;
        private readonly AuthService _authService;

        public GetAuthUserQueryHandler(IMediator mediator, AuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }
        public async Task<GetAuthUserResponse> Handle(GetAuthUserQuery query, CancellationToken cancellationToken)
        {
            var userId = _authService.GetAuthorizedUserId();
            var result = await _mediator.Send(new GetUserQuery(new GetUserRequest()
            {
                Id = userId
            }), cancellationToken);
            return new GetAuthUserResponse()
            {
                Response = result.Response
            };
        }
    }
}
