using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class RecordNotEditableException : ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;

        public RecordNotEditableException(string title = "This record is not editable.",string errorCode="") : base(title, Statuscode,errorCode)
        {
        }
    }
}
