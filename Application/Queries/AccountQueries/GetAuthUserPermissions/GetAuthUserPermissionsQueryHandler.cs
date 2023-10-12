using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries.UserQueries.GetUserDirectivePermissions;
using Infrastructure.Configurations.Queries;
using Infrastructure.Services;
using MediatR;

namespace Application.Queries.AccountQueries.GetAuthUserPermissions
{
    public class GetAuthUserPermissionsQueryHandler:IQueryHandler<GetAuthUserPermissionsQuery, GetAuthUserPermissionsResponse>
    {
        private readonly IMediator _mediator;
        private readonly AuthService _authService;

        public GetAuthUserPermissionsQueryHandler(IMediator mediator , AuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }
        public async Task<GetAuthUserPermissionsResponse> Handle(GetAuthUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var authUserId = _authService.GetAuthorizedUserId();
            var response = (await _mediator.Send(new GetUserDirectivePermissionsQuery(
                new GetUserDirectivePermissionsRequest()
                {
                    UserId = authUserId
                }))).Response;

            return new GetAuthUserPermissionsResponse()
            {
                Response = response
            };

        }
    }
}
