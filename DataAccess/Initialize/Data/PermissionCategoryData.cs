using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Data;
using Domain.Entities.Identity;

namespace DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {

        public static List<PermissionCategory> BuildPermissionCategories(ApplicationDbContext context)
        {
            var permissions = context.Permissions.ToList();
            var add = permissions.First(c => c.Label == nameof(PermissionEnum.Add));
            var edit = permissions.First(c => c.Label == nameof(PermissionEnum.Edit));
            var delete = permissions.First(c => c.Label == nameof(PermissionEnum.Delete));
            var list = permissions.First(c => c.Label == nameof(PermissionEnum.List));
            List<PermissionCategory> categories = new List<PermissionCategory>()
                {
                    new PermissionCategory()
                {
                    Label = "User",
                    VisibleLabel = "User",
                    Description = "",
                    PossiblePermissions = new List<PermissionCategoryPermission>()
                    {
                        new PermissionCategoryPermission()
                        {
                            Permission = add,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = edit,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission =delete,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = list,

                        },

                    }

                },
                new PermissionCategory()
                {
                    Label = "Role",
                    VisibleLabel = "Role",
                    Description = "",
                    PossiblePermissions = new List<PermissionCategoryPermission>()
                    {
                        new PermissionCategoryPermission()
                        {
                            Permission = add,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = list ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = edit,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = delete,

                        },

                    }

                }, new PermissionCategory()
                {
                    Label = "Language",
                    VisibleLabel = "Language",
                    Description = "",
                    PossiblePermissions = new List<PermissionCategoryPermission>()
                    {
                        new PermissionCategoryPermission()
                        {
                            Permission = list ,

                        },

                        new PermissionCategoryPermission()
                        {
                            Permission = edit,
                        },

                    }

                },

            };



            return categories;
        }


    }
}
