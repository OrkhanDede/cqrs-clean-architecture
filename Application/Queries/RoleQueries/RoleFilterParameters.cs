using System.Collections.Generic;

namespace Application.Queries.RoleQueries
{
    public class RoleFilterParameters
    {
        public string Text { get; set; }
        public List<string> Ids { get; set; }
        public List<string> UserIds { get; set; }
    }
}
