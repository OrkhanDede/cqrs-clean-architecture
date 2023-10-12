using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class IdMustBeGuidException : ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;

        public IdMustBeGuidException(string title = "Id must be in guid format!", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
}
