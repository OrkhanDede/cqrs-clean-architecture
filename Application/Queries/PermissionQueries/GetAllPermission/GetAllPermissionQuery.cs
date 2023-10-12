using Infrastructure.Configurations.Queries;

namespace Application.Queries.PermissionQueries.GetAllPermission
{
    public class GetAllPermissionQuery : IQuery<GetAllPermissionResponse>
    {
        public GetAllPermissionRequest Request { get; set; }
    }
}
