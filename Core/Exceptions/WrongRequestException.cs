using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class WrongRequestException: ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;
        public WrongRequestException(string title = "Wrong request.",string errorCode="") : base(title, Statuscode,errorCode)
        {
        }
    }
}
