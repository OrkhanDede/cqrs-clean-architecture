using Core.Models;

namespace Application.Queries.RoleQueries.GetAllRole
{
    public class GetAllRoleRequest
    {
        public RoleFilterParameters FilterParameters { get; set; }
        public PagingParameters PagingParameters { get; set; }
        public SortParameters SortParameters { get; set; }
    }
}
