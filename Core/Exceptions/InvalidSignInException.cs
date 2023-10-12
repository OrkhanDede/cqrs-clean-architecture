using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class InvalidSignInException : ApiException
    {
        private const int Statuscode = StatusCodes.Status400BadRequest;

        public InvalidSignInException(string title = "Username or password is not valid.", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
}
