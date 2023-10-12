namespace Application.Queries.PermissionQueries
{
    public class PermissionCategoryRelationResponse
    {
        public int RelationId { get; set; }
        public PermissionCategoryResponse Category { get; set; }
        public PermissionResponse Permission { get; set; }
    }
}
