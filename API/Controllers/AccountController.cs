using System.Linq;
using System.Threading.Tasks;
using Application.Commands.AccountCommands.ForgotPassword;
using Application.Commands.AccountCommands.RefreshToken;
using Application.Commands.AccountCommands.Register;
using Application.Commands.AccountCommands.SignIn;
using Application.Commands.AccountCommands.SignOutUser;
using Application.Commands.AccountCommands.StartImpersonate;
using Application.Commands.AccountCommands.StopImpersonate;
using Application.Queries.AccountQueries.GetAuthUser;
using Application.Queries.AccountQueries.GetAuthUserPermissions;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Core.Extensions;
using Infrastructure.Middlewares;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class AccountController : BaseController
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign-in")]
        public async Task<ApiResponse> AuthAsync([FromBody] SignInRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());
            var isEmail = request.EmailOrUsername.IsEmail();
            var isUsername = request.EmailOrUsername.IsUsername();

            if (!(isEmail || isUsername))
                ModelState.AddModelError("emailOrUsername", "Enter valid Email/Username");


            var response = await Mediator.Send(new SignInCommand(request));

            return new ApiResponse(result: response);
        }



        [HttpPost("reset-password")]
        public async Task<ApiResponse> PasswordResetAsync([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());
            var isEmail = request.EmailOrUsername.IsEmail();
            var isUsername = request.EmailOrUsername.IsUsername();

            if (!(isEmail || isUsername))
                ModelState.AddModelError("emailOrUsername", "Enter valid Email/Username");

            var response = await Mediator.Send(new ForgotPasswordCommand(request));
            return new ApiResponse(result: response);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("get-authorized-user")]
        public async Task<ApiResponse> GetAuthorizedAsync()
        {
            var result = await Mediator.Send(new GetAuthUserQuery
                (new GetAuthUserRequest()));
            return new ApiResponse(result.Response);

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("sign-out")]
        public async Task<ApiResponse> SignOutAsync()
        {
            var response = await Mediator.Send(new SignOutUserCommand(new SignOutUserRequest()));
            return new ApiResponse(response);
        }
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("refresh-token")]
        public async Task<ApiResponse> GetToken()
        {
            var userId = _authService.GetAuthorizedUserId();
            var response = await Mediator.Send(new RefreshTokenCommand(new RefreshTokenRequest()
            {
                UserId = userId
            }));
            return new ApiResponse(response);
        }

        [Permission("admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("start-impersonate")]
        public async Task<ApiResponse> StartImpersonate(string userId)
        {
            var result = (await Mediator.Send(new StartImpersonateCommand(new StartImpersonateRequest()
            {
                UserId = userId
            })));
            return new ApiResponse(result: result);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("stop-impersonate")]
        public async Task<ApiResponse> StopImpersonate()
        {
            await Mediator.Send(new StopImpersonateCommand(new StopImpersonateRequest()
            {
            }));
            return new ApiResponse(result: null);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("get-permissions")]
        public async Task<ApiResponse> GetPermissionsAsync()
        {

            var result = await Mediator.Send(new GetAuthUserPermissionsQuery(new()));
            return new ApiResponse(result: result.Response);

        }
        [HttpPost("sign-up")]
        public async Task<ApiResponse> SignUpAsync([FromBody] RegisterRequest request)
        {
            var result = await Mediator.Send(new RegisterCommand
                (request));
            return new ApiResponse(result: null);

        }

    }
}
