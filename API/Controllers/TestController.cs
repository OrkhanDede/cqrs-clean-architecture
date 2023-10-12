using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TestController : BaseController
    {

        [HttpGet]
        public ApiResponse Get()
        {
            return new ApiResponse(new[] { "value 1", "value 2", "value 2" });
        }
    }
}
