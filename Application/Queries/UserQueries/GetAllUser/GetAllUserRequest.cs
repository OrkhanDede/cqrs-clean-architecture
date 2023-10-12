using Core.Models;

namespace Application.Queries.UserQueries.GetAllUser
{
    public class GetAllUserRequest
    {
        public UserFilterParameters FilterParameters { get; set; }
        public PagingParameters PagingParameters { get; set; }
        public SortParameters SortParameters { get; set; }
    }
}
