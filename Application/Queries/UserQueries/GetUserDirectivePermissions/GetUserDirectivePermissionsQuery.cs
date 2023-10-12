using Infrastructure.Configurations.Queries;

namespace Application.Queries.UserQueries.GetUserDirectivePermissions
{
    public class GetUserDirectivePermissionsQuery:IQuery<GetUserDirectivePermissionsResponse>
    {
        public GetUserDirectivePermissionsQuery(GetUserDirectivePermissionsRequest request)
        {
            Request = request;
        }

        public GetUserDirectivePermissionsRequest Request { get; set; }
    }
}
