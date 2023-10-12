using System.Collections.Generic;

namespace Application.Queries.UserQueries
{
    public class UserFilterParameters
    {
        public string Text { get; set; }
        public List<string> Ids { get; set; } = new List<string>();
        public List<string> Names { get; set; } = new List<string>();
    }
}
