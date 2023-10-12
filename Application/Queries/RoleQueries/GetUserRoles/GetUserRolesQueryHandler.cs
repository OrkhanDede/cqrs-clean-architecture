using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries.RoleQueries.GetAllRole;
using Infrastructure.Configurations.Queries;
using Infrastructure.Services;
using MediatR;

namespace Application.Queries.RoleQueries.GetUserRoles
{
    public class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, GetUserRolesResponse>
    {
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;

        public GetUserRolesQueryHandler(IMediator mediator, ExceptionService exceptionService)
        {
            _mediator = mediator;
            _exceptionService = exceptionService;
        }
        public async Task<GetUserRolesResponse> Handle(GetUserRolesQuery query, CancellationToken cancellationToken)
        {
            var result = (await _mediator.Send(new GetAllRoleQuery(new GetAllRoleRequest()
            {
                FilterParameters = new RoleFilterParameters()
                {
                    UserIds = new List<string>() { query.Request.UserId },
                }
            }), cancellationToken)).Response.Items;

            if (result == null)
                throw _exceptionService.RecordNotFoundException();

            return new GetUserRolesResponse()
            {
                Response = result
            };
        }
    }
}
