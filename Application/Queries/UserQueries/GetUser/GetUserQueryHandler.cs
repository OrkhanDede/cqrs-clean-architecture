using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Queries.UserQueries.GetAllUser;
using AutoMapper;
using Core.Constants;
using Core.Exceptions;
using Core.Models;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Queries;
using Infrastructure.Services;
using MediatR;

namespace Application.Queries.UserQueries.GetUser
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IMediator _mediator;

        public GetUserQueryHandler(IMapper mapper, IUserRepository userRepository, ExceptionService exceptionService, IMediator mediator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
            _mediator = mediator;
        }

        public async Task<GetUserResponse> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {

            var response = (await _mediator.Send(new GetAllUserQuery(new()
            {
                FilterParameters = new UserFilterParameters()
                {
                    Ids = new List<string>()
                    {
                        query.Request.Id
                    }
                },
                PagingParameters = new PagingParameters()
                {
                    Limit = 1
                }

            }), cancellationToken)).Response.Items.FirstOrDefault();

            if (response == null)
                throw _exceptionService.RecordNotFoundException();
            return new GetUserResponse
            {
                Response = response
            };
        }
    }
}
