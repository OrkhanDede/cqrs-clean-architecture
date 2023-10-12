using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Queries.PermissionQueries;

namespace Application.Queries.AccountQueries.GetAuthUserPermissions
{
    public class GetAuthUserPermissionsResponse
    {
        public List<PermissionResponse> Response { get; set; }
    }
}
