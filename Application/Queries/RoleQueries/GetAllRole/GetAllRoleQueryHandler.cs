using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Constants;
using Core.Extensions;
using Core.Models;
using DataAccess;
using DataAccess.Repository.RoleRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Queries;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.RoleQueries.GetAllRole
{
    public class GetAllRoleQueryHandler : IQueryHandler<GetAllRoleQuery, GetAllRoleResponse>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRoleQueryHandler(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GetAllRoleResponse> Handle(GetAllRoleQuery query, CancellationToken cancellationToken)
        {
            var includeParams = new IncludeStringConstants().RolePermissionIncludeList.ToList();

            var filterParameters = query.Request.FilterParameters;

            Expression<Func<Role, bool>> filterPredicate = c => true;
            //filter by props
            if (filterParameters != null)
            {
                var isTextExist = !string.IsNullOrEmpty(filterParameters.Text);
                var isIdsExist = filterParameters.Ids != null && filterParameters.Ids.Any();
                var isUserIdsExist = filterParameters.UserIds != null && filterParameters.UserIds.Any();

                filterPredicate = c =>
                    (!isTextExist || c.Name.ToLower().Contains(filterParameters.Text.ToLower())) &&
                    (!isUserIdsExist || c.Users.Any(e => filterParameters.UserIds.Contains(e.UserId))) &&
                    (!isIdsExist || filterParameters.Ids.Contains(c.Id));
            }
            //merge filter expressions

            //get total data count before paging
            var dataCount = await _repository.FindBy(filterPredicate).CountAsync(cancellationToken: cancellationToken);

            var data = _repository.FindBy(filterPredicate, includeParams.ToArray());

            //sorting
            data = data.SortBy(query.Request.SortParameters);


            //paging
            data = data.FindPaged(query.Request.PagingParameters);

            //mapping
            var result = _mapper.Map<List<Role>, List<RoleResponse>>(await data.ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false));
            return new GetAllRoleResponse()
            {
                Response = new FilteredDataResult<RoleResponse>()
                {
                    Items = result,
                    TotalCount = dataCount
                }
            };
        }
    }
}
