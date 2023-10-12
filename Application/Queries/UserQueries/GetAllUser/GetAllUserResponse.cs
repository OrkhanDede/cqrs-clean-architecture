using Core.Models;

namespace Application.Queries.UserQueries.GetAllUser
{
    public class GetAllUserResponse
    {
        public FilteredDataResult<UserResponse> Response { get; set; }
    }
}