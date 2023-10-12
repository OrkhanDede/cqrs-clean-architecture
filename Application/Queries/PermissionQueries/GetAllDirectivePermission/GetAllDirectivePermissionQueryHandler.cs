using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Repository.PermissionRepository;
using Infrastructure.Configurations.Queries;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.PermissionQueries.GetAllDirectivePermission
{
    public class GetAllDirectivePermissionQueryHandler : IQueryHandler<GetAllDirectivePermissionQuery, GetAllDirectivePermissionResponse>
    {
        private readonly IPermissionRepository _repository;
        private readonly IMapper _mapper;

        public GetAllDirectivePermissionQueryHandler(IPermissionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GetAllDirectivePermissionResponse> Handle(GetAllDirectivePermissionQuery query, CancellationToken cancellationToken)
        {
            var data = _repository.FindBy(c => c.IsDirective && !string.Equals(c.Label.ToLower(), "admin"));
            data = data.OrderByDescending(c => c.Label);
            var result =
                _mapper.Map<List<Domain.Entities.Identity.Permission>, List<PermissionResponse>>(await data.ToListAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false));

            return new GetAllDirectivePermissionResponse()
            {
                Response = result
            };
        }
    }
}
