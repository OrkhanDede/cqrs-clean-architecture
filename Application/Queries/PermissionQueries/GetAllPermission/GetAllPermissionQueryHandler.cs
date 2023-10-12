using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Repository.PermissionRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Queries;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.PermissionQueries.GetAllPermission
{
    public class GetAllPermissionQueryHandler : IQueryHandler<GetAllPermissionQuery, GetAllPermissionResponse>
    {
        private readonly IPermissionCategoryPermissionRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPermissionQueryHandler(IPermissionCategoryPermissionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GetAllPermissionResponse> Handle(GetAllPermissionQuery query, CancellationToken cancellationToken)
        {
            var data = _repository.GetAll("Permission", "Category");
            data = data.OrderByDescending(c => c.Category.Label);
            var result =
                _mapper.Map<List<PermissionCategoryPermission>, List<PermissionCategoryRelationResponse>>(await data.ToListAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false));
            return new GetAllPermissionResponse()
            {
                Response = result
            };
        }
    }
}
