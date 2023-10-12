using System;

namespace Application.Queries.PermissionQueries
{
    public class PermissionResponse
    {
        public string Label { get; set; }
        public string VisibleLabel { get; set; }
        public string Description { get; set; }
        public bool IsDirective { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
