using System.Threading.Tasks;
using Application.Commands.RoleCommands.CreateRole;
using Application.Commands.RoleCommands.DeleteRole;
using Application.Commands.RoleCommands.UpdateRole;
using Application.Queries.RoleQueries;
using Application.Queries.RoleQueries.GetAllRole;
using Application.Queries.RoleQueries.GetRole;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Core.Models;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RoleController : BaseController
    {


        [Permission("role_list")]
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetAsync(string id)
        {
            var result = await Mediator.Send(new GetRoleQuery(new GetRoleRequest()
            {
                Id = id
            }));
            return new ApiResponse(result.Response);
        }

        [Permission("role_list")]
        [HttpGet]
        public async Task<ApiResponse> GetAllAsync([FromQuery] RoleFilterParameters filterParameters, [FromQuery] PagingParameters pagingParameters, [FromQuery] SortParameters sortParameters)
        {
            var result = await Mediator.Send(new GetAllRoleQuery(new GetAllRoleRequest()
            {
                FilterParameters = filterParameters,
                PagingParameters = pagingParameters,
                SortParameters = sortParameters
            }));
            return new ApiResponse(result.Response);
        }

        [Permission("role_add")]
        [HttpPost]
        public async Task<ApiResponse> CreateAsync([FromBody] CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());
            var result = await Mediator.Send(new CreateRoleCommand(request));

            return await GetAsync(result.Response.Id).ConfigureAwait(false);
        }

        [Permission("role_edit")]
        [HttpPut]
        public async Task<ApiResponse> UpdateAsync([FromBody] UpdateRoleRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());
            var result = await Mediator.Send(new UpdateRoleCommand(request));

            return await GetAsync(result.Response.Id).ConfigureAwait(false);
        }

        [Permission("role_delete")]
        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteAsync(string id)
        {

            await Mediator.Send(new DeleteRoleCommand(new DeleteRoleRequest()
            {
                Id = id
            }));

            return new ApiResponse(result: null);
        }

    }
}
