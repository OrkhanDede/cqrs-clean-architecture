using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class RecordAlreadyExistException : ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;

        public RecordAlreadyExistException(string title = "Record already exist.", string errorCode = "") : base(title, Statuscode, errorCode)
        {
        }
    }
}
