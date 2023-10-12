using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Configurations.Queries;

namespace Application.Queries.AccountQueries.GetAuthUserPermissions
{
    public class GetAuthUserPermissionsQuery:IQuery<GetAuthUserPermissionsResponse>
    {
        public GetAuthUserPermissionsQuery(GetAuthUserPermissionsRequest request)
        {
            Request = request;
        }

        public GetAuthUserPermissionsRequest Request { get; set; }
    }
}
