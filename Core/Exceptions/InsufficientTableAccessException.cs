using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class InsufficientTableAccessException : ApiException
    {
        private const int Statuscode = StatusCodes.Status403Forbidden;

        public InsufficientTableAccessException(string title = "Insufficient table access.", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
}
