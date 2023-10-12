using System.Threading.Tasks;
using Application.Commands.UserCommands.ChangeUserPassword;
using Application.Commands.UserCommands.CreateUser;
using Application.Commands.UserCommands.DeleteUser;
using Application.Commands.UserCommands.UpdateUser;
using Application.Queries.UserQueries;
using Application.Queries.UserQueries.GetAllUser;
using Application.Queries.UserQueries.GetUser;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Core.Models;
using Infrastructure.Middlewares;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : BaseController
    {
        private readonly AuthService _authService;

        public UserController(AuthService authService)
        {
            _authService = authService;
        }

        [Permission("user_list")]
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetAsync(string id)
        {
            var result = await Mediator.Send(new GetUserQuery(new GetUserRequest() { Id = id }));
            return new ApiResponse(result.Response);

        }

        [Permission("user_list")]
        [HttpGet]
        public async Task<ApiResponse> GetAllAsync([FromQuery] UserFilterParameters filterParameters, [FromQuery] SortParameters sortParameters, [FromQuery] PagingParameters pagingParameters)
        {

            var result = await Mediator.Send(new GetAllUserQuery(new GetAllUserRequest()
            {
                FilterParameters = filterParameters,
                PagingParameters = pagingParameters,
                SortParameters = sortParameters
            }));
            // return result;
            return new ApiResponse(result.Response);

        }

        [Permission("user_add")]
        [HttpPost]
        public async Task<ApiResponse> CreateAsync([FromBody] CreateUserRequest request)
        {

            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var result = await Mediator.Send(new CreateUserCommand(request));
            return await GetAsync(result.Response.Id);

        }
        [Permission("user_edit")]
        [HttpPut]
        public async Task<ApiResponse> UpdateAsync([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var result = await Mediator.Send(new UpdateUserCommand(request));
            return await GetAsync(result.Response.Id);
        }

        [Permission("user_edit")]
        [HttpPut("{id}/change-password")]
        public async Task<ApiResponse> ChangePassword(string id, [FromBody] ChangeUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            await Mediator.Send(new ChangeUserPasswordCommand(id, request));
            return new ApiResponse(result: null);

        }
        [HttpPut("change-password")]
        public async Task<ApiResponse> ChangePassword([FromBody] ChangeUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());
            var authUserId = _authService.GetAuthorizedUserId();

            await Mediator.Send(new ChangeUserPasswordCommand(authUserId, request));
            return new ApiResponse(result: null);

        }

        [Permission("user_delete")]
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(string id)
        {
            await Mediator.Send(new DeleteUserCommand(new DeleteUserRequest()
            {
                UserId = id
            }));
            return new ApiResponse(result: null);
        }
    }
}
