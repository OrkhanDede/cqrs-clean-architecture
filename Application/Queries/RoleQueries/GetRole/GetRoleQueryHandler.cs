using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries.RoleQueries.GetAllRole;
using Infrastructure.Configurations.Queries;
using Infrastructure.Services;
using MediatR;

namespace Application.Queries.RoleQueries.GetRole
{
    public class GetRoleQueryHandler : IQueryHandler<GetRoleQuery, GetRoleResponse>
    {
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;

        public GetRoleQueryHandler(IMediator mediator, ExceptionService exceptionService)
        {
            _mediator = mediator;
            _exceptionService = exceptionService;
        }
        public async Task<GetRoleResponse> Handle(GetRoleQuery query, CancellationToken cancellationToken)
        {
            var result = (await _mediator.Send(new GetAllRoleQuery(new GetAllRoleRequest()
            {
                FilterParameters = new RoleFilterParameters()
                {
                    Ids = new List<string>() { query.Request.Id },
                }
            }), cancellationToken)).Response.Items.FirstOrDefault();

            if (result == null)
                throw _exceptionService.RecordNotFoundException();
            return new GetRoleResponse()
            {
                Response = result
            };
        }
    }
}
