using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class InternalServerException : ApiException
    {

        public InternalServerException(string title = "Sorry, an unexpected error has occurred.",string errorCode="") : base(title, StatusCodes.Status500InternalServerError,errorCode)
        {
        }
    }
}
