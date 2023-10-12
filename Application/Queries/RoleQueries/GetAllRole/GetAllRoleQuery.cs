using Infrastructure.Configurations.Queries;

namespace Application.Queries.RoleQueries.GetAllRole
{
    public class GetAllRoleQuery : IQuery<GetAllRoleResponse>
    {
        public GetAllRoleQuery(GetAllRoleRequest request)
        {
            Request = request;
        }

        public GetAllRoleRequest Request { get; set; }
    }
}
