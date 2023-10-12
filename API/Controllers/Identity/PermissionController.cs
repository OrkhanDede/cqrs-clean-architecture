using System.Threading.Tasks;
using Application.Queries.PermissionQueries.GetAllDirectivePermission;
using Application.Queries.PermissionQueries.GetAllPermission;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PermissionController : BaseController
    {


        [HttpGet]
        public async Task<ApiResponse> GetAllAsync()
        {
            var result = await Mediator.Send(new GetAllPermissionQuery());

            return new ApiResponse(result.Response);
        }

        [HttpGet("directive-permissions")]
        public async Task<ApiResponse> GetAllDirectivePermissionsAsync()
        {
            var result = await Mediator.Send(new GetAllDirectivePermissionQuery());

            return new ApiResponse(result.Response);
        }

    }
}
