using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class RecordNotFoundException : ApiException
    {
        private const int Statuscode = StatusCodes.Status404NotFound;

        public RecordNotFoundException(string title = "Requested data not found.",string errorCode="") : base(title, Statuscode,errorCode)
        {
        }
    }
}
