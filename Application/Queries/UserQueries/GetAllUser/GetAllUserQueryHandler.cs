using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Extensions;
using Core.Models;
using DataAccess;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Queries;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.UserQueries.GetAllUser
{
    public class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, GetAllUserResponse>
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUserQueryHandler(AuthService authService, IUserRepository userRepository, IMapper mapper)
        {
            _authService = authService;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<GetAllUserResponse> Handle(GetAllUserQuery query, CancellationToken cancellationToken)
        {

            Expression<Func<User, bool>> filterPredicate = null;

            var filterParameters = query.Request.FilterParameters;
            if (filterParameters != null)
            {
                var isTextExist = !string.IsNullOrEmpty(filterParameters.Text);
                var isIdsExist = filterParameters.Ids != null && filterParameters.Ids.Any();
                var isNamesExist = filterParameters.Names != null && filterParameters.Names.Any();
                filterPredicate = c =>
                    (!isTextExist || (c.UserName.ToLower().Contains(filterParameters.Text.ToLower()) ||
                                      c.Email.ToLower().Contains(filterParameters.Text.ToLower()))) &&
                    (!isNamesExist || filterParameters.Names.Select(e => e.ToLower()).ToList().Contains(c.UserName.ToLower())) &&
                    (!isIdsExist || filterParameters.Ids.Contains(c.Id));

            }

            //get total data count before paging
            var dataCount = await _userRepository.FindBy(filterPredicate).CountAsync(cancellationToken: cancellationToken);

            var data = _userRepository.FindBy(filterPredicate);

            //sorting
            data = data.SortBy(query.Request.SortParameters);


            //paging
            if (!query.Request.PagingParameters.IsAll)
                data = data.FindPaged(query.Request.PagingParameters);

            //mapping
            var result = _mapper.Map<List<User>, List<UserResponse>>(await data.ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false));

            return new GetAllUserResponse()
            {
                Response = new FilteredDataResult<UserResponse>()
                {
                    Items = result,
                    TotalCount = dataCount
                }
            };
        }
    }
}
