using System.Collections.Generic;
using Application.Queries.PermissionQueries;
using Application.Queries.RoleQueries;

namespace Application.Queries.UserQueries
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }


    }
}
