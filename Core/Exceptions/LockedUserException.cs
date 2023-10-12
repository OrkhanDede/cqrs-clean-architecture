using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class LockedUserException : ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;

        public LockedUserException(string title = "Your account has been blocked for 1 hour, please contact administrator.", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
}
