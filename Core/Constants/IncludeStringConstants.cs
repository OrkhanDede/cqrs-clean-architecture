namespace Core.Constants
{
    public class IncludeStringConstants
    {
        public readonly string[] UserRolePermissionIncludeArray = new string[] { "Roles.Role.PermissionCategory.PermissionCategoryPermission.Category", "Roles.Role.PermissionCategory.PermissionCategoryPermission.Permission", "DirectivePermissions.Permission" };
        //public static string[] RolePermissionIncludeArray = new string[] { "PermissionCategory.PermissionCategoryPermission.Category", "PermissionCategory.PermissionCategoryPermission.Permission" };

        public readonly string[] RolePermissionIncludeList = new string[] {"PermissionCategory.PermissionCategoryPermission.Category",
            "PermissionCategory.PermissionCategoryPermission.Permission" };
    }
}
