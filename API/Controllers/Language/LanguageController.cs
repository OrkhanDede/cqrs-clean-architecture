using System.Threading.Tasks;
using Application.Commands.LanguageCommands.ResetLanguage;
using Application.Commands.LanguageCommands.SetLanguageKeyValue;
using Application.Queries.LanguageQueries.GetAllLanguage;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Language
{
    public class LanguageController : BaseController
    {
        [HttpGet]
        public async Task<ApiResponse> GetLanguagesAsync([FromQuery] GetAllLanguageRequest request)
        {
            var result = await Mediator.Send(new GetAllLanguageQuery(request));

            return new ApiResponse(result.Response);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Permission("language_set")]
        [HttpPost("set-value")]
        public async Task<ApiResponse> SetValueAsync([FromBody] SetLanguageKeyValueRequest request)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());
            await Mediator.Send(new SetLanguageKeyValueCommand(request));
            return new ApiResponse(result: null);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Permission("admin")]
        [HttpGet("reset")]
        public async Task<ApiResponse> Reset()
        {

            await Mediator.Send(new ResetLanguageCommand(new ResetLanguageRequest()));
            return new ApiResponse(result: null);
        }

    }
}
