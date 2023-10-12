using Infrastructure.Configurations.Queries;

namespace Application.Queries.RoleQueries.GetUserRoles
{
    public class GetUserRolesQuery : IQuery<GetUserRolesResponse>
    {
        public GetUserRolesQuery(GetUserRolesRequest request)
        {
            Request = request;
        }

        public GetUserRolesRequest Request { get; set; }
    }
}
