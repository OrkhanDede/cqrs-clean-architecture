using Infrastructure.Configurations.Queries;

namespace Application.Queries.RoleQueries.GetRole
{
    public class GetRoleQuery : IQuery<GetRoleResponse>
    {
        public GetRoleQuery(GetRoleRequest request)
        {
            Request = request;
        }

        public GetRoleRequest Request { get; set; }

    }
}
