using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries.PermissionQueries;
using AutoMapper;
using Core.Exceptions;
using DataAccess.Repository.PermissionRepository;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Queries;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.UserQueries.GetUserDirectivePermissions
{
    public class GetUserDirectivePermissionsQueryHandler:IQueryHandler<GetUserDirectivePermissionsQuery,GetUserDirectivePermissionsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public GetUserDirectivePermissionsQueryHandler(IUserRepository userRepository , IPermissionRepository permissionRepository , IMapper mapper)
        {
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }
        public async Task<GetUserDirectivePermissionsResponse> Handle(GetUserDirectivePermissionsQuery query, CancellationToken cancellationToken)
        {
            var user =await _userRepository.GetUserByIdAsync(query.Request.UserId);
            if (user == null)
                throw new RecordNotFoundException();


            var permissions = await _permissionRepository.FindBy(c => c.Users.Any(b => b.UserId == user.Id)).ToListAsync(cancellationToken: cancellationToken);

            var result = _mapper.Map<List<Permission>, List<PermissionResponse>>(permissions);

            return new GetUserDirectivePermissionsResponse()
            {
                Response = result
            };
        }
    }
}
